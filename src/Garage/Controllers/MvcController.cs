using Garage.Models;
using Garage.Services;
using Microsoft.AspNetCore.Mvc;

namespace Garage.Controllers;

public abstract class MvcController<TController>: Controller where TController: Controller
{
    private readonly IMvcContext<TController> _context;

    protected MvcController(IMvcContext<TController> context)
    {
        _context = context;
    }

    protected ILogger Logger => _context.Logger;
    protected ITimeProvider Time => _context.Time;
    protected IWebHostEnvironment Host => _context.Host;
    protected IStateService State => _context.State;

    protected IActionResult NotFoundView(string message)
    {
        var model = new NotFoundModel
        {
            Message = message
        };
        var result = new ViewResult
        {
            ViewName = "NotFound",
            StatusCode = 404,
            ViewData = new Microsoft.AspNetCore.Mvc.ViewFeatures.ViewDataDictionary(
                metadataProvider: new Microsoft.AspNetCore.Mvc.ModelBinding.EmptyModelMetadataProvider(),
                modelState: ModelState)
            {
                Model = model
            }
        };
        return result;
    }
}