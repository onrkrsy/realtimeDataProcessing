using RealTimeDataAnalysis.Api.Mappings;
using RealtimeDataAnalysis.Core.Interfaces; 
using RealTimeDataAnalysis.Application.Services;
using RealTimeDataAnalysis.Application.Interfaces;
using RealTimeDataAnalysis.Infrastructer.Repositiories;
using RealTimeDataAnalysis.Api.Services;
using System.Net;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAutoMapper(typeof(MappingProfile));
builder.Services.AddSingleton<IDataAnalyzer, DataAnalyzerService>();
builder.Services.AddSingleton<IAnalysisResultRepository, InMemoryAnalysisResultRepository>(); 
builder.Services.AddSingleton<IWebSocketHandler, WebSocketHandler>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins",
        builder =>
        {
            builder.AllowAnyOrigin()
                   .AllowAnyMethod()
                   .AllowAnyHeader();
        });
});

var app = builder.Build();
app.UseWebSockets();

app.Map("/ws", async context =>
{
    if (context.WebSockets.IsWebSocketRequest)
    {
        using var webSocket = await context.WebSockets.AcceptWebSocketAsync();
        var handler = app.Services.GetRequiredService<IWebSocketHandler>();
        await handler.HandleConnectionAsync(webSocket);
    }
    else
    {
        context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
    }
});
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
//app.UseRouting();

app.UseCors("AllowAllOrigins");
app.UseAuthorization();

app.MapControllers();

app.Run();
