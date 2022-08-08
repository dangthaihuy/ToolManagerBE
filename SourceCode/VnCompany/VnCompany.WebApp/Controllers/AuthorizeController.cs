using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using VnCompany.WebApp.Helpers;
using VnCompany.WebApp.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace NetCoreMVC.Controllers
{
    public class AuthorizeController : Controller
    {
        private readonly ILogger<AuthorizeController> _logger;

        public AuthorizeController(ILogger<AuthorizeController> logger)
        {
            _logger = logger;
            
        }

        public IActionResult Index()
        {
            ViewBag.RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier;

            return View();
        }
       
    }
}
