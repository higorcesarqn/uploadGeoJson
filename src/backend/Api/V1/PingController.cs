using Api.Controllers;
using Core.Notifications;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;

namespace Api.V1
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class PingController : ApiController
    {
        public PingController(INotifications notifications) : base(notifications)
        {

        }

        [HttpGet]
        public IActionResult Get([FromServices]IHttpContextAccessor httpContext)
        {
            var request = httpContext.HttpContext.Request;

            return Response(new
            {
                Host = request.Host.Value,
                Path = request.Path.Value,
                request.Method,
                DateTime = DateTime.Now
            });
        }
    }
}
