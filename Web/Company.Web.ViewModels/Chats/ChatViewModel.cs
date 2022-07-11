namespace Company.Web.ViewModels.Chats
{
    using System.Collections.Generic;

    using Company.Data.Models;

    public class ChatViewModel
    {
        public ApplicationUser FromUser { get; set; }

        public ApplicationUser ToUser { get; set; }

        public ICollection<ChatMessage> ChatMessages { get; set; } = new HashSet<ChatMessage>();

        public string GroupName { get; set; }
    }
}