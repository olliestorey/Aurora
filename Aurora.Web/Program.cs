using Aurora.Web.Data;
using Aurora.Web.Services;
using Aurora.Web.SignalR.Hubs;
using Microsoft.AspNetCore.Http.Connections;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddMemoryCache();
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});

builder.Services.AddSignalR()
    .AddJsonProtocol(options =>
    {
        options.PayloadSerializerOptions.PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.CamelCase;
    })
    .AddHubOptions<GameHub>(options =>
    {
        options.EnableDetailedErrors = true;
    })
    .AddHubOptions<PlayerHub>(options =>
    {
        options.EnableDetailedErrors = true;
    }); ;


builder.Services.AddDbContext<DatabaseContext>(options =>
    options.UseSqlite("Data Source=app.db"));

builder.Services.AddTransient<IGlobalLeaderboardService, GlobalLeaderboardService>();
builder.Services.AddTransient<IRoomService, RoomService>();
builder.Services.AddTransient<IEventDispatcherService, EventDispatcherService>();

var app = builder.Build();

app.UseCors("AllowAll");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapHub<GameHub>("/ws/game", options =>
{
    options.Transports =
        HttpTransportType.WebSockets |
        HttpTransportType.LongPolling;
    options.LongPolling.PollTimeout = TimeSpan.FromSeconds(45);
    options.AllowStatefulReconnects = true;
});

app.MapHub<PlayerHub>("/ws/player", options =>
{
    options.Transports =
        HttpTransportType.WebSockets |
        HttpTransportType.LongPolling;
    options.LongPolling.PollTimeout = TimeSpan.FromSeconds(15);
    options.AllowStatefulReconnects = true;
});

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
