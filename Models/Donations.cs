using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EmpathyKick.Models
{
    public class Donations
    {
        [Key]
        public int ReceiptNumber { get; set; }
        public DateTime? Timestamp { get; set; }
        public int? OrganizationId { get; set; }
        public string? DonorFullName { get; set; }
        public decimal? Amount { get; set; }
        public string? PaymentMethod { get; set; }

        public int? UserId { get; set; }
        public int? CardId { get; set; }
        public string? Frequency { get; set; }
        // Navigation properties
        [ForeignKey("OrganizationId")]
        public Organization? Organization { get; set; }

        [ForeignKey("UserId")]
        public User? User { get; set; }

        [ForeignKey("CardId")]
        public Card? Card { get; set; }

    }
}
