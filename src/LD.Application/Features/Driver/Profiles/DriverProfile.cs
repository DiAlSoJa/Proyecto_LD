using AutoMapper;
using LD.Contracts.Driver;
using LD.Contracts.Requests;

namespace LD.Application.Features.Driver.Profiles;

public class DriverProfile : Profile
{
    public DriverProfile()
    {
        CreateMap<LD.Domain.Entities.Driver, DriverDto>();
        CreateMap<LD.Domain.Entities.Driver, DriverRequest>();
        CreateMap<DriverRequest, LD.Domain.Entities.Driver>();
    }
}
