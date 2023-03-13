using Application.Utils;
using System.Net;

namespace WebAPI.Middlewares
{
    public class GlobalExceptionMiddlewareV2
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionMiddlewareV2> _logger;

        public GlobalExceptionMiddlewareV2(RequestDelegate next, ILogger<GlobalExceptionMiddlewareV2> logger)
        {
            _next = next;
            _logger = logger;
        }
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                // var emailService = context.RequestServices.GetService<ISendMailHelper>();
                
                // if (emailService is not null)
                // {
                //     await emailService.SendMailAsync("quangtmse161987@fpt.edu.vn", "Exception", ex.ToString()); 
                // }
                
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                context.Response.ContentType = "text/plain";
                // todo push notification & writing log
                _logger.LogError("Exception: ");
                _logger.LogError(ex.ToString());
                await context.Response.WriteAsync(ex.Message);
             

            }
        }
    }
}
