namespace CorrAIPoc.Api.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Azure.ServiceBus;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Logging;

    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> logger;
        private readonly IConfiguration configuration;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, IConfiguration configuration)
        {
            this.logger = logger;
            this.configuration = configuration;
        }

        [HttpGet]
        public async Task<IEnumerable<WeatherForecast>> Get([FromQuery]string message)
        {
            var body = string.IsNullOrWhiteSpace(message) ? DateTime.Now.Ticks.ToString() : message;
            this.logger.LogInformation(body);
            var connectionString = configuration.GetValue<string>("SBConn");
            var sbClient = new QueueClient(connectionString, "inputq");
            var sbMessage = new Message(Encoding.UTF8.GetBytes(body)) { SessionId = body };
            await sbClient.SendAsync(sbMessage);
            this.logger.LogInformation($"{body} - sbMessage.Diagnostic-Id = {sbMessage.UserProperties["Diagnostic-Id"]} / currActivity.TraceId = {Activity.Current?.TraceId.ToString() ?? string.Empty}");

            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            })
            .ToArray();
        }
    }
}
