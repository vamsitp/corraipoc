namespace CorrAIPoc.Api.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Text;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Azure.ServiceBus;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Logging;

    [ApiController]
    [Route("api")]
    public class CorrelationController : ControllerBase
    {
        private readonly ILogger<CorrelationController> logger;
        private readonly IConfiguration configuration;

        public CorrelationController(ILogger<CorrelationController> logger, IConfiguration configuration)
        {
            this.logger = logger;
            this.configuration = configuration;
        }

        [HttpGet]
        public async Task<IEnumerable<CorrelationInfo>> Get()
        {
            return await Process(DateTime.Now.Ticks.ToString());
        }

        [HttpGet("{message}")]
        public async Task<IEnumerable<CorrelationInfo>> Get([FromRoute]string message)
        {
            return await Process(message);
        }

        private async Task<IEnumerable<CorrelationInfo>> Process(string message)
        {
            this.logger.LogInformation(message);
            var connectionString = configuration.GetValue<string>("SBConn");
            var sbClient = new QueueClient(connectionString, "inputq");
            var sbMessage = new Message(Encoding.UTF8.GetBytes(message)) { SessionId = message };
            await sbClient.SendAsync(sbMessage);
            this.logger.LogInformation($"{message} Sent - sbMessage.Diagnostic-Id = {sbMessage.UserProperties["Diagnostic-Id"]} / currActivity.TraceId = {Activity.Current?.TraceId.ToString() ?? string.Empty}");

            return new[] { new CorrelationInfo
            {
                Date = DateTime.Now,
                MessageBody = message,
                TraceId = Activity.Current?.TraceId.ToString(),
                MessageId = sbMessage.MessageId,
                DiagnosticId = sbMessage.UserProperties["Diagnostic-Id"].ToString(),
                CorrelationId = sbMessage.CorrelationId
            }};
        }
    }
}
