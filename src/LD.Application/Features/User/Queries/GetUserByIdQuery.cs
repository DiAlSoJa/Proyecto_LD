using AutoMapper;
using LD.Application.Common.Interfaces;
using LD.Application.Common.Interfaces.Auth;
using LD.Application.Common.Results;
using LD.Contracts.Requests;
using LD.Contracts.User;
using LD.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace LD.Application.Features.User.Queries;

public record GetUserByIdQuery(string UserId) : IRequest<Result<UserRequest?>>;

public class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, Result<UserRequest?>>
{

    private readonly IApplicationUserManager _userManager;
    private readonly IUserContextService _currentUser;
    private readonly IMapper _mapper;

    public GetUserByIdQueryHandler(
        IApplicationUserManager userManager,
        IUserContextService currentUser,
        IMapper mapper)
    {
        _userManager = userManager; 
        _currentUser = currentUser;
        _mapper = mapper;
    }

    public async Task<Result<UserRequest?>> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {

        var user = await _userManager.GetUserByIdAsync(request.UserId);

        if (user == null)
            return Result<UserRequest?>.Failure("No se pudo encontrar el usuario",new List<string> {"No existe el usuario" });


        return Result<UserRequest?>.Success(user, "Usuario encontrado exitosamente");
    }
}
