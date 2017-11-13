using System;
using System.Diagnostics;
using AspNetCoreWebApp.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace AspNetCoreWebApp.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public IActionResult Index()
        {
            _logger.LogInformation("Index page was shown.");
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            _logger.LogInformation("About page was shown.");
            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            _logger.LogInformation("Contact page was shown.");
            return View();
        }

        public IActionResult Error()
        {
            var requestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier;
            _logger.LogWarning($"Error page was shown, RequestId = {requestId}.");
            return View(new ErrorViewModel { RequestId = requestId });
        }
    }
}