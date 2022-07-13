namespace Company.Web.Infrastructure
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Company.Data;
    using Company.Data.Models;
    using Company.Web.Infrastructure.Contracts;
    using Company.Web.Infrastructure.Hubs;
    using Microsoft.AspNetCore.SignalR;
    using Microsoft.EntityFrameworkCore;

    public class ChatService : IChatService
    {
        private readonly IHubContext<ChatHub> hubContext;
        private readonly IUnitOfWork unitOfWork;

        public ChatService(
                IHubContext<ChatHub> hubContext,
                IUnitOfWork unitOfWork)
        {
            this.hubContext = hubContext;
            this.unitOfWork = unitOfWork;
        }

        public async Task AddUserToGroup(string groupName, string toUsername, string fromName)
        {
            var toUser = this.unitOfWork
                                .Users
                                .All()
                                .FirstOrDefault(x => x.UserName == toUsername);
            var toUserId = toUser.Id;

            var fromUser = this.unitOfWork
                                .Users
                                .All()
                                .FirstOrDefault(x => x.UserName == fromName);
            var fromId = fromUser.Id;
            var fromUserImage = fromUser.ImageUrl;

            var targetGroup = this.unitOfWork
                                    .Groups
                                    .All()
                                    .FirstOrDefault(x => x.Name == groupName);

            if (targetGroup == null)
            {
                targetGroup = new Group
                {
                    Name = groupName,
                };

                var targetToUser = new UserGroup
                {
                    User = toUser,
                    Group = targetGroup,
                };

                var targetFromUser = new UserGroup
                {
                    User = fromUser,
                    Group = targetGroup,
                };

                targetGroup.UserGroups.Add(targetToUser);
                targetGroup.UserGroups.Add(targetFromUser);

                await this.unitOfWork.Groups.AddAsync(targetGroup);
                await this.unitOfWork.CompleteAsync();
            }
            await this.hubContext.Clients.Group(groupName).SendAsync("ReceiveMessage", fromName, fromUserImage, $"{fromName} has joined the chat.");
        }

        public async Task<ICollection<ChatMessage>> ExtractAllMessages(string group)
        {
            var targetGroup = this.unitOfWork
                                .Groups
                                .All()
                                .FirstOrDefault(x => x.Name.ToLower() == group.ToLower());
            if (targetGroup != null)
            {
                var messages = this.unitOfWork
                                    .ChatMessages
                                    .All()
                                    .Where(x => x.GroupId == targetGroup.Id)
                                    .OrderBy(x => x.SendedOn)
                                    .ToList();

                foreach (var message in messages)
                {
                    message.ApplicationUser = await this.unitOfWork.Users
                        .All()
                        .FirstOrDefaultAsync(x => x.Id == message.ApplicationUserId);
                }

                return messages;
            }

            return null;
        }

        public async Task GroupReceiveNewMessage(string senderContetId, string fromUsername, string message, string group)
        {
            var fromUser = this.unitOfWork
                                 .Users
                                 .All()
                                 .FirstOrDefault(x => x.UserName == fromUsername);

            var fromId = fromUser.Id;
            var fromImage = fromUser.ImageUrl;

            var newMessage = new ChatMessage
            {
                ApplicationUser = fromUser,
                Group = this.unitOfWork.Groups.All().FirstOrDefault(x => x.Name.ToLower() == group.ToLower()),
                SendedOn = DateTime.UtcNow,
                Content = message,
            };

            await this.unitOfWork.ChatMessages.AddAsync(newMessage);
            await this.hubContext.Clients.GroupExcept(group, senderContetId).SendAsync("SendMessageToGroup", fromUsername, fromImage, message);
            await this.hubContext.Clients.User(fromId).SendAsync("SenderReceiveMessage", fromUsername, fromImage, message);
            await this.unitOfWork.CompleteAsync();
        }

        public async Task ReceiveNewMessage(string fromUsername, string message, string group)
        {
            var fromUser = this.unitOfWork.Users
                                            .All()
                                            .FirstOrDefault(x => x.UserName == fromUsername);

            var fromId = fromUser.Id;
            var fromImage = fromUser.ImageUrl;

            await this.unitOfWork.ChatMessages.AddAsync(new ChatMessage
            {
                ApplicationUser = fromUser,
                Group = this.unitOfWork.Groups.All().FirstOrDefault(x => x.Name.ToLower() == group.ToLower()),
                SendedOn = DateTime.UtcNow,
                ReceiverUsername = fromUser.UserName,
                RecieverImageUrl = fromUser.ImageUrl,
                Content = message,
            });

            await this.unitOfWork.CompleteAsync();
            await this.hubContext.Clients.User(fromId).SendAsync("SendMessage", fromUsername, fromImage, message);
        }

        public async Task SendMessageToGroup(string senderContextId, string fromUsername, string message, string group)
        {
            var fromUser = await this.unitOfWork.Users.All().FirstOrDefaultAsync(x => x.UserName == fromUsername);
            var fromUserImage = fromUser.ImageUrl;
            var toGroup = await this.unitOfWork.Groups.All().FirstOrDefaultAsync(x => x.Name.ToLower() == group.ToLower());

            var newMessage = new ChatMessage
            {
                ApplicationUser = fromUser,
                Group = toGroup,
                SendedOn = DateTime.UtcNow,
                ReceiverUsername = toGroup.Name,
                RecieverImageUrl = fromUserImage,
                Content = message,
            };

            await this.unitOfWork.ChatMessages.AddAsync(newMessage);
            await this.hubContext.Clients.GroupExcept(group, senderContextId).SendAsync("SendMessageToGroup", fromUsername, fromUserImage, message);
            await this.hubContext.Clients.User(fromUser.Id).SendAsync("ReceiveMessageSender", fromUsername, fromUserImage, message);

            await this.unitOfWork.CompleteAsync();
        }

        public async Task<string> SendMessageToUser(string fromUsername, string toUsername, string message, string group)
        {
            var toUser = this.unitOfWork.Users.All().FirstOrDefault(x => x.UserName == toUsername);
            var toId = toUser.Id;
            var toImage = toUser.ImageUrl;

            var fromUser = this.unitOfWork.Users.All().FirstOrDefault(x => x.UserName == fromUsername);
            var fromId = fromUser.Id;
            var fromImage = fromUser.ImageUrl;

            var newMessage = new ChatMessage
            {
                ApplicationUser = fromUser,
                Group = this.unitOfWork.Groups.All().FirstOrDefault(x => x.Name.ToLower() == group.ToLower()),
                SendedOn = DateTime.UtcNow,
                ReceiverUsername = toUser.UserName,
                RecieverImageUrl = toUser.ImageUrl,
                Content = message,
            };

            await this.unitOfWork.ChatMessages.AddAsync(newMessage);
            await this.unitOfWork.CompleteAsync();
            await this.hubContext.Clients.User(toId).SendAsync("ReceiveMessage", fromUsername, fromImage, message);

            return toId;
        }

        public async Task UserJoinedGroupMessage(string username, string group)
        {
            var fromUser = this.unitOfWork.Users
            .All()
            .FirstOrDefault(x => x.UserName == username);

            var fromImage = fromUser.ImageUrl;
            var joinedGroupMessage = $"{username} has joined the group chat.";

            var dbGroup = await this.unitOfWork.Groups.All().FirstOrDefaultAsync(x => x.Name == group);

            if (dbGroup == null)
            {
                var targetGroup = new Group
                {
                    Name = group,
                };

                await this.unitOfWork.Groups.AddAsync(targetGroup);
                await this.unitOfWork.CompleteAsync();
            }

            var newMessage = new ChatMessage
            {
                ApplicationUser = fromUser,
                Group = this.unitOfWork.Groups.All().FirstOrDefault(x => x.Name.ToLower() == group.ToLower()),
                SendedOn = DateTime.UtcNow,
                Content = joinedGroupMessage,
            };

            await this.unitOfWork.ChatMessages.AddAsync(newMessage);
            await this.hubContext.Clients.Group(group).SendAsync("JoinedGroupMessage", username, fromImage, joinedGroupMessage);
            await this.unitOfWork.CompleteAsync();
        }
    }
}
