using System.Text.Json;
using Garage.Constants;
using Garage.Entities;

namespace Garage.Data;

public class EmbeddedDataReader: IEmbeddedDataReader
{
    public Site ReadDefaultSite()
    {
        // read the embedded resource "Garage.Data.DefaultSite.json"
        var assembly = typeof(EmbeddedDataReader).Assembly;
        using var stream = assembly.GetManifestResourceStream("Garage.Data.Default.json");
        if (stream == null)
        {
            throw new InvalidOperationException("Embedded resource 'Garage.Data.DefaultSite.json' not found.");
        }
        using var reader = new StreamReader(stream);
        var json = reader.ReadToEnd();
        var site = JsonSerializer.Deserialize<Site>(json, Defaults.JsonOptions);
        return site??throw new ApplicationException("Error reading embedded default site data.");
    }
}