using RealtimeDataAnalysis.Core.Entities;
using RealTimeDataAnalysis.Application.Interfaces;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealTimeDataAnalysis.Infrastructer.Repositiories
{
    public class InMemoryAnalysisResultRepository : IAnalysisResultRepository
    {
        private readonly ConcurrentQueue<AnalysisResult> _results = new ConcurrentQueue<AnalysisResult>();

        public void Add(AnalysisResult result)
        {
            _results.Enqueue(result);

            while (_results.TryPeek(out var oldestResult) &&
                   oldestResult.Timestamp < DateTime.UtcNow.AddMinutes(-1))
            {
                _results.TryDequeue(out _);
            }
        }

        public List<AnalysisResult> GetLastMinuteResults()
        {
            return _results.ToList();
        }
    }
}
