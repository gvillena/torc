
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Torc.Services.BookLibrary.API.Extensions;
using Torc.Services.BookLibrary.Infrastructure;
using Torc.Services.BookLibrary.Infrastructure.Repositories;

namespace Torc.Services.BookLibrary.API
{
    public class Program
    {
        public async static Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
                
            builder.Services.AddControllers();
            builder.Services.AddDefaultOpenApi(builder.Configuration);
            builder.Services.AddDbContexts(builder.Configuration);
            builder.Services.AddAzureServiceBus(builder.Configuration);
            builder.Services.AddCors(builder.Configuration);
            
            builder.Services.AddScoped<IBookRepository, BookRepository>();
            

            var app = builder.Build();

            app.UseDefaultOpenApi();
            app.MapControllers();
            app.UseCors();

            using (var scope = app.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<BookLibraryContext>();
                var env = app.Services.GetService<IWebHostEnvironment>();                
                await context.Database.MigrateAsync();
            }

            await app.RunAsync();
        }
    }
}
