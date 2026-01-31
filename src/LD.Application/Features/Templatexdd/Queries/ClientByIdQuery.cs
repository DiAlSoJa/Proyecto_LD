using LD.Application.Common.Interfaces.Auth;
using LD.Application.Common.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LD.Application.Features.Auth.Commands;

public class ByIdQuery : IRequest<bool>
{

}

public class ByIdQueryHandler : IRequestHandler<ByIdQuery, bool>
{

    public ByIdQueryHandler()
    {

    }

    public async Task<bool> Handle(ByIdQuery request, CancellationToken cancellationToken)
    {
        return true;
    }
}
