using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EmpathyKick.Models
{
    public class OrganizationAdmin
    {
        [Key]
        [ForeignKey("User")]
        public int UserID { get; set; }

        [Key]
        [ForeignKey("Organization")]
        public int OrganizationID { get; set; }
        public DateTime? AuthorizationDate { get; set; }
        public DateTime? DeauthorizationDate { get; set; }
    }
}
