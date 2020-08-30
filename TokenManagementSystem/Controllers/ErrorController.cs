namespace TokenManagementSystem.Controllers
{
    using System.Diagnostics.CodeAnalysis;
    using System.Net;
    using System.Net.Http;
    using Microsoft.AspNetCore.Diagnostics;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;

    [Route("api/[controller]")]
    [ApiExplorerSettings(IgnoreApi = true)]
    [ExcludeFromCodeCoverage]
    public class ErrorController : ControllerBase
    {
        [Route("/error")]
        public HttpResponseMessage Error()
        {
            var context = HttpContext.Features.Get<IExceptionHandlerFeature>();
            var exception = context?.Error;

            var response = new HttpResponseMessage()
            {
                Content = new StringContent(exception.Message),
                StatusCode = HttpStatusCode.InternalServerError
            };

            return response;
        }
    }
}
