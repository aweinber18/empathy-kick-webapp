using MessagePack;
using System.ComponentModel.DataAnnotations;

namespace EmpathyKick.Models
{
    public class User
    {
        [System.ComponentModel.DataAnnotations.Key]
        public int UserId { get; set; }

        [Required(ErrorMessage = "User name is required")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }

        [Required(ErrorMessage = "First name is required")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Lase name is required")]
        public string LastName { get; set; }

        public DateTime? ActiveDate { get; set; }
        public DateTime? InactiveDate { get; set; }

        [Required(ErrorMessage = "Phone Number is required")]
        public string Phone { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress]
        public string Email { get; set; }
       
        public int? AddressID { get; set; }

        public string? THemeColor { get; set; }
    }
}
