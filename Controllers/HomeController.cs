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

        public IActionResult EAColumnSelectionView()
        {
            List<string> tableNames = new List<string>();
            // Get all keys in the form collection
            var formKeys = Request.Form.Keys;

            // Iterate over the keys to find the dynamically named checkboxes
            foreach (var key in formKeys)
            {
                // Check if the key represents a dynamically named checkbox
                if (key.StartsWith("Checkbox"))
                {
                    // Get the value of the checkbox (will be "on" if checked)
                    string checkboxValue = Request.Form[key];

                    // Process the checkbox data as needed
                    bool isChecked = checkboxValue == "on";
                    string checkboxName = key.Substring("Checkbox".Length); // Extract the number from the key

                    // Now you can use isChecked and checkboxNumber as needed
                    // For example, you might want to store this information or perform some other action
                    tableNames.Add(checkboxName);
                }
            }

            return View("EAColumnSelectionView", tableNames);
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