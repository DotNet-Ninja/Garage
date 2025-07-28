using Garage.Entities;

namespace Garage.Services;

public interface ISiteService
{
    Task<List<SiteSummary>> ListSitesAsync(bool force=false);
    Task DeleteSiteAsync(string slug);
    Task<Site?> GetSiteAsync(string slug);
    Task SaveAsync(Site site);
}