namespace OneHundredAndEightyCore.Common
{
    public interface IDBService
    {
        void SettingsSetValue(SettingsType name, string value);
        string SettingsGetValue(SettingsType name);
    }
}