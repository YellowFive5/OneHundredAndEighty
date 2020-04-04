#region Usings

using System;
using System.Globalization;
using NLog;

#endregion

namespace OneHundredAndEightyCore.Common
{
    public class ConfigService
    {
        private readonly object locker;
        private readonly Logger logger;
        private readonly DBService dbService;

        public ConfigService(Logger logger, DBService dbService)
        {
            this.logger = logger;
            this.dbService = dbService;
            locker = new object();
        }

        public void Write(SettingsType key, object value)
        {
            lock (locker)
            {
                dbService.SettingsSetValue(key, Converter.ToString(value));
            }
        }

        public T Read<T>(SettingsType key)
        {
            lock (locker)
            {
                object value;

                if (typeof(T) == typeof(double))
                {
                    value = Converter.ToDouble(dbService.SettingsGetValue(key));
                }
                else if (typeof(T) == typeof(decimal))
                {
                    value = Converter.ToDecimal(dbService.SettingsGetValue(key));
                }
                else if (typeof(T) == typeof(float))
                {
                    value = Converter.ToFloat(dbService.SettingsGetValue(key));
                }
                else if (typeof(T) == typeof(int))
                {
                    value = Converter.ToInt(dbService.SettingsGetValue(key));
                }
                else if (typeof(T) == typeof(bool))
                {
                    value = Converter.ToBool(dbService.SettingsGetValue(key));
                }
                else if (typeof(T) == typeof(string))
                {
                    value = dbService.SettingsGetValue(key);
                }
                else
                {
                    throw new Exception($"Not supported type for {nameof(Read)} method");
                }

                return (T) value;
            }
        }
    }
}