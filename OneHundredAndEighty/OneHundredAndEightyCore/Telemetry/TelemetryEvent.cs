#region Usings

using System;
using System.Collections.Generic;

#endregion

namespace OneHundredAndEightyCore.Telemetry
{
    public class TelemetryEvent
    {
        public TelemetryEvent(string name)
        {
            Name = name;
        }

        private readonly Dictionary<string, double> clientSideMetrics = new Dictionary<string, double>();
        private readonly Dictionary<string, string> clientSideData = new Dictionary<string, string>();
        private Guid OperationId { get; } = Guid.NewGuid();
        public string Name { get; }

        public void AddClientSideData(string dataKey, string dataValue)
        {
            clientSideData[dataKey] = !string.IsNullOrWhiteSpace(dataValue)
                                          ? dataValue
                                          : "<null>";
        }

        public void AddClientSideData(string dataKey, DateTime dataValue)
        {
            clientSideData[dataKey] = dataValue.ToString("s");
        }

        public void AddClientSideMetric(string metricKey, double metricValue)
        {
            clientSideMetrics[metricKey] = metricValue;
        }

        public void AddClientSideMetric(string metricGroup, string metricKey, double metricValue)
        {
            clientSideMetrics[$"{metricGroup}_{metricKey}"] = metricValue;
        }

        public Dictionary<string, string> GetData()
        {
            var dictionary = GetDataInternal();
            if (dictionary == null || dictionary.Count == 0)
            {
                return null;
            }

            return dictionary;
        }

        public Dictionary<string, double> GetMetrics()
        {
            var dictionary = GetMetricsInternal();
            if (dictionary == null || dictionary.Count == 0)
            {
                return null;
            }

            return dictionary;
        }

        private Dictionary<string, string> GetDataInternal()
        {
            var allData = new Dictionary<string, string>
                          {
                              {"OperationId", OperationId.ToString()}
                          };

            foreach (var clientSideDataEntry in clientSideData)
            {
                allData[clientSideDataEntry.Key] = clientSideDataEntry.Value;
            }

            return allData;
        }

        private Dictionary<string, double> GetMetricsInternal()
        {
            var allMetrics = new Dictionary<string, double>();
            foreach (var clientSideMetric in clientSideMetrics)
            {
                allMetrics[clientSideMetric.Key] = clientSideMetric.Value;
            }

            return allMetrics;
        }
    }
}