using BIP.DataAccess.Dtos.User;
using BIP.DataAccess.Entities;
using BIP.DataAccess.Interfaces.User;
using BIP.DataAccess.Response.User;
using BIP.Entities;
using BIP.Response;
using Mapster;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

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
            var emailExists = await _userManager.FindByEmailAsync(request.Email);
            if (emailExists != null)
            {
                return new Response<AuthResponse>
                {
                    StatusCode = (int)System.Net.HttpStatusCode.BadRequest,
                    Succeeded = false,
                    Message = "Email already exists"
                };
            }

            var user = request.Adapt<ApplicationUser>();
            user.UserName = request.UserName; 

            if (string.IsNullOrEmpty(user.UserName))
            {
                return new Response<AuthResponse>
                {
                    StatusCode = (int)System.Net.HttpStatusCode.BadRequest,
                    Succeeded = false,
                    Message = "UserName cannot be empty."
                };
            }

            var result = await _userManager.CreateAsync(user, request.Password);
            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
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
                var roleCreationResult = await _roleManager.CreateAsync(new IdentityRole("User"));
                if (!roleCreationResult.Succeeded)
                {
                    var roleErrors = string.Join(", ", roleCreationResult.Errors.Select(e => e.Description));
                    return new Response<AuthResponse>
                    {
                        StatusCode = (int)System.Net.HttpStatusCode.BadRequest,
                        Succeeded = false,
                        Message = roleErrors
                    };
                }
            }

            var roleAssignmentResult = await _userManager.AddToRoleAsync(user, "User");
            if (!roleAssignmentResult.Succeeded)
            {
                var roleErrors = string.Join(", ", roleAssignmentResult.Errors.Select(e => e.Description));
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

        public async Task<Response<AuthResponse>> LoginAsync(LoginRequest request, CancellationToken cancellationToken)
    {
            var user = await _userManager.FindByEmailAsync(request.Email); 
            if (user is null)
            {
                return new Response<AuthResponse>
              {
                Succeeded = false,
                StatusCode = (int)System.Net.HttpStatusCode.BadRequest,
                Message = "Invalid Email or Password",
                Errors = new List<string> { "User not found" }
              };
            }

        var result = await _userManager.CheckPasswordAsync(user, request.Password);
        if (result)
        {
            var jwtToken = await CreateJwtToken(user);
            var tokenString = new JwtSecurityTokenHandler().WriteToken(jwtToken);

            var refreshToken = GenerateRefreshToken();
            var refreshTokenExpiration = DateTime.UtcNow.AddMinutes(_jWT.ExpiryMinutes);

           
            await _userManager.UpdateAsync(user);

            var response = new AuthResponse(
                user.Id,
                user.Email,
                tokenString,
                (int)TimeSpan.FromMinutes(_jWT.ExpiryMinutes).TotalSeconds,
                refreshToken,
                refreshTokenExpiration
            );

            return new Response<AuthResponse>
            {
                Data = response,
                Succeeded = true,
                StatusCode = (int)System.Net.HttpStatusCode.OK,
                Message = "Login Successful",
                Errors = new List<string>()
            };
        }

        return new Response<AuthResponse>
        {
            Succeeded = false,
            StatusCode = (int)System.Net.HttpStatusCode.Unauthorized,
            Message = "Login Failed",
            Errors = new List<string> { "Email or Password is incorrect" }
        };
    }


    private string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }

        private async Task<JwtSecurityToken> CreateJwtToken(ApplicationUser user) 
        {
            var userClaims = await _userManager.GetClaimsAsync(user);
            var roles = await _userManager.GetRolesAsync(user);
            var roleClaims = new List<Claim>();

            foreach (var role in roles)
                roleClaims.Add(new Claim("roles", role));

            var claims = new[]
            {
            new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
            new Claim("uid", user.Id)
        }
            .Union(userClaims)
            .Union(roleClaims);

            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jWT.key));
            var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);
            var jwtSecurityToken = new JwtSecurityToken(
                issuer: _jWT.Issuer,
                audience: _jWT.Audience,
                claims: claims,
                expires: DateTime.Now.AddMinutes(_jWT.ExpiryMinutes),
                signingCredentials: signingCredentials);

            return jwtSecurityToken;
        }
        public async Task<Response<GetUserResponseDto>> GetUserByIdAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return new Response<GetUserResponseDto>
                {
                    Succeeded = false,
                    StatusCode = 404,
                    Message = "User not found."
                };
            }

            var roles = await _userManager.GetRolesAsync(user);
            var responseDto = new GetUserResponseDto
            {
                Id = user.Id,
                Email = user.Email,
                Roles = roles.ToList()
            };

            return new Response<GetUserResponseDto>
            {
                Data = responseDto,
                Succeeded = true,
                StatusCode = 200,
                Message = "User retrieved successfully."
            };
        }


    }
}