namespace OneHundredAndEightyCore.Common
{
    public interface IConfigService
    {
        void Write(SettingsType key, object value);
        T Read<T>(SettingsType key);
    }
}