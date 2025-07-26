using System.IO.Abstractions;
using System.Security.Claims;
using System.Text.Json;
using Garage.Entities;
using Microsoft.Extensions.Caching.Memory;

namespace Garage.Services;

public class UserPageService: IUserPageService
{
    private readonly IFileSystem _files;
    private readonly IMemoryCache _cache;
    private readonly IWebHostEnvironment _host;
    private readonly ILogger<UserPageService> _logger;

    public UserPageService(IFileSystem files, IMemoryCache cache, IWebHostEnvironment host,
        ILogger<UserPageService> logger)
    {
        _files = files;
        _cache = cache;
        _host = host;
        _logger = logger;
    }

    public async Task<UserPageCollection> GetUserPageCollectionAsync(ClaimsPrincipal user)
    {
        // Get userId from ClaimsPrincipal extension method
        var userId = user.GetUserId();
        if (string.IsNullOrEmpty(userId))
        {
            // Return anonymous defaults if userId is not available
            return UserPageCollection.AnonymousDefaults;
        }

        // Try to get from cache
        if (_cache.TryGetValue(userId, out UserPageCollection? cachedCollection) && cachedCollection is not null)
        {
            return cachedCollection;
        }

        // Build file path: {wwwroot}/data/{userid}.json
        var dataDir = _files.Path.Combine(_host.WebRootPath, "data");
        var userFile = GetFilePath(user);

        if (_files.File.Exists(userFile))
        {
            try
            {
                await using var stream = _files.File.OpenRead(userFile);
                var collection = await JsonSerializer.DeserializeAsync<UserPageCollection>(stream);
                if (collection != null)
                {
                    _cache.Set(userId, collection, TimeSpan.FromMinutes(30));
                    return collection;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to read or deserialize user page collection for user {UserId}", userId);
            }
        }

        // If not found, return anonymous defaults
        return UserPageCollection.AnonymousDefaults;
    }

    public async Task SaveAsync(ClaimsPrincipal user, UserPageCollection userPageCollection)
    {
        var userId = user.GetUserId();
        if (string.IsNullOrEmpty(userId))
            throw new ArgumentException("UserId could not be determined from ClaimsPrincipal.", nameof(user));

        if (userPageCollection.UserId != userId)
            throw new InvalidOperationException("UserId in UserPageCollection does not match the authenticated user.");

        var dataDir = GetFileDirectory(user);
        if (!_files.Directory.Exists(dataDir))
        {
            _files.Directory.CreateDirectory(dataDir);
        }

        var userFile = GetFilePath(user);

        try
        {
            await using var stream = _files.File.Open(userFile, FileMode.Create, FileAccess.Write, FileShare.None);
            await JsonSerializer.SerializeAsync(stream, userPageCollection);
            _cache.Set(userId, userPageCollection, TimeSpan.FromMinutes(30));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to save user page collection for user {UserId}", userId);
            throw;
        }
    }

    private string GetFilePath(ClaimsPrincipal user)
    {
        var dataDir = GetFileDirectory(user);
        var userId = user.GetUserId().Replace("|", "-");
        return _files.Path.Combine(dataDir, $"{userId}.json");
    }

    private string GetFileDirectory(ClaimsPrincipal user)
    {
        return _files.Path.Combine(_host.WebRootPath, "data");
    }
}