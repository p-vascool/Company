namespace Company.Web.Infrastructure.Contracts
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Company.Data.Models;

    public interface IChatService
    {
        Task AddUserToGroup(string groupName, string toUsername, string fromName);

        Task<string> SendMessageToUser(string fromUsername, string toUsername, string message, string group);

        Task SendMessageToGroup(string senderContextId, string fromUsername, string message, string group);

        Task ReceiveNewMessage(string fromUsername, string message, string group);

        Task UserJoinedGroupMessage(string username, string group);

        Task GroupReceiveNewMessage(string senderContetId, string fromUsername, string message, string group);

        Task<ICollection<ChatMessage>> ExtractAllMessages(string group);
    }
}
