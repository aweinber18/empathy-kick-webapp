using System.ComponentModel.DataAnnotations;
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
    }
}
