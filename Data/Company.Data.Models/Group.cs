namespace Company.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using Company.Data.Common.Models;

    public class Group : BaseModel<int>, IDeletableEntity
    {
        public Group()
        {
            this.UserGroups = new HashSet<UserGroup>();
            this.ChatMessages = new HashSet<ChatMessage>();
        }

        [MaxLength(100)]
        public string Name { get; set; }

        public ICollection<UserGroup> UserGroups { get; set; }

        public ICollection<ChatMessage> ChatMessages { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime? DeletedOn { get; set; }
    }
}
