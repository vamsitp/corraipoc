namespace CorrAI.Core
{
    using System.Threading.Tasks;

    using Microsoft.Azure.ServiceBus;
    using Microsoft.Extensions.Logging;

    public interface IServiceBusService
    {
        Task Enqueue(Message message, string queueName, bool newMessage, ILogger runLogger);
        Task Process(Message message, string queueName);
    }
}
