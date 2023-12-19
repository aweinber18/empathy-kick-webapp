using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;
using System.Text;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace EmpathyKick.Models
{
    public class MyDBContext : DbContext
    {
        SqlConnection _connection = new SqlConnection("Server=tcp:empathykick.database.windows.net," +
            "1433;Initial Catalog=EmpathyKickDB;Persist Security Info=False;User ID=empathyadmin;" +
            "Password=Adminpassword123;MultipleActiveResultSets=False;Encrypt=True;" +
            "TrustServerCertificate=False;Connection Timeout=60;");
        public MyDBContext() { }
        public MyDBContext(DbContextOptions<MyDBContext> options) : base(options)
        {
        }
        public DbSet<Organization> Organizations { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Donation> Donations { get; set; }
        public class Donation
        {
            [Key]
            public int ReceiptNumber { get; set; }
            public DateTime Timestamp { get; set; }
            public int OrganizationId { get; set; }
            public string DonorFullName { get; set; }
            public int Amount { get; set; }
            public string? PaymentMethod { get; set; }
            public int? UserId { get; set; }
            public int? CardId { get; set; }
            public string? Frequency { get; set; }

        }
        public class Organization
        {
            public Organization() // Add a default constructor
            {
            }
            public Organization(int orgId, string orgName, int addId, int taxId, string logo, string color)
            {
                OrganizationId = orgId;
                OrganizationName = orgName;
                AddressId = addId;
                TaxId = taxId;
                LogoFile = logo;
                ThemeColor = color;
            }
            [Key]
            public int OrganizationId { get; set; }
            public string OrganizationName { get; set; }
            public int AddressId { get; set; }
            public int? TaxId { get; set; }
            public string LogoFile { get; set; }
            public string? ThemeColor { get; set; }
        }
        public class Address
        {
            [Key]
            public int AddressId { get; set; }
            public string StreetAddress { get; set; }
            public string City { get; set; }
            public string Region { get; set; }
            public string Country { get; set; }

            public List<string> GetSpecifiedData(List<string> specifiedColumns)
            {
                List<string> returnedData = new List<string>();
                for (int i = 0; i < specifiedColumns.Count; i++)
                {
                    switch (specifiedColumns[i])
                    {
                        case "AddressId":
                            returnedData.Add(AddressId.ToString());
                            break;
                        case "Address":
                            returnedData.Add(StreetAddress);
                            break;
                        case "City":
                            returnedData.Add(City);
                            break;
                        case "Region":
                            returnedData.Add(Region);
                            break;
                        case "Country":
                            returnedData.Add(Country);
                            break;
                    }
                }
                return returnedData;
            }
        }

        public class TableColumnPair
        {
            public TableColumnPair(string table, string column)
            {
                TableName = table;
                ColumnName = column;
            }
            public string TableName { get; set; }
            public string ColumnName { get; set; }
        }
        public IList<string> GetTableNames()
        {
            FormattableString sql = FormattableStringFactory.Create($"SELECT table_name FROM information_schema.tables WHERE TABLE_NAME NOT LIKE 'spt%' AND TABLE_NAME NOT LIKE 'MSreplication%' AND TABLE_NAME NOT LIKE 'data%';");

            var tablenames = this.Database.SqlQuery<string>(sql).ToList();
            return tablenames;
        }
        public List<TableColumnPair> GetTableAndColumnNames(List<string> tables)
        {
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
            FormattableString sql = FormattableStringFactory.Create($"SELECT TABLE_NAME, COLUMN_NAME " +
                                                                    $"FROM information_schema.COLUMNS " +
                                                                    $"WHERE TABLE_NAME IN " +
                                                                    tablesString +
                                                                    ";");

            var pairs = new List<TableColumnPair>();
            
            _connection.Open();
            using (SqlCommand command = new SqlCommand(sql.Format, _connection))
            {

                // Execute the query and get a data reader
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    // Check if there are rows in the result set
                    if (reader.HasRows)
                    {
                        // Iterate through the rows
                        while (reader.Read())
                        {
                            // Access columns by name
                            string tableName = reader.GetString(reader.GetOrdinal("TABLE_NAME"));
                            string columnName = reader.GetString(reader.GetOrdinal("COLUMN_NAME"));

                            // Create TableColumnPair object and add it to the list
                            var pair = new TableColumnPair(tableName, columnName);
                            pairs.Add(pair);
                        }
                    }
                }
            }
            return pairs;
        }
        public SqlDataReader GetEAData(string eaDataQuery)
        {
            _connection.Open();
            using (SqlCommand command = new SqlCommand(eaDataQuery, _connection))
            {
                SqlDataReader reader = command.ExecuteReader();
                return reader;
            }
        }
        public string GetEADataQuery(List<TableColumnPair> pairs, List<string> wherePredicates)
        {
            StringBuilder sql = new StringBuilder();

            var tables = new HashSet<string>();
            var columns = new HashSet<string>();
            string joinerColumn = "";
            foreach (TableColumnPair pair in pairs)
            {
                tables.Add(pair.TableName.ToString());
                if (! columns.Add(pair.ColumnName.ToString()))
                    joinerColumn = pair.ColumnName.ToString();
            }

            sql.Append("SELECT ");
            for (int i= 0; i < columns.Count; i++)
            {
                sql.Append(pairs[i].ColumnName);
                if (i < columns.Count - 1)
                    sql.Append(", ");
            }
            sql.Append(" FROM ");
            sql.Append(tables.ToList<string>()[0]);
            if (tables.Count > 1)
            {
                sql.Append(" a JOIN" + tables.ToList<string>()[1] + " b ON a." + joinerColumn + " = " + " b." + joinerColumn);
            }

            if (wherePredicates.Count > 0)
            {
                sql.Append(" WHERE ");
                for (int i=0; i<wherePredicates.Count; i++)
                {
                    sql.Append(wherePredicates[i]);
                    if (i<wherePredicates.Count - 1)
                        sql.Append(" AND ");
                }
            }

            return sql.ToString();
        }
    }
}