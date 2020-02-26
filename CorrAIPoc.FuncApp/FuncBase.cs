namespace CorrAIPoc.FuncApp
{
    using System;
    using System.IO;
    using System.Runtime.CompilerServices;
    using System.Threading.Tasks;

    using CorrAI.Core;

    using Microsoft.Azure.ServiceBus;
    using Microsoft.Azure.ServiceBus.Diagnostics;
    using Microsoft.Extensions.Logging;

    public class FuncBase
    {
        protected readonly ILogger<FuncBase> logger;
        protected readonly IDatabaseService dbService;
        protected readonly IServiceBusService sbService;

        public FuncBase(IServiceBusService serviceBusHelper, IDatabaseService databaseHelper, ILogger<FuncBase> logger)
        {
            this.sbService = serviceBusHelper;
            this.dbService = databaseHelper;
            this.logger = logger;
        }

        protected async Task Process(Func<Task> func, Message message, ILogger runLogger, [CallerFilePath] string cfp = "")
        {
            await func();
        }
    }
}
