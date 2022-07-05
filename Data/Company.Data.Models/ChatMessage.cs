namespace Company.Data.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    using Company.Data.Common.Models;

    public class ChatMessage : BaseModel<int>, IDeletableEntity
    {
        [MaxLength(550)]
        public string Content { get; set; }

        [Required]
        [ForeignKey(nameof(Group))]
        public int GroupId { get; set; }

        public Group Group { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime? DeletedOn { get; set; }
    }
}
