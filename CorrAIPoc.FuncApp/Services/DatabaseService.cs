namespace CorrAI.Core
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.Extensions.Logging;

    public class DatabaseService : IDatabaseService
    {
        private readonly ILogger<DatabaseService> logger;
        private readonly CorrAIDBContext dbContext;

        public DatabaseService(ILogger<DatabaseService> logger, CorrAIDBContext dbContext)
        {
            this.logger = logger;
            this.dbContext = dbContext;
        }

        public async Task InsertAsync(string message, string traceId, ILogger runLogger)
        {
            this.logger.LogInformation($"{ this.GetType().Name } - DatabaseHelper.Insert({message}) called");
            await Task.CompletedTask;
        }

        public async Task<IList<T>> GetAsync<T>(string message)
        {
            this.logger.LogInformation(null, $"{ this.GetType().Name } - DatabaseHelper.Get({message}) called");
            return await Task.FromResult(default(IList<T>));
        }
    }
}
