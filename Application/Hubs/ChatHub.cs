using Application.Services.PrivateMessageServices.Interfaces;
using Application.Services.UserService.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace BusinessLayer.Hubs
{
    [Authorize]
    public class ChatHub : Hub
    {
        private readonly IAuthenticatedUserService authenticatedUserService;
        private readonly IPrivateMessageService privateMessageService;
        private static readonly Dictionary<string, string> activeUsers = new();

        public ChatHub(
            IAuthenticatedUserService authenticatedUserService,
            IPrivateMessageService privateMessageService)
        {
            this.authenticatedUserService = authenticatedUserService;
            this.privateMessageService = privateMessageService;
        }

        public async Task SendMessageToAll(string userId, string message)
        {
            await Clients.Others.SendAsync("ReceiveMessage", userId, message);
        }

        public async Task SendMessageToUser(string userId, string message)
        {
            var Storedmessage = await privateMessageService.StorePrivateMessage(userId, message);
            if (activeUsers.ContainsKey(userId))
            {
                var username = authenticatedUserService.GetAuthenticatedUsername();
                await Clients.Client(activeUsers[userId]).SendAsync("ReceiveMessage", Storedmessage, username);
            }
        }

        public async Task AddUser(string userId, string connectionId)
        {
            activeUsers.Add(userId, connectionId);
        }

        public string GetConnectionId()
        {
            return Context.ConnectionId;
        }

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