using System.Configuration;

namespace VRChatify
{
    internal class Config
    {

        public static void SetConfig(string key, string value)
        {
            var configFile = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            var settings = configFile.AppSettings.Settings;
            if (settings[key] == null)
            {
                settings.Add(key, value);
            }
            else
            {
                settings[key].Value = value;
            }
            configFile.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection(configFile.AppSettings.SectionInformation.Name);
        }

        public static string GetConfig(string key)
        {
            return ConfigurationManager.AppSettings[key];
        }

    }
}
