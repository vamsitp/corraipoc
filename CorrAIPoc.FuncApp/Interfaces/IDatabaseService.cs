namespace CorrAI.Core
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Microsoft.Extensions.Logging;

    public interface IDatabaseService
    {
        Task InsertAsync(string message, string traceId, ILogger runLogger);
        Task<IList<T>> GetAsync<T>(string message);
    }
}
