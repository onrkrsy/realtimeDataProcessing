using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MockSensorDataService; 
public class DataGeneratorService
{
    private const string WebSocketUri = "ws://localhost:5001/ws";

    public async Task StartAsync()
    {
        using var client = new ClientWebSocket();
        await client.ConnectAsync(new Uri(WebSocketUri), CancellationToken.None);
        Console.WriteLine("Connected to WebSocket. Generating data...");

        var random = new Random();
        int id = 0;
        while (client.State == WebSocketState.Open)
        {

            var sensorType = random.Next(2) == 0 ? "Temperature" : "Humidity";
            var value = sensorType == "Temperature"
                ? random.NextDouble() * 50 // 0-50°C
                : random.NextDouble() * 100; // 0-100%

            var sensorData = new
            {
                Id = ++id,
                Type = sensorType,
                Value = value,
                Timestamp = DateTime.UtcNow
            };
             
            var json = JsonSerializer.Serialize(sensorData);
            var bytes = Encoding.UTF8.GetBytes(json);

            await client.SendAsync(new ArraySegment<byte>(bytes), WebSocketMessageType.Text, true, CancellationToken.None);
            Console.WriteLine($"Sent: {json}");

            await Task.Delay(1000); // 1 second delay
        }
    }
}
