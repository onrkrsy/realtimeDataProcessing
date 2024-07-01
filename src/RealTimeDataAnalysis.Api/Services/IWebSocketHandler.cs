using System.Net.WebSockets;

namespace RealTimeDataAnalysis.Api.Services;
public interface IWebSocketHandler
{
    Task HandleConnectionAsync(WebSocket webSocket);
}
