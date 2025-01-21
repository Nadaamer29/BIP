using BIP.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BIP.DataAccess.Response
{
    public class ResponseHandler
    {
        public Response<T> Success<T>(T entity, object meta = null)
        {
            return new Response<T>()
            {
                Data = entity,
                StatusCode = (int)System.Net.HttpStatusCode.OK,
                Succeeded = true,
                Message = "Request Successful",
                Meta = meta ?? new object()  
            };
        }
        public Response<T> BadRequest<T>(string message = null)
        {
            return new Response<T>()
            {
                StatusCode = (int)System.Net.HttpStatusCode.BadRequest,
                Succeeded = false,
                Message = message ?? "Bad Request"
            };
        }

        public Response<T> Unauthorized<T>()
        {
            return new Response<T>()
            {
                StatusCode = (int)System.Net.HttpStatusCode.Unauthorized,
                Succeeded = false,
                Message = "Unauthorized"
            };
        }

        public Response<T> NotFound<T>(string message = null)
        {
            return new Response<T>()
            {
                StatusCode = (int)System.Net.HttpStatusCode.NotFound,
                Succeeded = false,
                Message = message ?? "Resource Not Found"
            };
        }

        public Response<T> Created<T>(T entity, object meta = null)
        {
            return new Response<T>()
            {
                Data = entity,
                StatusCode = (int)System.Net.HttpStatusCode.Created,
                Succeeded = true,
                Message = "Created Successfully",
                Meta = meta
            };
        }

        public Response<T> Deleted<T>(string message = null)
        {
            return new Response<T>()
            {
                StatusCode = (int)System.Net.HttpStatusCode.OK,
                Succeeded = true,
                Message = message ?? "Deleted Successfully"
            };
        }

        public Response<T> UnprocessableEntity<T>(string message = null)
        {
            return new Response<T>()
            {
                StatusCode = (int)System.Net.HttpStatusCode.UnprocessableEntity,
                Succeeded = false,
                Message = message ?? "Unprocessable Entity"
            };
        }
    }
}
