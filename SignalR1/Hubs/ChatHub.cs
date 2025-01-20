using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using SignalR1.Models;
using SignalR1.myModel;
using System;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.Extensions.Logging;

namespace SignalR1.Hubs
{
    public class ChatHub : Hub
    {
        private readonly MyChatDatabseContext _dbContext;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly HubConnectionService _hubConnectionService;
        private readonly ILogger<ChatHub> _logger;

        public ChatHub(MyChatDatabseContext dbContext, IHttpContextAccessor httpContextAccessor, HubConnectionService hubConnectionService, ILogger<ChatHub> logger)
        {
            _dbContext = dbContext;
            _httpContextAccessor = httpContextAccessor;
            _hubConnectionService = hubConnectionService;
            _logger = logger;
        }

        public override async Task OnConnectedAsync()
        {
            _logger.LogInformation($"Client connected: {Context.ConnectionId}");
            await Clients.Caller.SendAsync("OnConnected");
            await base.OnConnectedAsync();
        }

        public async Task<object> SaveUserConnection(int userId)
        {
           // var userId = "5";
            _logger.LogInformation($"SaveUserConnection called for userId: {userId}");
            try
            {
                if (userId <= 0)
                {
                    throw new ArgumentException("UserId cannot be null or empty");
                }

                var connectionId = Context.ConnectionId;
                _logger.LogInformation($"Attempting to save connection for user {userId} with connection ID {connectionId}");

                var existingConnection = await _dbContext.HubConnections
                    .FirstOrDefaultAsync(h => h.UserId == userId);

                if (existingConnection != null)
                {
                    existingConnection.ConnectionId = connectionId;
                    _dbContext.HubConnections.Update(existingConnection);
                    _logger.LogInformation($"Updating existing connection for user {userId}");
                }
                else
                {
                    var randomId = GenerateRandomAlphanumeric(7);
                    int randnumId = GenerateRandomNumeric(4);

                    var hubConnection = new HubConnection
                    {
                        ConnectionId = connectionId,
                        UserId = userId,
                        ChatId = randnumId,
                        Id = randomId
                    };

                    _dbContext.HubConnections.Add(hubConnection);
                    _logger.LogInformation($"Adding new connection for user {userId}");
                }

                await _dbContext.SaveChangesAsync();
                _logger.LogInformation($"Connection saved/updated successfully for user {userId}!");

                return new
                {
                    success = true,
                    message = "Connection saved successfully.",
                    userId = userId
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error in SaveUserConnection for userId: {userId}");
                return new
                {
                    success = false,
                    message = $"Error saving connection: {ex.Message}"
                };
            }
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            _logger.LogInformation($"Client disconnected: {Context.ConnectionId}");
            await _hubConnectionService.RemoveConnection(Context.ConnectionId);
            await base.OnDisconnectedAsync(exception);
        }

        private static string GenerateRandomAlphanumeric(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var random = new Random();
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        private static int GenerateRandomNumeric(int length)
        {
            if (length <= 0)
                throw new ArgumentException("Length must be greater than 0.");

            var random = new Random();
            var numericString = new string(Enumerable.Repeat("0123456789", length)
                .Select(s => s[random.Next(s.Length)]).ToArray());

            return int.Parse(numericString);
        }
    }
}

