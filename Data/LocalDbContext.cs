using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;
using System.Text;

namespace EmpathyKick.Data
{
    public class LocalDbContext : DbContext
    {
        //Database database = new Database();
        public IList<string> GetTableNames() {
            FormattableString sql = FormattableStringFactory.Create($"SELECT table_name " +
                                                        $"FROM information_schema.tables" +
                                                        $"WHERE TABLE_NAME NOT LIKE 'spt%' " +
                                                        $"AND NOT LIKE 'MSreplication%';");

            /*var tables = _context.Tables.FromSqlRaw(sql.Format, sql.GetArguments()).ToList();*/

            return new List<string> { "User", "Address", "EmpathyAdmin", "Organization", "OrganizationAdmin",
                "Card", "CardRegistration", "Tag", "TagRegistration", "Donation", "Invoice", "Product"};
        }

        public IList<string> GetColumnNames(IList<string> tables) {
            var tableStringBuilder = new StringBuilder();
            for (int i = 0; i < tables.Count; i++)
            {
                tableStringBuilder.Append("'");
                tableStringBuilder.Append(tables[i]);
                tableStringBuilder.Append("'");

                if (i < tables.Count - 1) 
                    tableStringBuilder.Append(", ");
            }
            string tablesString = "(" + tableStringBuilder.ToString() + ")";
            FormattableString sql = FormattableStringFactory.Create($"SELECT COLUMN_NAME " +
                                                        $"FROM information_schema.COLUMNS " +
                                                        $"WHERE TABLE_NAME IN " + 
                                                        tablesString + 
                                                        ";");
            /*var tables = _context.Tables.FromSqlRaw(sql.Format, sql.GetArguments()).ToList();*/
            return new List<string> { "AddressID", "Address", "City", "Region", "Country", "ZIP" };
        }
    }
}
