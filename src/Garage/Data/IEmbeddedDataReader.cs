using Garage.Entities;

namespace Garage.Data;

public interface IEmbeddedDataReader
{
    Site ReadDefaultSite();
}