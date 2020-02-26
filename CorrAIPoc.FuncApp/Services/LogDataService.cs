namespace CorrAI.Core
{
    using System.Collections.Generic;
    using System.Threading.Tasks;


    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Logging;

    public class LogDataService : ILogDataService
    {
        // https://dev.applicationinsights.io/quickstart
        // https://dev.loganalytics.io/documentation/Authorization/API-Keys
        // https://api.applicationinsights.io/{version}/apps/{app-id}/{operation}/[path]?[parameters]
        // X-API-Key:{key}

        private const string Query = "union traces, customEvents, exceptions, requests, dependencies " +
            "| where timestamp between(ago({0}) .. now()) {1}" +
            "and name !startswith('GET ') and name !endswith('.json') and name != 'AcceptMessageSession' " +
            "and customDimensions.Category !contains 'Host' " +
            "and customDimensions.Category !contains 'Microsoft.Azure.WebJobs' " +
            "and message !contains 'AI (Internal)' " +
            "and * !contains 'RenewSessionLock' " +
            "and * !contains 'AcceptMessageSession' " +
            "| order by timestamp asc " +
            "| project id=row_number(), timestamp, client_City, itemType, cloud_RoleName, message=case(isnotempty(name), strcat(name, ' | ', data), isnotempty(problemId), strcat('Ex: ', problemId), message), session_Id, operation_Id, operation_ParentId, appName, appId, itemId, details, customDimensions";
        private const string Url = "https://api.applicationinsights.io/v1/apps/{0}/query";
        private readonly IConfiguration config;
        private readonly ILogger<LogDataService> logger;
        // private static readonly List<PropertyInfo> props = typeof().GetProperties(BindingFlags.Public | BindingFlags.Instance).ToList();

        public LogDataService(IConfiguration config, ILogger<LogDataService> logger)
        {
            this.config = config;
            this.logger = logger;
        }

        public async Task<IEnumerable<T>> GetLogs<T>(string[] logTypes, string timeframe = "30m", string traceId = null)
        {
            logger.LogInformation($"{ this.GetType().Name } - GetLogs()");
            return await Task.FromResult(default(IEnumerable<T>));
        }
    }
}
