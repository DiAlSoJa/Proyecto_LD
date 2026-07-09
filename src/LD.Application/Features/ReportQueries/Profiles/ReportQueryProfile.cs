using AutoMapper;
using LD.Contracts.DTOs.ReportQueries;
using LD.Contracts.Requests;
using LD.Domain.Entities;

namespace LD.Application.Features.ReportQueries.Profiles;

public class ReportQueryProfile : Profile
{
    public ReportQueryProfile()
    {
        CreateMap<ReportQuery, ReportQueryDto>()
            .ForMember(dest => dest.Nombre, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.Query, opt => opt.MapFrom(src => src.SqlQuery));

        CreateMap<ReportQuery, ReportQuerySummaryDto>()
            .ForMember(dest => dest.Nombre, opt => opt.MapFrom(src => src.Name));

        CreateMap<ReportQueryRequest, ReportQuery>()
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Nombre == null ? null : src.Nombre.Trim()))
            .ForMember(dest => dest.SqlQuery, opt => opt.MapFrom(src => src.Query == null ? null : src.Query.Trim()));
    }
}
