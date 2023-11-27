using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace EmpathyKick.Data
{
    public class LocalDbContext : DbContext
    {
        //Database database = new Database();
        public IList<string> GetTableNames() {
            return new List<string> { "User", "Address", ""};
        }

        public IList<string> GetColumnNames(IList<string> tables) {
            return new List<string> { "AddressID", "Address", "City", "Region", "Country", "ZIP" };
        }
    }
}
