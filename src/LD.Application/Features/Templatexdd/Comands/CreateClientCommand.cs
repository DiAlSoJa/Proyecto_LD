using LD.Application.Common.Interfaces.Auth;
using LD.Application.Common.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LD.Application.Features.Auth.Commands;

public class CreateCommand : IRequest<bool>
{

}
public class CreateCommandHandler : IRequestHandler<CreateCommand, bool>
{

    public CreateCommandHandler()
    {

    }

    public async Task<bool> Handle(CreateCommand request, CancellationToken cancellationToken)
    {

        return true;
    }
}
