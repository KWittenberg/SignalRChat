namespace HubServer;

public class ChatHub : Hub
{
    public override async Task OnConnectedAsync()
    {
        var userId = string.Empty;
        var userName = string.Empty;

        userId = Context.GetHttpContext()?.Request.Query["userId"];
        userName = Context.GetHttpContext()?.Request.Query["userName"];

        Console.WriteLine($"User {userName} with Id:{userId} has connected.");
        Console.WriteLine($"Client connected: {Context.ConnectionId}");

        await Clients.Caller.SendAsync("ReceiveSystemMessage", $"Hi {Context.ConnectionId}, you just connected!");

        // return base.OnConnectedAsync(); //pošto je async to ne treba
    }
}