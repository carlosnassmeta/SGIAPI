using IMS.Application.Interface;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using System.Net;

namespace IMS.Api.Information.Controllers
{
    [ApiController, Authorize]
    public class CustomControllerBase : ControllerBase
    {
        private readonly IValidationDictionary _validationDictionary;

        public CustomControllerBase(IValidationDictionary validationDictionary)
        {
            _validationDictionary = validationDictionary;
        }

        protected IActionResult Ok<TResponse>(TResponse response)
        {
            if (_validationDictionary.HasErrors) return BadRequest(response);
            return HttpRequest(response, HttpStatusCode.OK);
        }

        protected IActionResult BadRequest<TResponse>(TResponse response)
        {
            return HttpRequest(response, HttpStatusCode.BadRequest);
        }

        protected IActionResult NotFound<TResponse>(TResponse response)
        {
            return HttpRequest(response, HttpStatusCode.NotFound);
        }

        protected IActionResult InternalServerError<TResponse>(TResponse response)
        {
            return HttpRequest(response, HttpStatusCode.InternalServerError);
        }

        protected IActionResult HttpRequest<TResponse>(TResponse response, HttpStatusCode httpStatusCode)
        {
            if (response != null && _validationDictionary.HasErrors)
                return SetResponseBody(httpStatusCode, new { response, _validationDictionary.Errors });
            else if (response == null && _validationDictionary.HasErrors)
                return SetResponseBody(httpStatusCode, _validationDictionary.Errors);
            else if (response != null && !_validationDictionary.HasErrors)
                return SetResponseBody(httpStatusCode, response);
            else
                return SetResponseBody(httpStatusCode);
        }

        private IActionResult SetResponseBody(HttpStatusCode httpStatusCode, object response = null)
        {
            switch (httpStatusCode)
            {
                case HttpStatusCode.NotFound:
                    return StatusCode((int)HttpStatusCode.NotFound, response);
                case HttpStatusCode.InternalServerError:
                    return StatusCode((int)HttpStatusCode.InternalServerError, response);
                case HttpStatusCode.BadRequest:
                default:
                    return StatusCode((int)httpStatusCode, response);
            }
        }
    }
}
