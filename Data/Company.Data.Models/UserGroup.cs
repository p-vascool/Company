using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Company.Data.Models
{
    public class UserGroup
    {
        [Required]
        [ForeignKey(nameof(ApplicationUser))]
        public int UserId { get; set; }

        public ApplicationUser User { get; set; }

        [Required]
        [ForeignKey(nameof(Group))]
        public int GroupId { get; set; }

        public Group Group { get; set; }
    }
}
