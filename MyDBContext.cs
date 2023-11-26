using Microsoft.EntityFrameworkCore;

namespace EmpathyKick
{
    public class MyDBContext : DbContext
    {
        public MyDBContext(DbContextOptions<MyDBContext> options) : base(options)
        {
        }

        public DbSet<Organization> Organizations { get; set; }
        public DbSet<Addresses> Address {get; set; }
        public DbSet<Donations> Donations { get; set; }
        
        
        
    }

    public class Donations
    {
        public int ReceiptNumber { get; set; }
        public string Timestamp { get; set; }
        public int OrganizationId { get; set; }
        public string DonorFullName { get; set; }
        public int Amount { get; set; }
        public string PaymentMethod { get; set; }
        public int UserId { get; set; }
        public int CardId { get; set; }
        public string Frequency { get; set; }

    }
    public class Organization
    {
        public int OrganizationId { get; set; }
        public string OrganizationName { get; set; }
        public int AddressId { get; set; }
        public int TaxId { get; set; }
        public string LogoFile { get; set; }
        public string ThemeColor { get; set; }
    }

    public class Addresses
    {
        public int AddressId { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string Region { get; set; }
        public string Country { get; set; }
    }
}
