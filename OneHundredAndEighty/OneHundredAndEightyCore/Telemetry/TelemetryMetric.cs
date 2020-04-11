#region Usings

using System;

#endregion

namespace OneHundredAndEightyCore.Telemetry
{
    public class TelemetryMetric : IComparable<TelemetryMetric>, IComparable
    {
        public TelemetryMetric(string name, double value)
        {
            Name = name;
            Value = value;
        }

        public string Name { get; }

        public double Value { get; }

        public static bool operator ==(TelemetryMetric left, TelemetryMetric right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(TelemetryMetric left, TelemetryMetric right)
        {
            return !Equals(left, right);
        }

        protected bool Equals(TelemetryMetric other)
        {
            return string.Equals(Name, other.Name);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }

            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            if (obj.GetType() != GetType())
            {
                return false;
            }

            return Equals((TelemetryMetric) obj);
        }

        public override int GetHashCode()
        {
            return Name.GetHashCode();
        }

        public int CompareTo(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return 1;
            }

            if (ReferenceEquals(this, obj))
            {
                return 0;
            }

            if (!(obj is TelemetryMetric))
            {
                throw new ArgumentException($"Object must be of type {nameof(TelemetryMetric)}");
            }

            return CompareTo((TelemetryMetric) obj);
        }

        public int CompareTo(TelemetryMetric other)
        {
            if (ReferenceEquals(this, other))
            {
                return 0;
            }

            if (ReferenceEquals(null, other))
            {
                return 1;
            }

            return string.Compare(Name, other.Name, StringComparison.Ordinal);
        }
    }
}