using Garage.Configuration;
using Garage.Constants;

namespace Garage.Services;

public class StateService : IStateService
{
    private readonly HttpRequest _request;
    private readonly HttpResponse _response;
    private readonly ITimeProvider _time;
    private readonly Settings _settings;
    private string? _siteSlug = null;

    public StateService(IHttpContextAccessor httpContextAccessor, ITimeProvider time, Settings settings)
    {
        _request = httpContextAccessor.HttpContext?.Request ?? throw new ArgumentNullException(nameof(httpContextAccessor.HttpContext));
        _response = httpContextAccessor.HttpContext?.Response ?? throw new ArgumentNullException(nameof(httpContextAccessor.HttpContext));
        _time = time;
        _settings = settings;
    }

    public string? SiteSlug
    {
        get
        {
            return _siteSlug ??=_request.Cookies[CookieKeys.SiteSlug];
        }
        set
        {
            _siteSlug = value;
            if (value == null)
            {
                // Remove the cookie if value is null
                _response.Cookies.Delete(CookieKeys.SiteSlug);
            }
            else
            {
                _response.Cookies.Append(CookieKeys.SiteSlug, value, new CookieOptions
                {
                    HttpOnly = true,
                    SameSite = SameSiteMode.Lax,
                    Expires = _time.Now.AddDays(_settings.PermanentCookieTimeoutDays),
                    Path = "/"
                });
            }
        }
    }
}
