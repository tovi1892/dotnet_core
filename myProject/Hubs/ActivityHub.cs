using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.Authorization;

namespace myProject.Hubs;

[Authorize]
public class ActivityHub : Hub
{

    public async Task BroadcastActivity(string username, string action, string itemName)
    {
        await Clients.All.SendAsync("ReceiveActivity", username, action, itemName);
    }
}
