using Garage.Configuration;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Garage.TagHelpers;

[HtmlTargetElement("version")]
public class VersionTagHelper : TagHelper
{
    private readonly Settings _settings;

    public VersionTagHelper(Settings settings)
    {
        _settings = settings;
    }

    public override Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
    {
        output.TagName = null;
        output.Content.Clear();
        output.Content.SetHtmlContent(_settings.Version);
        return base.ProcessAsync(context, output);
    }
}