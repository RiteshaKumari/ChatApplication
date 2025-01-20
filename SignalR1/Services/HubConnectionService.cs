using SignalR1.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

public class HubConnectionService
{
    private readonly MyChatDatabseContext _dbContext;

    public HubConnectionService(MyChatDatabseContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task RemoveConnection(string connectionId)
    {
        var hubConnection = await _dbContext.HubConnections.FirstOrDefaultAsync(con => con.ConnectionId == connectionId);
        if (hubConnection != null)
        {
            _dbContext.HubConnections.Remove(hubConnection);
            await _dbContext.SaveChangesAsync();
        }
    }
}
