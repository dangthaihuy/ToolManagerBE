using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace NetCoreMVC.Controllers
{
    public class ErrorController : Controller
    {
        private readonly ILogger<ErrorController> _logger;

        public ErrorController(ILogger<ErrorController> logger)
        {
            _logger = logger;            
        }

        public IActionResult Index(int? statusCode = null)
        {
            if(!statusCode.HasValue)
                statusCode = 404;

            return View(statusCode.ToString());
        }

        public IActionResult PermissionDennied()
        {
            return View();
        }

        public IActionResult AccountLocked()
        {
            return View();
        }
    }
}
