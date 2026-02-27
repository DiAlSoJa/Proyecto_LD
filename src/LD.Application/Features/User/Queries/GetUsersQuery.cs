using AutoMapper;
using LD.Application.Common.Interfaces;
using LD.Application.Common.Interfaces.Auth;
using LD.Application.Common.Results;
using LD.Contracts.User;
using LD.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace LD.Application.Features.User.Queries;

public class GetUsersQuery : IRequest<Result<List<UserDto>>>
{

}

public class GetUsersHandler : IRequestHandler<GetUsersQuery, Result<List<UserDto>>>
{

    private readonly IApplicationUserManager _userManager;
    private readonly IUserContextService _currentUser;
    private readonly IMapper _mapper;

    public GetUsersHandler(
        IApplicationUserManager userManager,
        IUserContextService currentUser,
        IMapper mapper)
    {
        _userManager = userManager; 
        _currentUser = currentUser;
        _mapper = mapper;
    }

    public async Task<Result<List<UserDto>>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(_currentUser.UserId))
            return Result<List<UserDto>>.Failure("UnAuthorized", new());

        var users = await _userManager.GetUsersAsync();

        if (users == null)
            return Result<List<UserDto>>.Failure("No se pudo encontrar el usuario",new());


        return Result<List<UserDto>>.Success(users, "Usuario encontrado exitosamente");
    }
}
