namespace CorrAI.Core
{
    public interface ILogDataService
    {
        System.Threading.Tasks.Task<System.Collections.Generic.IEnumerable<T>> GetLogs<T>(string[] logTypes, string timeframe = "30m", string traceId = null);
    }
}