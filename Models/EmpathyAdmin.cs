using System.ComponentModel.DataAnnotations.Schema;

namespace EmpathyKick.Models
{
    public class EmpathyAdmin
    {
        [ForeignKey("User")]
        public int UserID { get; set; }
        public DateTime AuthorizationDate { get; set; }
        public DateTime DeauthorizationDate { get; set; }

    }
}
