using Microsoft.AspNetCore.Mvc;

namespace Manager.WebApp.Models.Business
{
    public class TestController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
