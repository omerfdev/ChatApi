using Amazon.Runtime.Internal.Transform;
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
        private readonly IMongoDatabase mongoDatabase;
        private readonly IMongoCollection<ConnectionInfo> userConnectionsCollection;
        private static readonly Dictionary<string, string> activeUsers = new();
        public ChatHub(
            IAuthenticatedUserService authenticatedUserService,
            IPrivateMessageService privateMessageService,
            IMongoDatabase mongoDatabase)
        {
            this.authenticatedUserService = authenticatedUserService;
            this.privateMessageService = privateMessageService;
            this.mongoDatabase = mongoDatabase;

            // TODO: MongoDB Implementation
            this.userConnectionsCollection = mongoDatabase.GetCollection<ConnectionInfo>("ConnectionInfo");
        }

        public async Task SendMessageToAll(string userId, string message)
        {
            await Clients.Others.SendAsync("ReceiveMessage", userId, message);
        }


        public async Task SendMessageToUser(string userId, string message)
        {
            if (activeUsers.ContainsKey(userId))
            {
                var username = authenticatedUserService.GetAuthenticatedUsername();
                await Clients.Client(activeUsers[userId]).SendAsync("ReceiveMessage", message, username);
            }
        }


        public async Task AddUser(string userId, string connectionId)
        {
            // TODO: MongoDB Implementation
            var userConnection = new ConnectionInfo { UserId = userId, ConnectionId = connectionId };
            await userConnectionsCollection.InsertOneAsync(userConnection);
        }

        public string GetConnectionId()
        {
            return Context.ConnectionId;
        }

        //public List<string> GetActiveUserIds()
        //{
        //    // TODO: MongoDB Implementation
        //    var activeUserIds = userConnectionsCollection.AsQueryable().Select(uc => uc.UserId).ToList();
        //    return activeUserIds;
        //}
        public List<string> GetActiveUserIds()
        {
            return activeUsers.Keys.ToList();
        }
        public override async Task OnConnectedAsync()
        {
            var connectionId = GetConnectionId();
            var userId = authenticatedUserService.GetAuthenticatedUserId();
            if (!activeUsers.ContainsKey(userId))
            {
                activeUsers.Add(userId, connectionId);
            }
            await Clients.All.SendAsync("ReceiveActiveUsers", GetActiveUserIds());
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            var connectionId = GetConnectionId();

            foreach (var user in activeUsers)
            {
                if (user.Value == connectionId)
                {
                    activeUsers.Remove(user.Key);
                    break;
                }
            }
            await Clients.All.SendAsync("ReceiveActiveUsers", GetActiveUserIds());
            await base.OnDisconnectedAsync(exception);
        }
    }



}
