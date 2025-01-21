using BIP.DataAccess.Dtos.User;
using BIP.DataAccess.Interfaces.User;
using BIP.DataAccess.Response.User;
using BIP.Response;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace BIP.BuisnessLogic.Handlers
{
    public class UserCommandHandler :
        IRequestHandler<RegisterUserCommand, Response<string>>,
        IRequestHandler<LoginUserCommand, Response<AuthResponse>>
    {
        private readonly IUserRepository _applicationUserServices;

        public UserCommandHandler(IUserRepository applicationUserServices)
        {
            _applicationUserServices = applicationUserServices;
        }

        public async Task<Response<string>> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {
            var registerRequest = new RegisterRequestdto(
                request.Email,
                request.Password,
                request.UserName
            );

            var result = await _applicationUserServices.RegisterAsync(registerRequest, cancellationToken);
            return new Response<string>
            {
                Succeeded = result.Succeeded,
                Message = result.Message
            };
        }

        public async Task<Response<AuthResponse>> Handle(LoginUserCommand request, CancellationToken cancellationToken)
        {
            var loginRequest = new LoginRequest(request.Email, request.Password);

            var result = await _applicationUserServices.LoginAsync(loginRequest, cancellationToken);
            return new Response<AuthResponse>
            {
                Succeeded = result.Succeeded,
                Message = result.Message,
                Data = result.Data
            };
        }
    }
}
