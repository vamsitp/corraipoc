namespace CorrAIPoc.FuncApp
{
    using System.Diagnostics;
    using System.Text;
    using System.Threading.Tasks;

    using Microsoft.Azure.ServiceBus;
    using Microsoft.Azure.ServiceBus.Diagnostics;
    using Microsoft.Azure.WebJobs;
    using Microsoft.Extensions.Logging;

    public class InputHandler
    {
        [FunctionName("InputHandler")]
        public void Run([ServiceBusTrigger("inputq", Connection = "SBConn", IsSessionsEnabled = true)]Message message, ExecutionContext context, ILogger logger)
        {
            var currActivity = Activity.Current;
            var msgActivity = message.ExtractActivity();
            var body = Encoding.UTF8.GetString(message.Body);

            logger.LogInformation($" - {body} (currActivity.TraceId={currActivity?.TraceId.ToString() ?? string.Empty} / msgActivity.Id={msgActivity?.Id ?? string.Empty} / msg.CorrelationId={message.CorrelationId ?? string.Empty} / msg.props.diag-id={message.UserProperties["Diagnostic-Id"]} / context.InvocationId={context.InvocationId.ToString() ?? string.Empty})");
        }
    }
}
