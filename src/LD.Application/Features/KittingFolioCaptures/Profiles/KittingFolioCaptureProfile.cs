using AutoMapper;
using LD.Contracts.DTOs.KittingFolioCapture;
using LD.Domain.Entities;

namespace LD.Application.Features.KittingFolioCaptures.Profiles;

public class KittingFolioCaptureProfile : Profile
{
    public KittingFolioCaptureProfile()
    {
        CreateMap<KittingFolioCapture, KittingFolioCaptureDto>()
            .ForMember(dest => dest.KittingFolioCaptureId, opt => opt.MapFrom(src => src.KittingFolioCaptureId))
            .ForMember(dest => dest.KittingId, opt => opt.MapFrom(src => src.KittingId))
            .ForMember(dest => dest.KittingDetailId, opt => opt.MapFrom(src => src.KittingDetailId))
            .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt))
            .ForMember(dest => dest.Client, opt => opt.MapFrom(src => src.Kitting != null && src.Kitting.Client != null ? src.Kitting.Client.CommercialName ?? string.Empty : string.Empty))
            .ForMember(dest => dest.Project, opt => opt.MapFrom(src => src.Kitting != null && src.Kitting.Project != null ? src.Kitting.Project.ProjectName ?? string.Empty : string.Empty))
            .ForMember(dest => dest.KittingCode, opt => opt.MapFrom(src => src.Kitting != null ? src.Kitting.KittingCode ?? string.Empty : string.Empty))
            .ForMember(dest => dest.KittingStatus, opt => opt.MapFrom(src => src.Kitting != null ? src.Kitting.Status ?? string.Empty : string.Empty))
            .ForMember(dest => dest.GuideNumber, opt => opt.MapFrom(src => src.GuideNumber ?? string.Empty))
            .ForMember(dest => dest.InvoiceNumber, opt => opt.MapFrom(src => src.InvoiceNumber ?? string.Empty))
            .ForMember(dest => dest.PartNumber, opt => opt.MapFrom(src => src.PartNumber))
            .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description ?? string.Empty))
            .ForMember(dest => dest.Quantity, opt => opt.MapFrom(src => src.Quantity))
            .ForMember(dest => dest.LotNumber, opt => opt.MapFrom(src => src.LotNumber ?? string.Empty))
            .ForMember(dest => dest.SourceFileName, opt => opt.MapFrom(src => src.SourceFileName ?? string.Empty))
            .ForMember(dest => dest.SourceLineNumber, opt => opt.MapFrom(src => src.SourceLineNumber));
    }
}
