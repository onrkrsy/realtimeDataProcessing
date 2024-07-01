using RealtimeDataAnalysis.Core.Entities;

namespace RealtimeDataAnalysis.Core.Interfaces;
 
public interface IDataAnalyzer
{
    void ProcessData(SensorData data);
    List<AnalysisResult> GetLastMinuteResults();
}
