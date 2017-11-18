using System;
using System.Diagnostics;
using AspNetCoreWebApp.Web.Models;
using AspNetCoreWebApp.Web.Settings;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace AspNetCoreWebApp.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<HomeController> _logger;
        private readonly IOptions<AppSettings> _settings;

        public HomeController(ILogger<HomeController> logger, IOptions<AppSettings> settings, IConfiguration configuration)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _settings = settings ?? throw new ArgumentNullException(nameof(settings));
        }

        public IActionResult Index()
        {
            _logger.LogInformation("Index page was shown.");
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = $"Your application about page. {Environment.NewLine} Value from config: {_settings.Value.BaseDomain} {Environment.NewLine} Value from custom configuration file: {_configuration.GetValue<string>("Timeouts:ApplicationTimeout")}";

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