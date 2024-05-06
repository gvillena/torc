namespace Torc.Services.BookLibrary.API.Extensions
{
    public static class WebApplicationExtensions
    {
        public static IApplicationBuilder UseDefaultOpenApi(this WebApplication app)
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Book Library V1");
            });

            // Add a redirect from the root of the app to the swagger endpoint
            app.MapGet("/", () => Results.Redirect("/swagger/v1/swagger.json")).ExcludeFromDescription();

            return app;
        }
    }
}
