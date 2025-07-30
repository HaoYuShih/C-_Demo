using System.Net.WebSockets;
using System.Text;
using PersonnelApi.GraphQL;
using PersonnelApi.Services;

var builder = WebApplication.CreateBuilder(args);

// 1. Add services to the container.

// Add standard API Controllers
builder.Services.AddControllers();

// Add gRPC services
builder.Services.AddGrpc();

// Add GraphQL services
builder.Services.AddGraphQLServer()
    .AddMutationType<PersonnelMutation>();

// Swagger/OpenAPI is still useful for the HTTP API
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// 2. Configure the HTTP request pipeline.

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Enable WebSockets
var webSocketOptions = new WebSocketOptions
{
    KeepAliveInterval = TimeSpan.FromMinutes(2)
};
app.UseWebSockets(webSocketOptions);

// 3. Map endpoints

// Map standard API Controllers (for HTTP API)
app.MapControllers();

// Map gRPC services
app.MapGrpcService<GrpcPersonnelService>();

// Map GraphQL endpoint
app.MapGraphQL();

// Map WebSocket endpoint
app.Map("/ws", async context =>
{
    if (context.WebSockets.IsWebSocketRequest)
    {
        using var ws = await context.WebSockets.AcceptWebSocketAsync();
        var logger = app.Services.GetRequiredService<ILogger<Program>>();
        logger.LogInformation("WebSocket connection established.");

        var buffer = new byte[1024 * 4];
        var receiveResult = await ws.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);

        while (!receiveResult.CloseStatus.HasValue)
        {
            var receivedMessage = Encoding.UTF8.GetString(buffer, 0, receiveResult.Count);
            logger.LogInformation("WebSocket received: {Message}", receivedMessage);

            var responseMessage = $"Echo from server: {receivedMessage}";
            var responseBytes = Encoding.UTF8.GetBytes(responseMessage);

            await ws.SendAsync(
                new ArraySegment<byte>(responseBytes, 0, responseBytes.Length),
                receiveResult.MessageType,
                receiveResult.EndOfMessage,
                CancellationToken.None);

            receiveResult = await ws.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
        }

        await ws.CloseAsync(
            receiveResult.CloseStatus.Value,
            receiveResult.CloseStatusDescription,
            CancellationToken.None);

        logger.LogInformation("WebSocket connection closed.");
    }
    else
    {
        context.Response.StatusCode = StatusCodes.Status400BadRequest;
    }
});


app.Run();
