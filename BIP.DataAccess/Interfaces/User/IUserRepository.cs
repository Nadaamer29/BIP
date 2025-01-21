using BIP.DataAccess.Dtos.User;
using BIP.DataAccess.Response.User;
using BIP.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BIP.DataAccess.Interfaces.User
{
    public interface IUserRepository
    {
        Task<Response<AuthResponse>> RegisterAsync(RegisterRequestdto request, CancellationToken cancellationToken);

      //  Task<Response<AuthResponse>> LoginAsync(LoginRequest request, CancellationToken cancellationToken);

    }
}

