namespace Garage.Services;

public interface ITimeProvider
{
    DateTimeOffset Now { get; }
    DateTimeOffset RequestTime { get; }
}