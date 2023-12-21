using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EmpathyKick.Models
{
    [PrimaryKey(nameof(UserID), nameof(OrganizationID))]
    public class OrganizationAdmin
    {
     
        [Column(Order = 0)] // Order is important for composite keys
        [ForeignKey("User")]
        public int UserID { get; set; }

       
        [Column(Order = 1)] // Order is important for composite keys
        [ForeignKey("Organization")]
        public int OrganizationID { get; set; }
        public DateTime? AuthorizationDate { get; set; }
        public DateTime? DeauthorizationDate { get; set; }
    }
}
