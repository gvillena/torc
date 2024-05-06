using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Torc.Services.BookLibrary.Infrastructure;
using Microsoft.OpenApi.Models;
using Torc.BuildingBlocks.EventBus.Absractions;
using Torc.BuildingBlocks.EventBus.AzureServiceBus;
using System.Runtime.ConstrainedExecution;

namespace Torc.Services.BookLibrary.API.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddDbContexts(this IServiceCollection services, IConfiguration configuration)
        {
            static void ConfigureSqlOptions(SqlServerDbContextOptionsBuilder sqlOptions)
            {
                sqlOptions.MigrationsAssembly(typeof(Program).Assembly.FullName);

                sqlOptions.EnableRetryOnFailure(maxRetryCount: 15, maxRetryDelay: TimeSpan.FromSeconds(30), errorNumbersToAdd: null);
            };

            services.AddDbContext<BookLibraryContext>(options =>
            {                
                options.UseSqlServer(configuration.GetRequiredConnectionString("BookLibraryDB"), ConfigureSqlOptions);
            });
            
            return services;
        }

        public static IServiceCollection AddDefaultOpenApi(this IServiceCollection services, IConfiguration configuration)
        {
            var openApi = configuration.GetSection("OpenApi");

            if (!openApi.Exists())
            {
                return services;
            }

            services.AddEndpointsApiExplorer();

            return services.AddSwaggerGen(options =>
            {
                var document = openApi.GetRequiredSection("Document");

                var version = document.GetRequiredValue("Version") ?? "v1";

                options.SwaggerDoc(version, new OpenApiInfo
                {
                    Title = document.GetRequiredValue("Title"),
                    Version = version,
                    Description = document.GetRequiredValue("Description")
                });
            });
        }

        public static IServiceCollection AddAzureServiceBus(this IServiceCollection services, IConfiguration configuration)
        {
            var azureServiceBus = configuration.GetSection("AzureServiceBus");

            if (!azureServiceBus.Exists())
            {
                return services;
            }

            var torcBookLibrary = azureServiceBus.GetRequiredSection("TorcBookLibrary");
            
            var namespaceName = torcBookLibrary.GetRequiredValue("NamespaceName");
            var events = torcBookLibrary.GetRequiredSection("Events");

            var bookAddedEvent = events.GetRequiredSection("BookAdded");
            var topicNAme = bookAddedEvent.GetRequiredValue("TopicName");
            var subscriptionName= bookAddedEvent.GetRequiredValue("SubscriptionName");

            var eventsInfo = new Dictionary<string, AzureServiceBusEventBusInfo>();
            eventsInfo["BookAdded"] = new AzureServiceBusEventBusInfo(topicNAme, subscriptionName);

            services.AddSingleton<IEventBus, AzureServiceBusEventBus>(serviceProvider => {
                return new AzureServiceBusEventBus(namespaceName, eventsInfo, serviceProvider);
            });

            return services;
        }

        public static IServiceCollection AddCors(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddCors(options =>
            {
                options.AddDefaultPolicy(
                    builder =>
                    {
                        builder.WithOrigins("http://localhost:8080", "http://localhost:5173")
                               .AllowAnyHeader()
                               .AllowAnyMethod();
                    });
            });

            return services;
        }
    }
}
