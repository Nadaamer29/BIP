using BIP.DataAccess.Response.User;
using BIP.Response;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BIP.DataAccess.Dtos.User
{
    public record LoginUserCommand(string Email, string Password) : IRequest<Response<AuthResponse>>;

}
