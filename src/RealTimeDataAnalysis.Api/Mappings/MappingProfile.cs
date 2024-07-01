using AutoMapper;
using RealtimeDataAnalysis.Core.Entities;
using RealTimeDataAnalysis.Api.Dtos; 

namespace RealTimeDataAnalysis.Api.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<AnalysisResult, AnalysisResultDto>().ReverseMap();
    }
}
