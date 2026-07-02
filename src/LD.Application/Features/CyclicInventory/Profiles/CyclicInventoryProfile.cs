using AutoMapper;
using LD.Contracts.InventarioCiclico;
using LD.Contracts.Requests;
using LD.Domain.Entities;
using CyclicInventoryEntity = LD.Domain.Entities.CyclicInventory;
using CyclicInventoryDetailEntity = LD.Domain.Entities.CyclicInventoryDetail;

namespace LD.Application.Features.CyclicInventory.Profiles;

public class CyclicInventoryProfile : Profile
{
    public CyclicInventoryProfile()
    {
        CreateMap<CyclicInventoryEntity, CyclicInventoryDto>()
            .ForMember(dest => dest.InventarioCiclicoId,
                opt => opt.MapFrom(src => src.CyclicInventoryId))
            .ForMember(dest => dest.Fecha,
                opt => opt.MapFrom(src => src.Date))
            .ForMember(dest => dest.Almacen,
                opt => opt.MapFrom(src => src.Warehouse != null ? src.Warehouse.WarehouseName : string.Empty))
            .ForMember(dest => dest.Auditor,
                opt => opt.MapFrom(src => src.AuditorName ?? src.AuditorUserId))
            .ForMember(dest => dest.Estatus,
                opt => opt.MapFrom(src => src.Status))
            .ForMember(dest => dest.FechaTerminado,
                opt => opt.MapFrom(src => src.CompletedAt))
            .ForMember(dest => dest.Detalles,
                opt => opt.MapFrom(src => src.Details));

        CreateMap<CyclicInventoryDetailEntity, CyclicInventoryDetailDto>()
            .ForMember(dest => dest.InventarioCiclicoDetalleId,
                opt => opt.MapFrom(src => src.CyclicInventoryDetailId))
            .ForMember(dest => dest.InventarioCiclicoId,
                opt => opt.MapFrom(src => src.CyclicInventoryId))
            .ForMember(dest => dest.Ubicacion,
                opt => opt.MapFrom(src => src.Location != null ? src.Location.LocationName : string.Empty))
            .ForMember(dest => dest.TakeNumber,
                opt => opt.MapFrom(src => src.TakeNumber <= 0 ? 1 : src.TakeNumber))
            .ForMember(dest => dest.Tomada,
                opt => opt.MapFrom(src => src.Counted))
            .ForMember(dest => dest.Teorico,
                opt => opt.MapFrom(src => src.TheoreticalQty))
            .ForMember(dest => dest.Fisico,
                opt => opt.MapFrom(src => src.PhysicalQty))
            .ForMember(dest => dest.MismaUbicacion,
                opt => opt.MapFrom(src => src.SameLocationQty))
            .ForMember(dest => dest.EnOtraUbicacion,
                opt => opt.MapFrom(src => src.AnotherLocationQty))
            .ForMember(dest => dest.ResultadoPrimeraToma,
                opt => opt.MapFrom(src => src.FirstCountResult))
            .ForMember(dest => dest.ResultadoSegundaToma,
                opt => opt.MapFrom(src => src.SecondCountResult))
            .ForMember(dest => dest.ResultadoTerceraToma,
                opt => opt.MapFrom(src => src.ThirdCountResult))
            .ForMember(dest => dest.ResultadoCuartaToma,
                opt => opt.MapFrom(src => src.FourthCountResult))
            .ForMember(dest => dest.ResultadoFinal,
                opt => opt.MapFrom(src => src.FinalResult))
            .ForMember(dest => dest.Escaneado,
                opt => opt.MapFrom(src => src.Scanned));

        CreateMap<InventarioCiclicoRequest, CyclicInventoryEntity>()
            .ForMember(dest => dest.CyclicInventoryId,
                opt => opt.MapFrom(src => src.InventarioCiclicoId))
            .ForMember(dest => dest.Date,
                opt => opt.MapFrom(src => src.Fecha))
            .ForMember(dest => dest.AuditorName,
                opt => opt.MapFrom(src => src.AuditorNombre))
            .ForMember(dest => dest.Status,
                opt => opt.MapFrom(src => src.Estatus))
            .ForMember(dest => dest.CompletedAt,
                opt => opt.MapFrom(src => src.FechaTerminado))
            .ForMember(dest => dest.Details, opt => opt.Ignore())
            .ForMember(dest => dest.Warehouse, opt => opt.Ignore());
        CreateMap<CyclicInventoryEntity, InventarioCiclicoRequest>()
            .ForMember(dest => dest.InventarioCiclicoId,
                opt => opt.MapFrom(src => src.CyclicInventoryId))
            .ForMember(dest => dest.Fecha,
                opt => opt.MapFrom(src => src.Date))
            .ForMember(dest => dest.AuditorNombre,
                opt => opt.MapFrom(src => src.AuditorName))
            .ForMember(dest => dest.Estatus,
                opt => opt.MapFrom(src => src.Status))
            .ForMember(dest => dest.FechaTerminado,
                opt => opt.MapFrom(src => src.CompletedAt))
            .ForMember(dest => dest.Detalles,
                opt => opt.MapFrom(src => src.Details));
        CreateMap<CyclicInventoryDetailDto, CyclicInventoryDetailEntity>()
            .ForMember(dest => dest.CyclicInventoryDetailId,
                opt => opt.MapFrom(src => src.InventarioCiclicoDetalleId))
            .ForMember(dest => dest.CyclicInventoryId,
                opt => opt.MapFrom(src => src.InventarioCiclicoId))
            .ForMember(dest => dest.TakeNumber,
                opt => opt.MapFrom(src => src.TakeNumber <= 0 ? 1 : src.TakeNumber))
            .ForMember(dest => dest.Counted,
                opt => opt.MapFrom(src => src.Tomada))
            .ForMember(dest => dest.TheoreticalQty,
                opt => opt.MapFrom(src => src.Teorico))
            .ForMember(dest => dest.PhysicalQty,
                opt => opt.MapFrom(src => src.Fisico))
            .ForMember(dest => dest.SameLocationQty,
                opt => opt.MapFrom(src => src.MismaUbicacion))
            .ForMember(dest => dest.AnotherLocationQty,
                opt => opt.MapFrom(src => src.EnOtraUbicacion))
            .ForMember(dest => dest.FirstCountResult,
                opt => opt.MapFrom(src => src.ResultadoPrimeraToma))
            .ForMember(dest => dest.SecondCountResult,
                opt => opt.MapFrom(src => src.ResultadoSegundaToma))
            .ForMember(dest => dest.ThirdCountResult,
                opt => opt.MapFrom(src => src.ResultadoTerceraToma))
            .ForMember(dest => dest.FourthCountResult,
                opt => opt.MapFrom(src => src.ResultadoCuartaToma))
            .ForMember(dest => dest.FinalResult,
                opt => opt.MapFrom(src => src.ResultadoFinal))
            .ForMember(dest => dest.Scanned,
                opt => opt.MapFrom(src => src.Escaneado));
    }
}
