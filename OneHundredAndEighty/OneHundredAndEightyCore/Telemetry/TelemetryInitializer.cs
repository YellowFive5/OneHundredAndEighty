#region Usings

using DeviceId;
using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.Extensibility;

#endregion

namespace OneHundredAndEightyCore.Telemetry
{
    public class TelemetryInitializer : ITelemetryInitializer
    {
        public void Initialize(ITelemetry telemetry)
        {
            telemetry.Context.Properties["ClientId"] = new DeviceIdBuilder().AddSystemUUID().ToString();
        }
    }
}