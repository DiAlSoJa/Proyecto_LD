using AutoMapper;
using LD.Contracts.DamageReports;
using LD.Contracts.Requests;
using LD.Domain.Entities;

namespace LD.Application.Features.DamageReports.Profiles;

public class DamageReportProfile : Profile
{
    public DamageReportProfile()
    {
        CreateMap<DamageReport, DamageReportDto>();

        CreateMap<DamageReportRequest, DamageReport>()
            .ForMember(dest => dest.AvailableInventory, opt => opt.Ignore())
            .ForMember(dest => dest.StandardLabel, opt => opt.Ignore())
            .ForMember(dest => dest.Product, opt => opt.Ignore())
            .ForMember(dest => dest.InventoryLocation, opt => opt.Ignore())
            .ForMember(dest => dest.InventoryClient, opt => opt.Ignore())
            .ForMember(dest => dest.InventoryProject, opt => opt.Ignore())
            .ForMember(dest => dest.InventoryWarehouse, opt => opt.Ignore());

        CreateMap<DamageReport, DamageReportRequest>();
    }
}
