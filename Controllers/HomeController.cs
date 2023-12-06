using EmpathyKick.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;

namespace EmpathyKick.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly MyDBContext _context;
        //public static Organization[] organizationsList;

        public HomeController(MyDBContext context, ILogger<HomeController> logger)
        {
            _context = context;
            _logger = logger;
        }
        [HttpGet]
        public IActionResult Index()
        {
            if (_context.Database.GetDbConnection().State != System.Data.ConnectionState.Open)
            {
                _context.Database.GetDbConnection().Open();
            }
 
            FormattableString sql = FormattableStringFactory.Create($"SELECT TOP 9 o.OrganizationID AS OrganizationId, o.OrganizationName,o.AddressID AS AddressId, o.TaxID AS TaxId, o.LogoFile, o.ThemeColor, SUM(d.Amount) AS TotalDonations FROM Organization o INNER JOIN Donation d ON o.OrganizationID = d.OrganizationID GROUP BY o.OrganizationID, o.OrganizationName, o.AddressID, o.TaxID, o.LogoFile, o.ThemeColor ORDER BY TotalDonations DESC;"); 
            var topOrganizations = _context.Organizations.FromSqlRaw(sql.Format, parameters: sql.GetArguments()).ToList();

            _context.Database.GetDbConnection().Close();
            return View(topOrganizations);


            //return View();

        }
       
        public IActionResult Privacy()
        {
            
            return View("Privacy");
        }

        public IActionResult Register()
        {
            return View("Register");
        }

        public IActionResult EATableSelectionView()
        {
          //pass in the context to the view so that we can access the database
            return View("EATableSelectionView",_context);
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
            var columnNames = _context.GetColumnNames(tableNames);
            return View("EAColumnSelectionView",columnNames);
        }

        public IActionResult EADataView()
        {
            var formKeys = Request.Form.Keys;
            List<string> columnNames = new List<string>();
            List<string> whereClauses = new List<string>();
              foreach (var key in formKeys)
            {
                if (key.StartsWith("Checkbox"))
                {
                    // Get the value of the checkbox (will be "on" if checked)
                    string checkboxValue = Request.Form[key];

                    // Process the checkbox data as needed
                    bool isChecked = checkboxValue == "on";
                    string checkboxName = key.Substring("Checkbox".Length); // Extract the number from the key

                    // Now you can use isChecked and checkboxNumber as needed
                    // For example, you might want to store this information or perform some other action
                    columnNames.Add(checkboxName);
                } else if (key.StartsWith("Textarea"))
                {
                    string textareaValue = Request.Form[key];

                    // Process the checkbox data as needed
                    string textareaName = key.Substring("Textarea".Length); // Extract the number from the key

                    whereClauses.Add(textareaName);
                }
            }
            
            return View("EADataView");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}