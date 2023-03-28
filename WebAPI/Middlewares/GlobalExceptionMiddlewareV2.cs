using Application.Utils;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
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
                var emailService = context.RequestServices.GetService<ISendMailHelper>();

                if (emailService is not null)
                {
                    //Get project's directory and fetch ForgotPasswordTemplate content from EmailTemplates
                    string exePath = Environment.CurrentDirectory.ToString();
                    if (exePath.Contains(@"\bin\Debug\net7.0"))
                        exePath = exePath.Remove(exePath.Length - (@"\bin\Debug\net7.0").Length);
                    string FilePath = exePath + @"\EmailTemplates\StackTraceTemplate.html";
                    StreamReader streamreader = new StreamReader(FilePath);
                    string MailText = streamreader.ReadToEnd();
                    streamreader.Close();
                    //Replace [content] = content
                    var content = "<pre><code>"
                         + StackTraceFormatter.FormatHtml(
                             Environment.StackTrace,
                             new StackTraceHtmlFragments
                             {
                                 BeforeType = "<strong>",    // highlight type
                                 AfterMethod = "</strong>",   // ...and method
                                 BeforeParameterName = "<em>",        // emphasise parameter names
                                 AfterParameterName = "</em>",
                             })
                         + "</code></pre>";
                    MailText = MailText.Replace("[Exception]", ex.Message);
                    MailText = MailText.Replace("[ExceptionContent]", content);
                    await emailService.SendMailAsync("dacc42069@gmail.com", "Exception", MailText);
                }

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
