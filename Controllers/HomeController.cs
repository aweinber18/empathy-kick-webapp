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
        [HttpPost]
        public IActionResult Privacy()
        {

            return View("Privacy");
        }
        public IActionResult ProfilePage(string organizationName)
        {
            ViewData["OrganizationName"] = organizationName;


            FormattableString sql = FormattableStringFactory.Create($"SELECT  o.OrganizationID AS OrganizationId, o.OrganizationName,o.AddressID AS AddressId, o.TaxID AS TaxId, o.LogoFile, o.ThemeColor, SUM(d.Amount) AS TotalDonations FROM Organization o INNER JOIN Donation d ON o.OrganizationID = d.OrganizationID where o.OrganizationName = '{organizationName}'  GROUP BY o.OrganizationID, o.OrganizationName, o.AddressID, o.TaxID, o.LogoFile, o.ThemeColor ORDER BY TotalDonations DESC;"); ;
            var organization = _context.Organizations.FromSqlRaw(sql.Format, parameters: sql.GetArguments()).ToList();
            Organization myOrg = organization[0];
            var number = myOrg.AddressId;

            FormattableString addressSQL = FormattableStringFactory.Create($"SELECT Address FROM Address WHERE AddressID = '{number}' ;");
            FormattableString regionSQL = FormattableStringFactory.Create($"SELECT Region FROM Address WHERE AddressID = '{number}' ;");
            FormattableString citySQL = FormattableStringFactory.Create($"SELECT City FROM Address WHERE AddressID = '{number}' ;");
            FormattableString countrySQL = FormattableStringFactory.Create($"SELECT Country FROM Address WHERE AddressID = '{number}' ;");
            FormattableString amountDonatedSQL = FormattableStringFactory.Create($" Select SUM(Amount) AS TotalDonations FROM Donation where OrganizationID = '{myOrg.OrganizationId}' GROUP BY OrganizationID");

            var address = _context.Database.SqlQuery<string>(addressSQL).ToList();
            var region = _context.Database.SqlQuery<string>(regionSQL).ToList();
            var city = _context.Database.SqlQuery<string>(citySQL).ToList();
            var country = _context.Database.SqlQuery<string>(countrySQL).ToList();
            var amount = _context.Database.SqlQuery<Decimal>(amountDonatedSQL).ToList();
            ViewData["address"] = address[0];
            ViewData["region"] = region[0];
            ViewData["city"] = city[0];
            ViewData["country"] = country[0];
            ViewData["amountDonated"] = amount[0];

            return View(myOrg);
        }


     
        public ActionResult Register()
        {

            
                return View();

        }

        [HttpPost]
        public IActionResult Register(User user)
        {
            var get_user = _context.User.FirstOrDefault(p => p.Username == user.Username);
            if (get_user == null)
            {
                _context.User.Add(user);
                _context.SaveChanges();
            }
            else
            {
                ViewBag.Message = "Username '" + user.Username +"' already exists ";
                return View();
            }
        
        ModelState.Clear();
        
		var newUser = "Successfully Registered " + user.Username;
        HttpContext.Session.SetString("NewUser",newUser);
        return RedirectToAction("Login");
       }
        public ActionResult Login()
        {

            return View();
        }
        [HttpPost]
        public ActionResult Login(User user)
        {
           
            {
                var get_user = _context.User.Single(p => p.Username == user.Username
                && p.Password == user.Password);
                if (get_user != null)
                {
                    HttpContext.Session.SetString("UserId", get_user.UserId.ToString());
                    HttpContext.Session.SetString("Username", get_user.Username.ToString());
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.Message = "Username or Password does not match";
                }

            }
            return View();
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