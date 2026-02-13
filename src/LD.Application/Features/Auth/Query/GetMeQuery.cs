using AutoMapper;
using LD.Application.Common.Interfaces;
using LD.Application.Common.Interfaces.Auth;
using LD.Application.Common.Results;
using LD.Contracts.User;
using LD.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace LD.Application.Features.Clients.Queries;

public class GetMeQuery : IRequest<Result<UserDto?>>
{

}

public class GetMeQueryHandler : IRequestHandler<GetMeQuery, Result<UserDto?>>
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

    public async Task<Result<UserDto?>> Handle(GetMeQuery request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(_currentUser.UserId))
            return Result<UserDto?>.Failure("UnAuthorized", new());

        var user = await _userManager.GetUserByIdAsync(_currentUser.UserId);

        if (user == null)
            return Result<UserDto?>.Failure("No se pudo encontrar el usuario",new());


        return Result<UserDto?>.Success(user, "Usuario encontrado exitosamente");
    }
}
