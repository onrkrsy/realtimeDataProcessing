# Gerçek Zamanlı Veri Analizi Projesi

Bu proje, sensör verilerinin gerçek zamanlı analizini yapan, WebSocket kullanarak canlı veri akışı sağlayan ve Chart.js ile veri görselleştirmesi sunan bir sistemdir. .NET 8 ile geliştirilmiştir.

## Proje Yapısı
![image](https://github.com/onrkrsy/realtimeDataProcessing/assets/11960564/6252182e-b8d6-475e-a2e9-f8baa39a0ccb)


```
RealTimeDataAnalysis/
│
├── MockSensorDataService/
├── RealTimeDataAnalysis.Api/
├── RealTimeDataAnalysis.Application/
├── RealTimeDataAnalysis.Core/
├── RealTimeDataAnalysis.Infrastructure/
└── RealTimeDataAnalysis.Web/
```
## Özellikler

- Gerçek zamanlı sensör veri üretimi (sıcaklık ve nem)
- WebSocket iletişimi ile canlı veri akışı
- Gelen sensör verilerinin analizi
- Analiz edilen verileri almak için RESTful API
- Chart.js kullanarak gerçek zamanlı veri görselleştirme
- Responsive web arayüzü

## Kullanılan Teknolojiler

- .NET 8
- ASP.NET Core
- WebSockets
- AutoMapper
- Chart.js
- jQuery

## Nasıl Çalışır?

1. `MockSensorDataService` sıcaklık ve nem sensör verileri üretir ve bunları WebSocket sunucusuna gönderir.
2. `RealTimeDataAnalysis.Api` WebSocket bağlantılarını yönetir ve verileri işler.
3. `DataAnalyzerService` gelen verileri analiz eder.
4. Analiz sonuçları `InMemoryAnalysisResultRepository`'de saklanır.
5. Web uygulaması, API'den verileri alır ve Chart.js ile görselleştirir.

## Önemli Kod Parçacıkları

### WebSocket Bağlantı Yönetimi (WebSocketHandler.cs)

```csharp
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
            _dataAnalyzer.ProcessData(sensorData);
        }
    }
}
```
## Veri Analizi (DataAnalyzerService.cs)

 
```csharp
private void PerformAnalysis()
{
    var lastMinuteData = _dataQueue
        .Where(d => d.Timestamp >= DateTime.UtcNow.AddMinutes(-1))
        .ToList();

    if (lastMinuteData.Any())
    {
        var temperatureData = lastMinuteData.Where(d => d.Type == SensorType.Temperature.ToString());
        var humidityData = lastMinuteData.Where(d => d.Type == SensorType.Humidity.ToString());

        var result = new AnalysisResult
        {
            Timestamp = DateTime.UtcNow,
            AverageTemperature = temperatureData.Any() ? temperatureData.Average(d => d.Value) : 0,
            AverageHumidity = humidityData.Any() ? humidityData.Average(d => d.Value) : 0
        };

        _resultRepository.Add(result);
    }
}
```
Veri Görselleştirme (site.js)
function updateChart(chart, canvasId, labels, data, label, color) {
    if (chart) {
        chart.data.labels = labels;
        chart.data.datasets[0].data = data;
        chart.update();
    } else {
        let ctx = document.getElementById(canvasId).getContext('2d');
        chart = new Chart(ctx, {
            type: 'line',
            data: {
                labels: labels,
                datasets: [{
                    label: label,
                    data: data,
                    borderColor: color,
                    tension: 0.1
                }]
            },
            options: {
                responsive: true,
                scales: {
                    y: {
                        beginAtZero: false
                    }
                }
            }
        });
    }
    return chart;
}
 
![image](https://github.com/onrkrsy/realtimeDataProcessing/assets/11960564/6fff1488-d127-4f68-b7ca-2c6e1d11f227)

## Nasıl Çalışır?

1. `MockSensorDataService` sahte sensör verileri üretir ve WebSocket sunucusuna gönderir.
2. `RealTimeDataAnalysis.Api` WebSocket bağlantılarını yönetir ve verileri paralel olarak işler.
3. `DataAnalyzerService` gelen verileri çoklu iş parçacığı kullanarak analiz eder.
4. Analiz sonuçları `InMemoryAnalysisResultRepository`'de thread-safe bir şekilde saklanır.
5. Web uygulaması, API'den verileri alır ve Chart.js ile yüksek performanslı görselleştirme sağlar.

## Başlangıç

1. Repoyu klonlayın
2. .NET 8 SDK'nın yüklü olduğundan emin olun
3. Proje kök dizinine gidin
4. Aşağıdaki komutları çalıştırın:

   ```bash
   dotnet restore
   dotnet build
   ```

5. API projesini başlatın:

   ```bash
   cd RealTimeDataAnalysis.Api
   dotnet run
   ```

6. MockSensorDataService'i başlatın:

   ```bash
   cd ../MockSensorDataService
   dotnet run
   ```
6. WEB projesini başlatın:

   ```bash
   cd ../RealTimeDataAnalysis.Web
   dotnet run
   ```
 
7. Web.UI'a http://localhost:5245/ adresinden ulaşabilirsiniz


 
