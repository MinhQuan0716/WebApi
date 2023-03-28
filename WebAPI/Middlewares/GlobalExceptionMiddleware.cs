using Application.Utils;
using System.Net;
using System.Runtime.CompilerServices;

namespace WebAPI.Middlewares
{
    public class GlobalExceptionMiddleware : IMiddleware
    {

        private readonly ISendMailHelper _sendmailHelper;
        public GlobalExceptionMiddleware(ISendMailHelper sendmailHelper)
        {
            _sendmailHelper = sendmailHelper;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {

            try
            {
                await next(context);
            }
            catch (Exception ex)
            {

                await _sendmailHelper.SendMailAsync("quangtmse161987@fpt.edu.vn", "Exception", ex.Message);
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                context.Response.ContentType = "text/plain";
                // todo push notification & writing log
                Console.WriteLine("GlobalExceptionMiddleware");
                Console.WriteLine(ex.Message);
                await context.Response.WriteAsync(ex.Message);
                return;

            }
        }
    }
}
