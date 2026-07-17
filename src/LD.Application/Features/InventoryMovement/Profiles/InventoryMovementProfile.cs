using AutoMapper;
using LD.Contracts.InventoryMovement;
using LD.Contracts.Requests;
using LD.Domain.Entities;

namespace LD.Application.Features.InventoryMovement.Profiles
{
    public class InventoryMovementProfile : Profile
    {
        public InventoryMovementProfile()
        {
            CreateMap<Domain.Entities.InventoryMovement, InventoryMovementDto>()
                .ForMember(dest => dest.Cliente,
                    opt => opt.MapFrom(src => src.Client != null ? src.Client.CommercialName : string.Empty))
                .ForMember(dest => dest.Proyecto,
                    opt => opt.MapFrom(src => src.Project != null ? src.Project.ProjectName : string.Empty))
                .ForMember(dest => dest.Almacen,
                    opt => opt.MapFrom(src => src.Location != null && src.Location.Warehouse != null
                        ? src.Location.Warehouse.WarehouseName
                        : string.Empty))
                .ForMember(dest => dest.Ubicacion,
                    opt => opt.MapFrom(src => src.Location != null ? src.Location.LocationName : string.Empty))
                .ForMember(dest => dest.StandardIdStr,
                    opt => opt.MapFrom(src => src.StandardLabel != null ? src.StandardLabel.StandarIdStr ?? string.Empty : string.Empty));

            CreateMap<InventoryMovementRequest, Domain.Entities.InventoryMovement>()
                .ForMember(dest => dest.ProductId,
                    opt => opt.MapFrom(src => src.ProductId))
                .ForMember(dest => dest.ClientId,
                    opt => opt.MapFrom(src => src.ClientId))
                .ForMember(dest => dest.ProjectId,
                    opt => opt.MapFrom(src => src.ProjectId))
                .ForMember(dest => dest.PartNumber,
                    opt => opt.MapFrom(src => src.PartNumber))
                .ForMember(dest => dest.Description,
                    opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.Fecha,
                    opt => opt.MapFrom(src => src.Fecha))
                .ForMember(dest => dest.Hora,
                    opt => opt.MapFrom(src => src.Hora))
                .ForMember(dest => dest.UserId,
                    opt => opt.MapFrom(src => src.UserId))
                .ForMember(dest => dest.LotNumber,
                    opt => opt.MapFrom(src => src.LotNumber))
                .ForMember(dest => dest.PalletNumber,
                    opt => opt.MapFrom(src => src.PalletNumber))
                .ForMember(dest => dest.Reference,
                    opt => opt.MapFrom(src => src.Reference))
                .ForMember(dest => dest.PurchaseOrder,
                    opt => opt.MapFrom(src => src.PurchaseOrder))
                .ForMember(dest => dest.CustomsDeclarationNumber,
                    opt => opt.MapFrom(src => src.CustomsDeclarationNumber))
                .ForMember(dest => dest.ExpirationDate,
                    opt => opt.MapFrom(src => src.ExpirationDate))
                .ForMember(dest => dest.DocumentType,
                    opt => opt.MapFrom(src => (LD.Domain.Enums.DocumentType_e)(int)src.DocumentType))
                .ForMember(dest => dest.MovementType,
                    opt => opt.MapFrom(src => (LD.Domain.Enums.MovementType_e)(int)src.MovementType))
                .ForMember(dest => dest.DocumentId,
                    opt => opt.MapFrom(src => src.DocumentId))
                .ForMember(dest => dest.StatusId,
                    opt => opt.MapFrom(src => src.StatusId))
                .ForMember(dest => dest.LocationId,
                    opt => opt.MapFrom(src => src.LocationId))
                .ForMember(dest => dest.Qty,
                    opt => opt.MapFrom(src => src.Qty))
                .ForMember(dest => dest.StandardId,
                    opt => opt.MapFrom(src => src.StandardId));

            CreateMap<Domain.Entities.InventoryMovement, InventoryMovementRequest>()
                .ForMember(dest => dest.PalletNumber,
                    opt => opt.MapFrom(src => src.PalletNumber))
                .ForMember(dest => dest.DocumentType,
                    opt => opt.MapFrom(src => (LD.Contracts.Enums.DocumentType_e)(int)src.DocumentType))
                .ForMember(dest => dest.MovementType,
                    opt => opt.MapFrom(src => (LD.Contracts.Enums.MovementType_e)(int)src.MovementType));
        }
    }
}
