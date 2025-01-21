using BIP.DataAccess.Dtos.User;
using BIP.DataAccess.Entities;
using BIP.DataAccess.Interfaces.User;
using BIP.DataAccess.Response.User;
using BIP.Entities;
using BIP.Response;
using Mapster;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace BIP.BuisnessLogic.Services
{
    public class UserRepository : IUserRepository
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;  
        private readonly JWT _jWT;

        public UserRepository(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IOptions<JWT> jwt)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _jWT = jwt.Value;
        }
       

        public async Task<Response<AuthResponse>> RegisterAsync(RegisterRequestdto request, CancellationToken cancellationToken)
        {
            var emailexists = await _userManager.FindByEmailAsync(request.Email);
            if (emailexists != null)
                return new Response<AuthResponse>
                {
                    StatusCode = (int)System.Net.HttpStatusCode.BadRequest,
                    Succeeded = false,
                    Message = "Email already exists"
                };

            var user = request.Adapt<ApplicationUser>();
            user.UserName = request.Email;
            if (string.IsNullOrEmpty(user.UserName))
            {
                return new Response<AuthResponse>
                {
                    StatusCode = (int)System.Net.HttpStatusCode.BadRequest,
                    Succeeded = false,
                    Message = "UserName is not being set correctly."
                };
            }

            var res = await _userManager.CreateAsync(user, request.Password);
            if (!res.Succeeded)
            {
                var errors = string.Join(", ", res.Errors.Select(e => e.Description));
                return new Response<AuthResponse>
                {
                    StatusCode = (int)System.Net.HttpStatusCode.BadRequest,
                    Succeeded = false,
                    Message = errors
                };
            }

            var roleExist = await _roleManager.RoleExistsAsync("User");
            if (!roleExist)
            {
                var roleResult = await _roleManager.CreateAsync(new IdentityRole("User"));
                if (!roleResult.Succeeded)
                {
                    var roleErrors = string.Join(", ", roleResult.Errors.Select(e => e.Description));
                    return new Response<AuthResponse>
                    {
                        StatusCode = (int)System.Net.HttpStatusCode.BadRequest,
                        Succeeded = false,
                        Message = roleErrors
                    };
                }
            }

            var roleResultAdd = await _userManager.AddToRoleAsync(user, "User");
            if (!roleResultAdd.Succeeded)
            {
                var roleErrors = string.Join(", ", roleResultAdd.Errors.Select(e => e.Description));
                return new Response<AuthResponse>
                {
                    StatusCode = (int)System.Net.HttpStatusCode.BadRequest,
                    Succeeded = false,
                    Message = roleErrors
                };
            }

            return new Response<AuthResponse>
            {
                StatusCode = (int)System.Net.HttpStatusCode.Created,
                Succeeded = true,
                Message = "Registration Successful"
            };
        }
    }
}