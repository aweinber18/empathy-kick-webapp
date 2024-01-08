using EmpathyKick.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;
using System.Net;
using EmpathyKick.Data;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;

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

            FormattableString sql = FormattableStringFactory.Create($"SELECT TOP 9 o.OrganizationID AS OrganizationId, o.OrganizationName,o.AddressID AS AddressId, o.TaxID AS TaxId, o.LogoFile, o.ThemeColor, SUM(d.Amount) AS TotalDonations FROM Organization o INNER JOIN Donation d ON o.OrganizationID = d.OrganizationID GROUP BY o.OrganizationID, o.OrganizationName, o.AddressID, o.TaxID, o.LogoFile, o.ThemeColor ORDER BY TotalDonations DESC;");
            var topOrganizations = _context.Organization.FromSqlRaw(sql.Format, parameters: sql.GetArguments()).ToArray();
            var UserloggedIn = HttpContext.Session.GetString("UserId");
            Organization[] recOrgs = new Organization[9];
            if (UserloggedIn != null && UserloggedIn != "")
            {

                recOrgs = _context.getOrganizationsDonatedTo(Int32.Parse(UserloggedIn));
            }

            Tuple<Organization[], Organization[]> tuple = new Tuple<Organization[], Organization[]>(topOrganizations, recOrgs);


            return View(tuple);
        }

        public IActionResult Privacy()
        {

            return View();
        }
        public IActionResult CampaignProfilePage(string organizationName)
        {
            ViewData["OrganizationName"] = organizationName;


            FormattableString sql = FormattableStringFactory.Create($"SELECT  o.OrganizationID AS OrganizationId, o.OrganizationName,o.AddressID AS AddressId, o.TaxID AS TaxId, o.LogoFile, o.ThemeColor, SUM(d.Amount) AS TotalDonations FROM Organization o INNER JOIN Donation d ON o.OrganizationID = d.OrganizationID where o.OrganizationName = '{organizationName}'  GROUP BY o.OrganizationID, o.OrganizationName, o.AddressID, o.TaxID, o.LogoFile, o.ThemeColor ORDER BY TotalDonations DESC;"); ;
            var organization = _context.Organization.FromSqlRaw(sql.Format, parameters: sql.GetArguments()).ToList();
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
            var orgs = _context.Organization.ToArray();
            ViewBag.Organizations = orgs;
            return View();
        }

        [HttpPost]
        public IActionResult Register(User user, Addresses address, string checkboxState)
        {

            var get_user = _context.User.FirstOrDefault(p => p.Username == user.Username);
            if (get_user == null)
            {
                //_context.Address.Add(address);
                //_context.SaveChanges();

                user.AddressID = address.AddressId;
                _context.User.Add(user);
                _context.SaveChanges();
                bool isAdmin = (Request.Form["isEmpathyAdmin"] == "on");
                if (isAdmin)
                {
                    EmpathyAdmin prospectiveAdmin = new EmpathyAdmin();
                    prospectiveAdmin.UserID = user.UserID;
                    prospectiveAdmin.AuthorizationDate = null;
                    prospectiveAdmin.DeauthorizationDate = null;
                    _context.EmpathyAdmin.Add(prospectiveAdmin);
                    _context.SaveChanges();
                }
                if (checkboxState != null)
                {
                    var checkboxStateDictionary = JsonConvert.DeserializeObject<Dictionary<string, bool>>(checkboxState);

                    if (checkboxStateDictionary.Count > 0)
                    {
                        foreach (var pair in checkboxStateDictionary)
                        {
                            if (pair.Value == true)
                            {
                                OrganizationAdmin prospectiveAdmin = new OrganizationAdmin();
                                prospectiveAdmin.OrganizationID = Int32.Parse(pair.Key);
                                prospectiveAdmin.UserID = user.UserID;
                                prospectiveAdmin.AuthorizationDate = null;
                                prospectiveAdmin.DeauthorizationDate = null;
                                _context.OrganizationAdmin.Add(prospectiveAdmin);
                                _context.SaveChanges();
                            }
                        }
                    }
                }

            }
            else
            {
                ViewBag.Message = "Username '" + user.Username + "' already exists ";
                return View();
            }

            ModelState.Clear();

            var newUser = "Successfully Registered " + user.Username;
            HttpContext.Session.SetString("NewUser", newUser);
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


                var get_user = _context.User.SingleOrDefault(p => p.Username == user.Username
                && p.Password == user.Password);
                if (get_user != null)
                {
                    HttpContext.Session.SetString("UserId", get_user.UserID.ToString());
                    HttpContext.Session.SetString("Username", get_user.Username.ToString());
                    HttpContext.Session.SetString("RedirectFromLogin", "True");
                    if (_context.IsEmpathyAdmin(get_user.UserID))
                    {
                        HttpContext.Session.SetString("EmpathyAdmin", "True");
                    }
                    else
                    {
                        HttpContext.Session.SetString("EmpathyAdmin", "False");
                    }
                    OrganizationAdmin[] orgsAdminOf = _context.IsOrganizationAdmin(get_user.UserID);
                    if (orgsAdminOf.Length > 0)
                    {
                        HttpContext.Session.SetString("OrganizationAdmin", "True");
                        string IdsOfOrgsAdminOf = "";
                        for (int i = 0; i < orgsAdminOf.Length; i++)
                        {
                            IdsOfOrgsAdminOf += orgsAdminOf[i].OrganizationID.ToString() + "$";
                        }
                        HttpContext.Session.SetString("IDsOfOrganizationsThatAdminOf", IdsOfOrgsAdminOf);
                    }
                    else
                    {
                        HttpContext.Session.SetString("OrganizationAdmin", "False");
                        HttpContext.Session.SetString("IDsOfOrganizationsThatAdminOf", "");
                    }
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.Message = "Username or Password does not match";
                }

            }
            return View();
        }

        public ActionResult LogOut()
        {
            HttpContext.Session.SetString("EmpathyAdmin", "");
            HttpContext.Session.SetString("OrganizationAdmin", "");
            HttpContext.Session.SetString("IDsOfOrganizationsThatAdminOf", "");
            HttpContext.Session.SetString("UserId", "");
            HttpContext.Session.SetString("Username", "");
            HttpContext.Session.SetString("RedirectFromLogin", "False");
            HttpContext.Session.SetString("RedirectFromLogOut", "True");
            return RedirectToAction("Index");
        }
        public IActionResult PendingEAView()
        {
            var pendingIDs = _context.EmpathyAdmin.Where(admin => admin.AuthorizationDate == null).Select(admin => admin.UserID).ToList();
            User[] UsersRequestingEAship = _context.User.Where(user => pendingIDs.Contains(user.UserID)).ToArray();
            EmpathyAdmin[] pendingAdmins = _context.EmpathyAdmin.Where(admin => admin.AuthorizationDate == null).ToArray();
            Tuple<User[], EmpathyAdmin[]> tuple = new Tuple<User[], EmpathyAdmin[]>(UsersRequestingEAship, pendingAdmins);
            return View("PendingEAView", tuple);
        }

        [HttpPost]
        public IActionResult ApproveEmpathyAdmin(int userId)
        {
            EmpathyAdmin[] admin = _context.EmpathyAdmin.Where(admin => admin.UserID == userId).Distinct().ToArray();
            admin[0].AuthorizationDate = DateTime.Now;
            _context.SaveChanges();
            return RedirectToAction("PendingEAView");
        }

        public IActionResult PendingOAView()
        {
            var pendingIDs = _context.OrganizationAdmin
             .Where(admin => admin.AuthorizationDate == null)
            .Select(admin => new { admin.UserID, admin.OrganizationID })
            .ToList();

            // Retrieve users and organizations based on the lists
            var users = _context.User.ToList();  // Fetch all users
            var organizations = _context.Organization.ToList();  // Fetch all organizations
            var associations = _context.OrganizationAdmin.ToList();
            // Retrieve associations between users and organizations
            var associations2 = associations
                .Where(assoc => pendingIDs.Any(p => p.UserID == assoc.UserID && p.OrganizationID == assoc.OrganizationID))
                .ToList();

            // Perform an inner join based on the associations
            var result = (from assoc in associations2
                          join user in users on assoc?.UserID equals user?.UserID
                          join org in organizations on assoc?.OrganizationID equals org?.OrganizationId
                          where user != null && org != null
                          select (User: user, Organization: org)
                ).ToArray();

            return View("PendingOAView", result);
        }

        [HttpPost]
        public IActionResult ApproveOrganizationAdmin(int userId, int orgId)
        {
            OrganizationAdmin[] admin = _context.OrganizationAdmin.Where(admin => admin.UserID == userId && admin.OrganizationID == orgId).Distinct().ToArray();
            admin[0].AuthorizationDate = DateTime.Now;
            _context.SaveChanges();
            return RedirectToAction("PendingEAView");
        }

        public ActionResult UserProfilePage()
        {
            User theUser = _context.User.Include(u => u.Address).Where(user => user.UserID == Int32.Parse(HttpContext.Session.GetString("UserId"))).Single();
            return View(theUser);
        }

        [HttpPost]
        public ActionResult UpdateUser(User updatedUser)
        {
            User notUpdated = _context.User.Find(updatedUser.UserID);
            if (notUpdated != null)
            {
                notUpdated.Username = updatedUser.Username;
                notUpdated.Password = updatedUser.Password;
                notUpdated.FirstName = updatedUser.FirstName;
                notUpdated.LastName = updatedUser.LastName;
                notUpdated.Phone = updatedUser.Phone;
                notUpdated.Email = updatedUser.Email;
                notUpdated.ThemeColor = updatedUser.ThemeColor;

                if (updatedUser.Address != null)
                {
                    notUpdated.Address = updatedUser.Address;

                    if (updatedUser.Address.Address != null)
                    {
                        notUpdated.Address.Address = updatedUser.Address.Address;
                    }
                    if (updatedUser.Address.City != null)
                    {
                        notUpdated.Address.City = updatedUser.Address.City;
                    }
                    if (updatedUser.Address.Region != null)
                    {
                        notUpdated.Address.Region = updatedUser.Address.Region;
                    }
                    if (updatedUser.Address.Country != null)
                    {
                        notUpdated.Address.Country = updatedUser.Address.Country;
                    }
                } else
                {
                    notUpdated.Address = null;
                }
                _context.SaveChanges();
                return RedirectToAction("UserProfilePage");
            }
            return View(UpdateUser);
        }
        public IActionResult EATableSelectionView()
        {
            //pass in the context to the view so that we can access the database
            return View("EATableSelectionView", _context);
        }
        [HttpPost]
        public ActionResult DonateHandler(int donationAmount, int  organizationID)
        {
          

            try
            {
                Donations newDonation = new Donations();
                newDonation.Amount = donationAmount;
                newDonation.OrganizationId = organizationID;
                _context.Donation.Add(newDonation);
                _context.SaveChanges();

                return Json(new { success = true});
            }
            catch (Exception ex)
            {
                return Json(new { success = false, error = ex.Message });
            }
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