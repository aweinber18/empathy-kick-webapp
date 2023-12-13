using System.ComponentModel.DataAnnotations;

namespace EmpathyKick.Models
{
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
}
