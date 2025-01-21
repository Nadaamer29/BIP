using BIP.Response;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace BIP.Base
{

    [Route("api/[controller]")]
    [ApiController]
    public class AppControllerBase : ControllerBase
    {
        #region Properties
      
        private IMediator _mediatorInstance;
        protected IMediator Mediator => _mediatorInstance ??= HttpContext.RequestServices.GetService<IMediator>();
        #endregion
        #region Actions
        public ObjectResult NewResult<T>(Response<T> response)
        {
            switch (response.StatusCode)
            {
                case (int)HttpStatusCode.OK:
                    return new OkObjectResult(response);
                case     (int)HttpStatusCode.Created:
                    return new CreatedResult(string.Empty, response);
                case (int)HttpStatusCode.Unauthorized:
                    return new UnauthorizedObjectResult(response);
                case (int)HttpStatusCode.BadRequest:
                    return new BadRequestObjectResult(response);
                case (int)HttpStatusCode.NotFound:
                    return new NotFoundObjectResult(response);
                case (int)HttpStatusCode.Accepted:
                    return new AcceptedResult(string.Empty, response);
                case (int)HttpStatusCode.UnprocessableEntity:
                    return new UnprocessableEntityObjectResult(response);
                default:
                    return new BadRequestObjectResult(response);
            }
        }
        #endregion
    }

}
