//using ASC.Web.Configuration;
//using ASC.Web.Models;
//using ASC.Web.Services;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.Extensions.Options;
//using System.Diagnostics;

//namespace ASC.Web.Controllers
//{
//    public class HomeController : Controller
//    {
//        private readonly ILogger<HomeController> _logger;

//        private IOptions<ApplicationSettings> _settings;
//        //public HomeController(ILogger<HomeController> logger, IOptions<ApplicationSettings>settings)
//        //{
//        //    _logger = logger;
//        //    _settings = settings;
//        //}
//        public HomeController(IOptions<ApplicationSettings> settings)
//        {
//            _settings = settings;
//        }
//        public IActionResult Index()
//        {
//            ViewBag.Title = _settings.Value.ApplicationTitle;
//            return View();
//        }
//        public IActionResult Index([FromServices] IEmailSender emailSender)
//        {
//            var emailService = this.HttpContext.RequestServices.GetService(typeof(IEmailSender)) as IEmailSender;
//            ViewBag.Title = _settings.Value.ApplicationTitle;
//            return View();
//        }

//        public IActionResult Privacy()
//        {
//            return View();
//        }

//        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
//        public IActionResult Error()
//        {
//            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
//        }
//    }
//}
using ASC.Web.Configuration;
using ASC.Web.Models;
using ASC.Web.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Diagnostics;
using ASC.Utilities;

namespace ASC.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IOptions<ApplicationSettings> _settings;
        private readonly IEmailSender _emailSender;

        public HomeController(
            ILogger<HomeController> logger,
            IOptions<ApplicationSettings> settings,
            IEmailSender emailSender)
        {
            _logger = logger;
            _settings = settings;
            _emailSender = emailSender;
        }

        public IActionResult Index()
        {
            // Set Session
            HttpContext.Session.SetSession("Test", _settings.Value);

            // Get Session
            var settings = HttpContext.Session.GetSession<ApplicationSettings>("Test");

            // Usage of IOptions
            ViewBag.Title = settings.ApplicationTitle;

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