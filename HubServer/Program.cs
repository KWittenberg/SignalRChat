using HubServer;


#region builder
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSignalR();

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
    {
        builder.WithOrigins("https://localhost:7199").AllowAnyHeader().AllowAnyMethod();
    });
});
#endregion



#region app
var app = builder.Build();

app.UseCors();

// app.MapGet("/", () => "Hello World!");

app.MapHub<ChatHub>("/chathub");


app.Run();
#endregion