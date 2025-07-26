using System.Security.Claims;
using Garage.Entities;

namespace Garage.Services;

public interface IUserPageService
{
    Task<UserPageCollection> GetUserPageCollectionAsync(ClaimsPrincipal user);
    Task SaveAsync(ClaimsPrincipal user, UserPageCollection userPageCollection);
}