using Microsoft.AspNetCore.Authentication;

namespace Garage.Services;

public interface ISignInService
{
    Task ChallengeAsync(string scheme, AuthenticationProperties properties);
    Task SignOutAsync(string scheme, AuthenticationProperties properties);
    Task SignOutAsync(string scheme);
}