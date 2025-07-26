using DotNetNinja.AutoBoundConfiguration;

namespace Garage.Configuration;


[AutoBind("Authentication")]
public class AuthenticationSettings
{
    public string Domain { get; set; } = string.Empty;
    public string ClientId { get; set; } = string.Empty;
}