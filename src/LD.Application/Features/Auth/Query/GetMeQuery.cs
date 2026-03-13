using AutoMapper;
using LD.Application.Common.Interfaces;
using LD.Application.Common.Interfaces.Auth;
using LD.Application.Common.Results;
using LD.Contracts.Responses;
using LD.Contracts.User;
using LD.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace LD.Application.Features.Clients.Queries;

public class GetMeQuery : IRequest<Result<GetMeReponse?>>
{

}

public class GetMeQueryHandler : IRequestHandler<GetMeQuery, Result<GetMeReponse?>>
{

    private readonly IApplicationUserManager _userManager;
    private readonly IUserContextService _currentUser;
    private readonly IMapper _mapper;

    public GetMeQueryHandler(
        IApplicationUserManager userManager,
        IUserContextService currentUser,
        IMapper mapper)
    {
        _userManager = userManager; 
        _currentUser = currentUser;
        _mapper = mapper;
    }

    public async Task<Result<GetMeReponse?>> Handle(GetMeQuery request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(_currentUser.UserId))
            return Result<GetMeReponse?>.Failure("UnAuthorized", new());

        var user = await _userManager.GetMe(_currentUser.UserId);

        if (user == null)
            return Result<GetMeReponse?>.Failure("No se pudo encontrar el usuario",new());


        return Result<GetMeReponse?>.Success(user, "Usuario encontrado exitosamente");
    }
}
