using Microsoft.EntityFrameworkCore;

namespace EmpathyKick.Data
{
    public class LocalDbContext : DbContext
    {
        //Database database = new Database();
        public IList<string> GetTableNames() {
            return new List<string> { "User", "Address", "EmpathyAdmin", "Organization", "OrganizationAdmin",
                "Card", "CardRegistration", "Tag", "TagRegistration", "Donation", "Invoice", "Product"};
        }

        public IList<string> GetColumnNames(IList<string> tables) {
            return new List<string> { "AddressID", "Address", "City", "Region", "Country", "ZIP" };
        }
    }
}
