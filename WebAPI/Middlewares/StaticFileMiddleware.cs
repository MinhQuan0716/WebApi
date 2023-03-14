using Microsoft.Extensions.FileProviders;

namespace WebAPI.Middlewares
{
    public class StaticFileMiddleWare
    {
        public void Configure(IApplicationBuilder app, Microsoft.Extensions.Hosting.IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            // serve static files
            app.UseStaticFiles();

            /*   app.UseStaticFiles(new StaticFileOptions()
               {
                   FileProvider = new PhysicalFileProvider(
                           Path.Combine(Directory.GetCurrentDirectory(), @"Resources\Files")),
                   RequestPath = new PathString("/StaticFiles")
               });*/
            
        }      
    }
}
