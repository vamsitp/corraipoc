namespace CorrAIPoc.Api
{
    using System;

    public class CorrelationInfo
    {
        public DateTime Date { get; set; }

        public string TraceId { get; set; }

        public string MessageBody { get; set; }

        public string MessageId { get; set; }

        public string DiagnosticId { get; set; }

        public string CorrelationId { get; set; }
    }
}
