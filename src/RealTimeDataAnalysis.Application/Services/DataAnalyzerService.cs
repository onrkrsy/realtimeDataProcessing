using RealtimeDataAnalysis.Core.Entities;
using RealtimeDataAnalysis.Core.Enums;
using RealtimeDataAnalysis.Core.Interfaces;
using RealTimeDataAnalysis.Application.Interfaces;
using System.Collections.Concurrent;

namespace RealTimeDataAnalysis.Application.Services
{ 
    public class DataAnalyzerService : IDataAnalyzer
    {
        private readonly ConcurrentQueue<SensorData> _dataQueue = new ConcurrentQueue<SensorData>();
        private readonly IAnalysisResultRepository _resultRepository;

        public DataAnalyzerService(IAnalysisResultRepository resultRepository)
        {
            _resultRepository = resultRepository;
        }

        public void ProcessData(SensorData data)
        {
            _dataQueue.Enqueue(data);
            PerformAnalysis();
        }

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

        public List<AnalysisResult> GetLastMinuteResults()
        {
            return _resultRepository.GetLastMinuteResults();
        }
    }
}
