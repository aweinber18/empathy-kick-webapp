using EmpathyKick.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace EmpathyKick.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Register()
        {
            return View("Register");
        }

        public IActionResult EATableSelectionView()
        {
            return View("EATableSelectionView");
        }

        public IActionResult ColumnNamesView()
        {
            return View("EAColumnSelectionView");
        }

        public IActionResult EADataView()
        {
            return View("EADataView");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}