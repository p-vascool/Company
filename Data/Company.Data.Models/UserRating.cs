using System.ComponentModel.DataAnnotations;

namespace Company.Data.Models
{
    public class UserRating
    {
        [Required]
        public string Username { get; set; }

        [Required]
        public string RaterUsername { get; set; }

        [MaxLength(5)]
        public int Stars { get; set; }
    }
}
