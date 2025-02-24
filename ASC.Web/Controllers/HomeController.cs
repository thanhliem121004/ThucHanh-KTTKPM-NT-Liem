using System.Diagnostics;
using ASC.Web.Configuration;
using ASC.Web.Models;
using ASC.Web.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace ASC.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private IOptions<ApplicationSettings> _setting;

        public HomeController(ILogger<HomeController> logger, IOptions<ApplicationSettings> setting)
        {
            _logger = logger;
            _setting = setting;
        }

        public IActionResult Index()
        {
            ViewBag.Title = _setting.Value.ApplicationTitle;

            var emailSener = HttpContext.RequestServices.GetService<IEmailSender>();
            var smsSender = HttpContext.RequestServices.GetService<ISmsSender>();

            return View();

        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult Dashboard()
        {
            return View();
        }
    }
}
