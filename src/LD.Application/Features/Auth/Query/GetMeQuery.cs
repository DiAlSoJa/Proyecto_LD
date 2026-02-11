using AutoMapper;
using LD.Application.Common.Interfaces;
using LD.Application.Common.Interfaces.Auth;
using LD.Contracts.User;
using LD.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace LD.Application.Features.Clients.Queries;

public class GetMeQuery : IRequest<UserDto?>
{

}

public class GetMeQueryHandler : IRequestHandler<GetMeQuery, UserDto?>
{

    //private readonly UserManager<ApplicationUser> _userManager;
    private readonly IUserContextService _currentUser;
    private readonly IMapper _mapper;

    public GetMeQueryHandler(
        IUserContextService currentUser,
        IMapper mapper)
    {
        _currentUser = currentUser;
        _mapper = mapper;
    }

    public async Task<UserDto?> Handle(GetMeQuery request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(_currentUser.UserId))
            return null;

        //var user = await _userManager.FindByIdAsync(_currentUser.UserId);

        //if (user == null)
        //    return null;

        //return _mapper.Map<UserDto>(user);
        return new UserDto
        {
            //Id = user.Id,
            //UserName = user.UserName,
            //Email = user.Email
        };
    }
}
