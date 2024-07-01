using RealtimeDataAnalysis.Core.Entities;

namespace RealTimeDataAnalysis.Application.Interfaces
{ 
    public interface IAnalysisResultRepository
    {
        void Add(AnalysisResult result);
        List<AnalysisResult> GetLastMinuteResults();
    }
}
