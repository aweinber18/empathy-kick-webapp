using MessagePack;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EmpathyKick.Models
{
    public class User
    {
        [System.ComponentModel.DataAnnotations.Key]
        public int UserID { get; set; }

        [Required(ErrorMessage = "Username is required")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }

        [Required(ErrorMessage = "First name is required")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last name is required")]
        public string LastName { get; set; }

        public DateTime? ActiveDate { get; set; }
        public DateTime? InactiveDate { get; set; }

        [Required(ErrorMessage = "Phone Number is required")]
        [Phone]
        public string Phone { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress]
        public string Email { get; set; }
       
        public int? AddressID { get; set; }

        public string? ThemeColor { get; set; }

        [ForeignKey("AddressID")]
        public Addresses? Address { get; set; }
        public virtual Donations Donations { get; set; }
    }
}
