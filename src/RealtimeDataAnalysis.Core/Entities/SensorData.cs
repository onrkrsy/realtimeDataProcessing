using RealtimeDataAnalysis.Core.Enums;

namespace RealtimeDataAnalysis.Core.Entities; 
public class SensorData
{
    public int Id { get; set; }
    public DateTime Timestamp { get; set; }
    public string Type { get; set; }
    public double Value { get; set; }
}
