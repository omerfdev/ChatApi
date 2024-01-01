using Application.Services.PrivateMessageServices.Implementations;
using Application.Services.PrivateMessageServices.Interfaces;
using Application.Services.UserService.Interfaces;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Hubs
{
    [Authorize]
    public class ChatHub : Hub
    {
        private readonly IAuthenticatedUserService authenticatedUserService;
        private readonly IPrivateMessageService privateMessageService;
        private readonly IMongoCollection<ConnectionInfo> connectionInfoCollection;

        public ChatHub(
            IAuthenticatedUserService authenticatedUserService,
            IPrivateMessageService privateMessageService,
            IMongoDatabase database)
        {
            this.authenticatedUserService = authenticatedUserService;
            this.privateMessageService = privateMessageService;
            this.connectionInfoCollection = database.GetCollection<ConnectionInfo>("ConnectionInfo");
        }

        public async Task SendMessageToAll(string userId, string message)
        {
            await Clients.Others.SendAsync("ReceiveMessage", userId, message);
        }

        public async Task SendMessageToUser(string userId, string message)
        {
            var storedMessage = await privateMessageService.StorePrivateMessage(userId, message);
            var connectionInfo = await connectionInfoCollection.Find(Builders<ConnectionInfo>.Filter.Eq(c => c.UserId, userId)).FirstOrDefaultAsync();

            if (connectionInfo != null)
            {
                var username = authenticatedUserService.GetAuthenticatedUsername();
                await Clients.Client(connectionInfo.ConnectionId).SendAsync("ReceiveMessage", storedMessage, username);
            }
        }

        public async Task AddUser(string userId)
        {
            var connectionId = Context.ConnectionId;
            await connectionInfoCollection.InsertOneAsync(new ConnectionInfo { UserId = userId, ConnectionId = connectionId });
        }

        public string GetConnectionId()
        {
            return Context.ConnectionId;
        }

        public List<string> GetActiveUserIds()
        {
            var userIds = connectionInfoCollection.Find(_ => true).ToList().ConvertAll(info => info.UserId);
            return userIds;
        }

        public override async Task OnConnectedAsync()
        {
            var userId = authenticatedUserService.GetAuthenticatedUserId();
            await AddUser(userId);
            await Clients.All.SendAsync("ReceiveActiveUsers", GetActiveUserIds());
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            var connectionId = GetConnectionId();
            await connectionInfoCollection.DeleteOneAsync(Builders<ConnectionInfo>.Filter.Eq(c => c.ConnectionId, connectionId));
            await Clients.All.SendAsync("ReceiveActiveUsers", GetActiveUserIds());
            await base.OnDisconnectedAsync(exception);
        }
    }

   
}
