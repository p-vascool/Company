namespace Company.Web.ViewModels.Chats
{
    using System.Collections.Generic;

    using Company.Data.Models;

    public class GroupChatVIewModel
    {
        public string GroupName { get; set; }

        public ICollection<ChatMessage> ChatMessages { get; set; } = new HashSet<ChatMessage>();

        public ICollection<ApplicationUser> Users { get; set; } = new HashSet<ApplicationUser>();
    }
}
