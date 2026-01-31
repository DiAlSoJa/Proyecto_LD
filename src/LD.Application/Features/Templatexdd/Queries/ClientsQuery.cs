using LD.Application.Common.Interfaces.Auth;
using LD.Application.Common.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LD.Application.Features.Auth.Commands;

public class GetQuery : IRequest<bool>
{

}

public class GetQueryHandler : IRequestHandler<GetQuery, bool>
{

    public GetQueryHandler(IAuthService authService)
    {

    }

    public async Task<bool> Handle(GetQuery request, CancellationToken cancellationToken)
    {
        return true;
    }
}
