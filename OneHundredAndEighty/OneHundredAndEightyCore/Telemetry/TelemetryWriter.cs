#region Usings

using System.Threading.Tasks;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.Extensibility;

#endregion

namespace OneHundredAndEightyCore.Telemetry
{
    public class TelemetryWriter
    {
        private static readonly object SyncTelemetryWrite = new object();
        private readonly TelemetryClient telemetryClient;
        private const string AppInsightsInstrumentationKey = "d72980ab-a100-4605-8e57-b47d7814f5cd";

        public TelemetryWriter()
        {
            telemetryClient = new TelemetryClient();
            Initialize();
        }

        private void Initialize()
        {
            var config = new TelemetryConfiguration(AppInsightsInstrumentationKey);
            TelemetryConfiguration.Active.InstrumentationKey = config.InstrumentationKey;
            telemetryClient.InstrumentationKey = AppInsightsInstrumentationKey;
            TelemetryConfiguration.Active.TelemetryInitializers.Add(new TelemetryInitializer());
        }

        private void Flush()
        {
            lock (SyncTelemetryWrite)
            {
                telemetryClient.Flush();
            }
        }

        private async Task WriteEventAsync(TelemetryEvent telemetryEvent)
        {
            if (telemetryEvent == null)
            {
                return;
            }

            await Task.Factory.StartNew(() =>
                                        {
                                            telemetryClient.TrackEvent(telemetryEvent.Name, telemetryEvent.GetData(), telemetryEvent.GetMetrics());
                                            Flush();
                                        });
        }

        public async Task WriteMetricAsync(TelemetryMetric metric)
        {
            if (metric == null)
            {
                return;
            }

            await Task.Factory.StartNew(() =>
                                        {
                                            telemetryClient.TrackMetric(metric.Name, metric.Value);
                                            Flush();
                                        });
        }

        public async Task WriteAppStart()
        {
            var simpleTelemetryEvent = new TelemetryEvent("App start");
            await WriteEventAsync(simpleTelemetryEvent);
        }
    }
}