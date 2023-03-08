using System.Net;

namespace WebAPI.Middlewares
{
    public class GlobalExceptionMiddleware : IMiddleware
    {
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                context.Response.ContentType = "text/plain";
                // todo push notification & writing log
                Console.WriteLine("GlobalExceptionMiddleware");
                Console.WriteLine(ex.Message);
                await context.Response.WriteAsync(ex.Message) ;
                return;
               
            }
        }
    }
}
