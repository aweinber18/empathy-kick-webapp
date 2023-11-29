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
            //hard code implementation for testing
           // organizationsList = new []{new Organization(1, "18ForIsrael",5,0, "18ForIsrael(Custom).png","null"), new Organization(2, "Aliya Institute",6,0, "AliyaInstitute(Custom).jpeg","none"), new Organization(3, "BBQ4IDF",9,0, "BBQ4IDF(Custom).jpeg","none"), new Organization(6, "Healing Hands", 10, 0, "healingHandsJewishHearts.jpeg", "none"), new Organization(7, "Light Up Chicago", 11, 0, "LightUpChicago(Custom).jpeg", "none"), new Organization(8, "Ohr Hatorah", 12, 0, "OhrHatorahYeshiva(Custom).jpg", "none"), new Organization(9, "school", 13, 0, "school(Custom).png", "none"), new Organization(11, "Standing for Israel", 14, 0, "StandingForIsrael.jpeg", "none"), new Organization(12, "Tzevet Hatzolah", 15, 0, "TzevetHatzolah(Custom).jpeg", "none"), new Organization(13, "Zaka", 16, 0, "zaka(Custom).jpeg", "none") };
        }

        public IActionResult Index()
        {
            if (_context.Database.GetDbConnection().State != System.Data.ConnectionState.Open)
            {
                _context.Database.GetDbConnection().Open();
            }
 
            FormattableString sql = FormattableStringFactory.Create($"SELECT TOP 9 o.OrganizationID AS OrganizationId, o.OrganizationName,o.AddressID AS AddressId, o.TaxID AS TaxId, o.LogoFile, o.ThemeColor, SUM(d.Amount) AS TotalDonations FROM Organization o INNER JOIN Donation d ON o.OrganizationID = d.OrganizationID GROUP BY o.OrganizationID, o.OrganizationName, o.AddressID, o.TaxID, o.LogoFile, o.ThemeColor ORDER BY TotalDonations DESC;"); ;
            var topOrganizations = _context.Organizations.FromSqlRaw(sql.Format, parameters: sql.GetArguments()).ToList();
            

            return View(topOrganizations);


            //return View();

        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Register()
        {
            return View("Register");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}