namespace Garage.Services;

public class SystemTimeProvider : ITimeProvider
{
    public DateTimeOffset Now => DateTimeOffset.Now;
    public DateTimeOffset RequestTime { get; } = DateTimeOffset.Now;
}