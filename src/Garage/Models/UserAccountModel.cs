using System.Security.Claims;
using Garage.Constants;
using ClaimTypes = Garage.Constants.ClaimTypes;

namespace Garage.Models;

public class UserAccountModel
{
    public UserAccountModel()
    {
    }

    public UserAccountModel(ClaimsPrincipal principal)
    {
        UserName = principal.Identity?.Name ?? Defaults.Users.UserName;
        var pictureClaim = principal.Claims.FirstOrDefault(c=>c.Type== ClaimTypes.Picture);
        Picture = (pictureClaim is not null) ? pictureClaim.Value : Defaults.Users.Picture;
        Roles = principal.Claims.Where(c => c.Type == ClaimTypes.Role)
                .Select(v => v.Value).Where(r=>r.StartsWith(Constants.Roles.AppRoleKey)).ToList();
    }

    public string UserName { get; set; } = Defaults.Users.UserName;
    public string Picture { get; set; } = string.Empty;
    public List<string> Roles { get; set; } = new();

    public string RolesString => (Roles.Any()) ? string.Join(',', Roles) : "No roles defined";
}