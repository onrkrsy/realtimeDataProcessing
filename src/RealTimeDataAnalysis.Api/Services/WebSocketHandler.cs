using RealtimeDataAnalysis.Core.Entities;
using RealtimeDataAnalysis.Core.Interfaces;
using System.Net.WebSockets;
using System.Text.Json;
using System.Text;

namespace RealTimeDataAnalysis.Api.Services
{
    public class WebSocketHandler : IWebSocketHandler
    {
        private readonly IDataAnalyzer _dataAnalyzer;

        public WebSocketHandler(IDataAnalyzer dataAnalyzer)
        {
            _dataAnalyzer = dataAnalyzer;
        }

        public async Task HandleConnectionAsync(WebSocket webSocket)
        {
            var buffer = new byte[1024 * 4];

            while (webSocket.State == WebSocketState.Open)
            {
                var result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);

                if (result.MessageType == WebSocketMessageType.Text)
                {
                    var json = Encoding.UTF8.GetString(buffer, 0, result.Count);
                    var sensorData = JsonSerializer.Deserialize<SensorData>(json);
                    Console.WriteLine($"Recieved: {json}");

                    _dataAnalyzer.ProcessData(sensorData);
                }
                else if (result.MessageType == WebSocketMessageType.Close)
                {
                    await webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, string.Empty, CancellationToken.None);
                }
            }
        }
    }
}
