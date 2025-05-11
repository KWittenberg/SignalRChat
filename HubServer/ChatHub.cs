namespace HubServer;

public class ChatHub : Hub<IChatClient>
{
    static readonly object LockUsers = new object();

    static readonly List<ConnectedUser> ConnectedUsers = new List<ConnectedUser>();



    public override async Task OnConnectedAsync()
    {
        var userId = string.Empty;
        var userName = string.Empty;

        userId = Context.GetHttpContext()?.Request.Query["userId"];
        userName = Context.GetHttpContext()?.Request.Query["userName"];

        Console.WriteLine($"User {userName} with Id:{userId} has connected.");
        Console.WriteLine($"Client connected: {Context.ConnectionId}");

        lock (LockUsers)
        {
            ConnectedUsers.Add(new ConnectedUser
            {
                UserId = userId,
                UserName = userName,
                ConnectionId = Context.ConnectionId
            });
        }

        await Clients.Caller.ReceiveSystemMessage($"Hi {userName}, you just connected!");
        await Clients.All.UpdateUserList(ConnectedUsers);

        // return base.OnConnectedAsync(); //pošto je async to ne treba
    }


    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        ConnectedUser? user;

        lock (LockUsers)
        {
            user = ConnectedUsers.FirstOrDefault(u => u.ConnectionId == Context.ConnectionId);
            if (user != null) ConnectedUsers.Remove(user);
        }

        if (user != null) await Clients.All.UpdateUserList(ConnectedUsers);

        await base.OnDisconnectedAsync(exception);
    }

    public async Task ForwardMessage(string fromUserId, string toConnectionId, string message)
    {
        if (!string.IsNullOrWhiteSpace(toConnectionId)) await Clients.Client(toConnectionId).ReceiveMessage(fromUserId, Context.ConnectionId, message);
    }
}