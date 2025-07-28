using System.IO.Abstractions;
using System.Text.Json;
using Garage.Configuration;
using Garage.Constants;
using Garage.Data;
using Garage.Entities;
using Microsoft.Extensions.Caching.Memory;

namespace Garage.Services;

public class SiteService: ISiteService
{
    private readonly IFileSystem _files;
    private readonly IMemoryCache _cache;
    private readonly IWebHostEnvironment _host;
    private readonly IEmbeddedDataReader _reader;
    private readonly Settings _settings;
    private readonly ILogger<SiteService> _logger;

    public SiteService(IFileSystem files, IMemoryCache cache, IWebHostEnvironment host, IEmbeddedDataReader reader, 
        Settings settings, ILogger<SiteService> logger)
    {
        _files = files;
        _cache = cache;
        _host = host;
        _reader = reader;
        _settings = settings;
        _logger = logger;
    }

    public async Task<List<SiteSummary>> ListSitesAsync(bool force=false)
    {
        // Check if Cached sites exist
        if (!force)
        {
            if (_cache.TryGetValue(CacheKeys.SitesList, out List<SiteSummary>? cached) && cached is not null)
            {
                return cached;
            }
        }

        var sites = new List<SiteSummary>();
        var dataDirectory = GetFileDirectory();
        if (!_files.Directory.Exists(dataDirectory))
        {
            _files.Directory.CreateDirectory(dataDirectory);
        }
        var paths = _files.Directory.GetFiles(dataDirectory, "*.json") ?? [];
        foreach (var path in paths)
        {
            // Read file
            try
            {
                var json = await _files.File.ReadAllTextAsync(path);
                var site = JsonSerializer.Deserialize<SiteSummary>(json, Defaults.JsonOptions);
                if (site != null)
                {
                    sites.Add(site);
                }
                else
                {
                    _logger.LogWarning("Failed to deserialize site from file: {FilePath}", path);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to deserialize site from file: {FilePath}", path);
            }
        }
        _cache.Set(CacheKeys.SitesList, sites, _settings.CacheTimeout);
        return sites;
    }

    public Task DeleteSiteAsync(string slug)
    {
        var sourcePath = GetFilePath(slug);
        var trashPath = $"{sourcePath}.trash";
        if (_files.File.Exists(sourcePath))
        {
            try
            {
                _files.File.Move(sourcePath, trashPath, true);
                _logger.LogInformation("Deleted site file: {FilePath}", sourcePath);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to delete site file: {FilePath}", sourcePath);
                throw;
            }
        }
        else
        {
            _logger.LogWarning("Site file not found for deletion: {FilePath}", sourcePath);
        }

        return ListSitesAsync(true);
    }

    public async Task<Site?> GetSiteAsync(string slug)
    {
        // Check the cache for the site object  
        if (_cache.TryGetValue(slug, out Site? cachedSite) && cachedSite is not null)
        {
            return cachedSite;
        }

        // Check the disk for the file  
        var filePath = GetFilePath(slug);
        if (_files.File.Exists(filePath))
        {
            try
            {
                var json = await _files.File.ReadAllTextAsync(filePath);
                var site = JsonSerializer.Deserialize<Site>(json, Defaults.JsonOptions);

                if (site != null)
                {
                    // Add the site to the cache  
                    await CacheSiteAsync(site);
                    return site;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to deserialize site from file: {FilePath}", filePath);
            }
        }
        else if (slug.Equals(Defaults.Sites.Slug, StringComparison.CurrentCultureIgnoreCase))
        {
            var defaultSite = _reader.ReadDefaultSite();
            await SaveAsync(defaultSite);
            return defaultSite;
        }

        // We tried and couldn't find anything matching, return null
        return null;
    }

    public async Task SaveAsync(Site site)
    {
        if((string.IsNullOrWhiteSpace(site.DefaultPage) || !site.Pages.Any(x=>x.Slug.Equals(site.DefaultPage))) && site.Pages.Any())
        {
            site.DefaultPage = site.Pages.ToSortedList().First().Slug;
        }

        var dataDir = GetFileDirectory();
        if (!_files.Directory.Exists(dataDir))
        {
            _files.Directory.CreateDirectory(dataDir);
        }
        var filePath = GetFilePath(site.Slug);
        var json = JsonSerializer.Serialize(site, Defaults.JsonOptions);
        await _files.File.WriteAllTextAsync(filePath, json);
        await CacheSiteAsync(site);
    }

    private string GetFilePath(string slug)
    {
        var dataDir = GetFileDirectory();
        return _files.Path.Combine(dataDir, $"{slug}.json");
    }
    
    private string GetFileDirectory()
    {
        return _files.Path.Combine(_host.WebRootPath, "data");
    }

    private async Task CacheSiteAsync(Site site)
    {
        _cache.Set(site.Slug, site, _settings.CacheTimeout);
        if((_cache.TryGetValue(CacheKeys.SitesList, out List<SiteSummary>? sites) && sites is not null))
        {
            var index = sites.FindIndex(x => x.Id == site.Id);
            if (index >= 0)
            {
                sites[index] = site;
                _cache.Set(CacheKeys.SitesList, sites, _settings.CacheTimeout);
                return;
            }
            sites.Add(site);
            _cache.Set(CacheKeys.SitesList, sites, _settings.CacheTimeout);
        }
        else
        {
            await ListSitesAsync(true);
        }
    }
}