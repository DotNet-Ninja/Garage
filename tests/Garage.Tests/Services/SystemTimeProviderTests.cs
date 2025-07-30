using Garage.Services;
using Shouldly;

namespace Garage.Tests.Services;

public class SystemTimeProviderTests
{
    [Fact]
    public void Now_ShouldReturnCurrentTime()
    {
        // Arrange
        var timeProvider = new SystemTimeProvider();
        var before = DateTimeOffset.Now;
        // Act
        var now = timeProvider.Now;
        // Assert
        var after = DateTimeOffset.Now;
        now.ShouldBeGreaterThanOrEqualTo(before);
        now.ShouldBeLessThanOrEqualTo(after);
    }

    [Fact]
    public void RequestTime_ShouldBeSetAtInstantiation()
    {
        // Arrange
        var before = DateTimeOffset.Now;
        var timeProvider = new SystemTimeProvider();
        var after = DateTimeOffset.Now;
        // Act
        var requestTime = timeProvider.RequestTime;
        // Assert
        requestTime.ShouldBeGreaterThanOrEqualTo(before);
        requestTime.ShouldBeLessThanOrEqualTo(after);
    }

    [Fact]
    public void RequestTime_ShouldNotChangeAfterInstantiation()
    {
        // Arrange
        var timeProvider = new SystemTimeProvider();
        var initialRequestTime = timeProvider.RequestTime;
        // Act
        Thread.Sleep(100); // Wait for a short period to ensure time has passed
        var laterRequestTime = timeProvider.RequestTime;
        // Assert
        initialRequestTime.ShouldBe(laterRequestTime);
    }
}