using RealtimeDataAnalysis.Core.Entities;

namespace RealtimeDataAnalysis.Core.Interfaces; 

public interface ISensorDataProvider
{
    Task<SensorData> GetNextDataAsync();
}
