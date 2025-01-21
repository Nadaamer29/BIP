using BIP.DataAccess.Dtos.User;
using BIP.DataAccess.Interfaces.User;
using BIP.DataAccess.Response;
using BIP.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BIP.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserRepository _userService;

        public UsersController(IUserRepository userService)
        {
            _userService = userService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterAsync([FromBody] RegisterRequestdto request, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { Message = "Invalid data" });
            }

            var response = await _userService.RegisterAsync(request, cancellationToken);

            if (response.Succeeded)
            {
                return StatusCode(StatusCodes.Status201Created, response); 
            }
            else
            {
                return BadRequest(response); 
            }
        }
    }


        //[HttpPost("register")]
        //public async Task<IActionResult> Register([FromBody] RegisterModel model)
        //{
        //    var user = new ApplicationUser { UserName = model.IdentityNumber, Email = model.Email };
        //    var result = await _userManager.CreateAsync(user, model.Password);

        //    if (result.Succeeded)
        //    {
        //        var roles = model.Roles ?? new string[] { };
        //        await _userManager.AddToRolesAsync(user, roles);

        //        return Ok(_responseHandler.Created(user));
        //    }

        //    return BadRequest(_responseHandler.BadRequest(result.Errors.Select(e => e.Description).ToList()));
        //}
    }
