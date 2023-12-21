using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;
using Microsoft.EntityFrameworkCore;

namespace EmpathyKick.Models
{
    public class MyDBContext : DbContext
    {

        public MyDBContext(DbContextOptions<MyDBContext> options) : base(options)
        {
        }

        public DbSet<Organization> Organization { get; set; }
        public DbSet<Addresses> Address { get; set; }
        public DbSet<Donations> Donation { get; set; }
        public DbSet<User> User { get; set; }
        public DbSet<EmpathyAdmin> EmpathyAdmin { get; set; }
        public DbSet<OrganizationAdmin> OrganizationAdmin { get; set; }

        //    public IList<string> GetColumnNames(IList<string> tables)
        //    {
        //        var tableStringBuilder = new StringBuilder();
        //        for (int i = 0; i < tables.Count; i++)
        //        {
        //            tableStringBuilder.Append("'");
        //            tableStringBuilder.Append(tables[i]);
        //            tableStringBuilder.Append("'");

        //            if (i < tables.Count - 1)
        //                tableStringBuilder.Append(", ");
        //        }
        //string tablesString = "(" + tableStringBuilder.ToString() + ")";
        //FormattableString sql = FormattableStringFactory.Create($"SELECT COLUMN_NAME " +
        //                                                        $"FROM information_schema.COLUMNS " +
        //                                                        $"WHERE TABLE_NAME IN " +
        //                                                        tablesString +
        //                                                        ";");
        //        /*var tables = _context.Tables.FromSqlRaw(sql.Format, sql.GetArguments()).ToList();*/
        //        return new List<string> { "AddressID", "Address", "City", "Region", "Country", "ZIP" };
        //    }
        //    public IList<string> GetTableNames()
        //    {
        //        FormattableString sql = FormattableStringFactory.Create($"SELECT table_name FROM information_schema.tables WHERE TABLE_NAME NOT LIKE 'spt%' AND TABLE_NAME NOT LIKE 'MSreplication%';");
        //        var tablenames = this.Database.SqlQuery<string>(sql). GetEnumerator();  
        //        return tablenames.;
        //    }


        //}


    }

}
