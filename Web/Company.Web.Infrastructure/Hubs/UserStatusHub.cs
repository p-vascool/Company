﻿namespace Company.Web.Infrastructure.Hubs
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Company.Data.Common.Repositories;
    using Company.Data.Models;
    using Microsoft.AspNetCore.SignalR;
    using Microsoft.EntityFrameworkCore;

    public class UserStatusHub : Hub
    {
        private static readonly List<string> OnlineUsers = new List<string>();
        private readonly IRepository<ApplicationUser> usersRepository;

        public UserStatusHub(IRepository<ApplicationUser> usersRepository)
        {
            this.usersRepository = usersRepository;
        }

        public async Task IsUserOnline(string username)
        {
            var user = await this.usersRepository.All().FirstOrDefaultAsync(x => x.UserName.ToLower() == username.ToLower());
            if (OnlineUsers.Contains(user.UserName))
            {
                await this.Clients.All.SendAsync("UserIsOnline", user.UserName);
            }
            else
            {
                await this.Clients.All.SendAsync("UserIsOffline", user.UserName);
            }
        }

        public override async Task OnConnectedAsync()
        {
            var username = this.Context.User.Identity.Name;
            if (username != null)
            {
                OnlineUsers.Add(username);
                await this.Clients.All.SendAsync("UserIsOnline", username);
            }
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            var username = this.Context.User.Identity.Name;
            if (username != null)
            {
                OnlineUsers.Remove(username);
                await this.Clients.All.SendAsync("UserIsOffline", username);
            }
        }
    }
}
