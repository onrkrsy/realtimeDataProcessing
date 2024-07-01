using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using RealtimeDataAnalysis.Core.Interfaces;
using RealTimeDataAnalysis.Api.Dtos;

namespace RealTimeDataAnalysis.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AnalysisController : ControllerBase
    {
        private readonly IDataAnalyzer _dataAnalyzer;
        private readonly IMapper _mapper;

        public AnalysisController(IDataAnalyzer dataAnalyzer, IMapper mapper)
        {
            _dataAnalyzer = dataAnalyzer;
            _mapper = mapper;
        }

        [HttpGet("lastminute")]
        public ActionResult<IEnumerable<AnalysisResultDto>> GetLastMinuteResults()
        {
            var results = _dataAnalyzer.GetLastMinuteResults();
            return Ok(_mapper.Map<List<AnalysisResultDto>>(results)); 
        }
    }
}
