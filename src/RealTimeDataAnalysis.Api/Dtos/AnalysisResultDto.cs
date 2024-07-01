namespace RealTimeDataAnalysis.Api.Dtos
{
    public class AnalysisResultDto
    {
        public DateTime Timestamp { get; set; }
        public double AverageTemperature { get; set; }
        public double AverageHumidity { get; set; }
    }
}
