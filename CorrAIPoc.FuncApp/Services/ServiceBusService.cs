namespace CorrAI.Core
{
    using System;
    using System.Collections.Concurrent;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using Microsoft.Azure.ServiceBus;
    using Microsoft.Azure.ServiceBus.Diagnostics;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Logging;

    // https://docs.microsoft.com/en-us/azure/azure-monitor/app/custom-operations-tracking#service-bus-queue
    public class ServiceBusService : IServiceBusService, IDisposable
    {
        public const string InputQueueName = "inputq";

        public const string OutputQueueName = "outputq";

        private bool disposedValue = false;

        private readonly ILogger<ServiceBusService> logger;

        private static ConcurrentDictionary<string, IQueueClient> queueClients = new ConcurrentDictionary<string, IQueueClient>();

        public ServiceBusService(IConfiguration configuration, ILogger<ServiceBusService> logger)
        {
            this.logger = logger;
            var connectionString = configuration.GetValue<string>("SBConn");
            queueClients.GetOrAdd(InputQueueName, new QueueClient(connectionString, InputQueueName));
            queueClients.GetOrAdd(OutputQueueName, new QueueClient(connectionString, OutputQueueName));
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~ServiceBusHelper()
        // {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    foreach (var item in queueClients)
                    {
                        item.Value.CloseAsync();
                    }
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        public async Task Enqueue(Message message, string queueName, bool newMessage, ILogger runLogger)
        {
            this.logger.LogInformation($"{ this.GetType().Name } - Enqueue: {Encoding.UTF8.GetString(message.Body)} (queue: {queueName})");
            var outMessage = newMessage ? message : message.Clone();

            if (!newMessage)
            {
                message.UserProperties.ToList().ForEach(x =>
                {
                    if (!outMessage.UserProperties.Any(y => y.Key.Equals(x.Key, StringComparison.OrdinalIgnoreCase)))
                    {
                        outMessage.UserProperties.Add(x);
                    }
                });

                outMessage.Body = Encoding.UTF8.GetBytes("Updated message - " + Encoding.UTF8.GetString(message.Body));
            }

            // TODO: Change to proper SessionId later
            outMessage.SessionId = message.ExtractActivity().TraceId.ToHexString();

            try
            {
                var queueClient = queueClients[queueName];
                await queueClient.SendAsync(outMessage);
            }
            catch (Exception e)
            {
                this.logger.LogInformation(e.Message);
                throw;
            }
        }

        public Task Process(Message message, string queueName)
        {
            //this.logger.LogInformation($"{ this.GetType().Name } - Process: {Encoding.UTF8.GetString(message.Body)} (queue: {queueName})");

            //// After the message is taken from the queue, create RequestTelemetry to track its processing.
            //// It might also make sense to get the name from the message.
            //var requestTelemetry = new RequestTelemetry { Name = "process " + queueName };

            //var rootId = message.UserProperties["RootId"].ToString();
            //var parentId = message.UserProperties["ParentId"].ToString();
            //// Get the operation ID from the Request-Id (if you follow the HTTP Protocol for Correlation).
            //requestTelemetry.Context.Operation.Id = rootId;
            //requestTelemetry.Context.Operation.ParentId = parentId;

            ////var operation = telemetryClient.StartOperation(requestTelemetry);

            ////try
            ////{
            ////    // await ProcessMessage();
            ////}
            ////catch (Exception e)
            ////{
            ////    telemetryClient.TrackException(e);
            ////    throw;
            ////}
            ////finally
            ////{
            ////    // Update status code and success as appropriate.
            ////    telemetryClient.StopOperation(operation);
            ////}

            return Task.CompletedTask;
        }
    }
}
