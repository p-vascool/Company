namespace Company.Data.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    using Company.Data.Common.Models;

    public class ChatMessage : BaseModel<int>
    {
        [MaxLength(550)]
        public string Content { get; set; }

        [Required]
        [ForeignKey(nameof(Group))]
        public int GroupId { get; set; }

        public Group Group { get; set; }
    }
}
