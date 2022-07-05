namespace Company.Data.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    using Company.Data.Common.Models;
    using Company.Data.Models.Enums;

    public class UserNotification : BaseModel<string>
    {
        public UserNotification()
        {
            this.Id = Guid.NewGuid().ToString();
        }

        [Required]
        public NotificationType NotificationType { get; set; }

        [Required]
        public NotificationStatus Status { get; set; }

        [Required]
        [ForeignKey(nameof(ApplicationUser))]
        public string ApplicationUserId { get; set; }

        public ApplicationUser ApplicationUser { get; set; }

        [Required]
        [MaxLength(20)]
        public string TargetUsername { get; set; }

        [Required]
        public DateTime CreatedOn { get; set; }

        [Required]
        public string Link { get; set; }

        [Required]
        public string Text { get; set; }
    }
}
