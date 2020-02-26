namespace CorrAIPoc.FuncApp
{
    using System;
    using System.Diagnostics;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using CorrAI.Core;

    using Microsoft.Azure.ServiceBus;
    using Microsoft.Azure.ServiceBus.Diagnostics;
    using Microsoft.Azure.WebJobs;
    using Microsoft.Extensions.Logging;

    public class InputHandler : FuncBase
    {
        public InputHandler(IServiceBusService serviceBusHelper, IDatabaseService databaseHelper, ILogger<InputHandler> logger)
            : base(serviceBusHelper, databaseHelper, logger)
        {
        }

        [FunctionName("InputHandler")]
        [return: ServiceBus("outputq", Connection = "SBConn", EntityType = Microsoft.Azure.WebJobs.ServiceBus.EntityType.Queue)]
        public async Task<Message> Run([ServiceBusTrigger("inputq", Connection = "SBConn", IsSessionsEnabled = true)]Message message, ExecutionContext context, ILogger runLogger)
        {
            var currActivity = Activity.Current;
            var msgActivity = message.ExtractActivity();
            var body = Encoding.UTF8.GetString(message.Body);

            await this.Process(async () =>
            {
                runLogger.LogInformation($"rlg - InputHandler.Run() - {body} (currActivity.TraceId={currActivity?.TraceId.ToString() ?? string.Empty} / msgActivity.Id={msgActivity?.Id ?? string.Empty} / msg.CorrelationId={message.CorrelationId ?? string.Empty} / msg.props.diag-id={message.UserProperties["Diagnostic-Id"]} / context.InvocationId={context.InvocationId.ToString() ?? string.Empty})");
                this.logger.LogInformation($"glg - InputHandler.Run() - {body} (currActivity.TraceId={currActivity?.TraceId.ToString() ?? string.Empty} / msgActivity.Id={msgActivity?.Id ?? string.Empty} / msg.CorrelationId={message.CorrelationId ?? string.Empty} / msg.props.diag-id={message.UserProperties["Diagnostic-Id"]} / context.InvocationId={context.InvocationId.ToString() ?? string.Empty})");
                await Task.CompletedTask;
            }, message, runLogger);

            var outMessage = message.Clone();
            message.UserProperties.ToList().ForEach(x =>
            {
                if (!outMessage.UserProperties.Any(y => y.Key.Equals(x.Key, StringComparison.OrdinalIgnoreCase)))
                {
                    outMessage.UserProperties.Add(x);
                }
            });

            outMessage.Body = Encoding.UTF8.GetBytes("Updated message - " + body);
            outMessage.SessionId = body;
            return await Task.FromResult(outMessage);
        }
    }
}
