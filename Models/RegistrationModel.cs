namespace EmpathyKick.Models
{
    public class RegistrationModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
        public string FullAddress { get; set; }
        public string StreetAddress { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string PostalCode { get; set; }
        public string Password { get; set; }
        public string ThemeColor { get; set; }
        public bool IsEmpathyAdmin { get; set; }
        public bool IsOrganizationAdmin { get; set; }
    }
}
