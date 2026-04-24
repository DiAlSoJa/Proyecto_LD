using AutoMapper;
using LD.Contracts.EquipmentSupplier;
using LD.Contracts.Requests;

namespace LD.Application.Features.EquipmentSupplier.Profiles;

public class EquipmentSupplierProfile : Profile
{
    public EquipmentSupplierProfile()
    {
        CreateMap<LD.Domain.Entities.EquipmentSupplier, EquipmentSupplierDto>();

        CreateMap<EquipmentSupplierRequest, LD.Domain.Entities.EquipmentSupplier>()
            .ForMember(dest => dest.EquipmentSupplierId, opt => opt.MapFrom(src => src.EquipmentSupplierId))
            .ForMember(dest => dest.EquipmentSupplierName, opt => opt.MapFrom(src => src.EquipmentSupplierName));

        CreateMap<LD.Domain.Entities.EquipmentSupplier, EquipmentSupplierRequest>();
    }
}
