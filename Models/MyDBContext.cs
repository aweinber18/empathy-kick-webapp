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

        public DbSet<Organization> Organizations { get; set; }
        public DbSet<Addresses> Addresses { get; set; }
        public DbSet<Donations> Donations { get; set; }

        public IList<string> GetTableNames()
        {
            FormattableString sql = FormattableStringFactory.Create($"SELECT table_name FROM information_schema.tables WHERE TABLE_NAME NOT LIKE 'spt%' AND TABLE_NAME NOT LIKE 'MSreplication%';");

            var tablenames = this.Database.SqlQuery<string>(sql).ToList();
            return tablenames;
        }


    }
    

    public class Donations
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

    public class Addresses
    {
        [Key]
        public int AddressId { get; set; }
        public string Address { get; set; }
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
                        returnedData.Add(Address);
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
}
