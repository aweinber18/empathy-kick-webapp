using System.ComponentModel.DataAnnotations;

namespace EmpathyKick.Models
{
    public class Card
    {
        [Key]
        public int CardID { get; set; }
        public string? CardNum { get; set; }
        public DateTime? CardExp {  get; set; }
        public string? CardSecurity { get; set; }
        public int? AddressID { get; set; }

        public virtual Donations Donations { get; set; }
    }
}