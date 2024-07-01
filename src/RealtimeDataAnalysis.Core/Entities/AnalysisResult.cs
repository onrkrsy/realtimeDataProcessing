namespace RealtimeDataAnalysis.Core.Entities; 

public class AnalysisResult
{
    public DateTime Timestamp { get; set; }
    public double AverageTemperature { get; set; }
    public double AverageHumidity { get; set; }
}
