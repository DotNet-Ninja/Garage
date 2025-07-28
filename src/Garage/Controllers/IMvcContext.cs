using Garage.Services;
using Microsoft.AspNetCore.Mvc;

namespace Garage.Controllers;

public interface IMvcContext
{
    ILogger Logger { get; }
    ITimeProvider Time { get; }
    IWebHostEnvironment Host { get; }
    IStateService State { get; }
}

public interface IMvcContext<TController> : IMvcContext where TController : Controller
{

}