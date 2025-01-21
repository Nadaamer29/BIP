using BIP.Base;
using BIP.DataAccess.Dtos.User;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace BIP.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApplicationUserController : AppControllerBase
    {
      //  private readonly IMediator _mediator;

        public ApplicationUserController()
        {
           // _mediator = mediator;
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterAsync([FromBody] RegisterUserCommand command, CancellationToken cancellationToken)
        {
            if (command == null)
            {
                return BadRequest(new { Message = "Command is null" });
            }

            if (!ModelState.IsValid)
            {
                var invalidResponse = new
                {
                    Succeeded = false,
                    Message = "Invalid data",
                    Errors = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage))
                };
                return BadRequest(invalidResponse);
            }

            var response = await Mediator.Send(command, cancellationToken);

            if (response.Succeeded)
            {
                return StatusCode(StatusCodes.Status201Created, response);
            }
            else
            {
                return BadRequest(response);
            }
        }


        [HttpPost("login")]
        public async Task<IActionResult> LoginAsync([FromBody] LoginUserCommand command, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { Message = "Invalid data" });
            }

            var response = await Mediator.Send(command, cancellationToken);

            if (response.Succeeded)
            {
                return Ok(response);
            }
            else
            {
                return BadRequest(response);
            }
        }

        //    [HttpGet("{id}")]
        //    public async Task<IActionResult> GetUserByIdAsync(string id)
        //    {
        //        var response = await _mediator.Send(new GetUserByIdCommand { UserId = id });

        //        if (response.Succeeded)
        //        {
        //            return Ok(response);
        //        }

        //        return StatusCode(response.StatusCode, response);
        //    }
        //}
    }
}
