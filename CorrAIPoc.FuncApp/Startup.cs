using CorrAIPoc.FuncApp;

using Microsoft.Azure.Functions.Extensions.DependencyInjection;

[assembly: FunctionsStartup(typeof(Startup))]
namespace CorrAIPoc.FuncApp
{
    using CorrAI.Core;
    using Microsoft.Azure.Functions.Extensions.DependencyInjection;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;

    public class Startup : FunctionsStartup
    {
        //public static readonly ILoggerFactory EFLoggerFactory = LoggerFactory.Create(builder =>
        //{
        //    builder
        //    .AddFilter((category, level) => category == DbLoggerCategory.Database.Command.Name) // && level == LogLevel.Information)
        //    .AddApplicationInsights(Utils.AppInsightsKey);
        //});

        public override void Configure(IFunctionsHostBuilder builder)
        {
            // builder.Services.AddApplicationInsightsTelemetry();
            // builder.Services.AddSingleton<ILogger, Core.Logger>();
            // builder.Services.AddSingleton(typeof(ILogger<>), typeof(Core.Logger<>));
            //builder.Services.AddDbContext<CorrAIDBContext>(
            //    options =>
            //    options
            //        .UseLoggerFactory(EFLoggerFactory)
            //        .UseSqlServer("SqlConn".GetConfigValue(), options => options.EnableRetryOnFailure()));
            builder.Services.AddScoped<CorrAIDBContext>();
            builder.Services.AddScoped<IDatabaseService, DatabaseService>();
            builder.Services.AddSingleton<IServiceBusService, ServiceBusService>();
        }
    }
}
