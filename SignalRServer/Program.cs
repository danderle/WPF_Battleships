using Microsoft.AspNetCore.ResponseCompression;
using SignalRServer;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSignalR();
builder.Services.AddResponseCompression(opts =>
{
    opts.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(new[] {"application/octet-stream"});
});

var app = builder.Build();

app.MapHub<BattleshipsHub>("/battleships");

app.Run();
