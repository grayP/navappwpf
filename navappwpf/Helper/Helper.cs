using System;
using System.Collections.Generic;
using System.Configuration;
using static navappwpf.Constants.Enums;

namespace navappwpf.Helper
{
    public static class ApplicationSettingHelper
    {
        #region Access App.Config appSettings
        /// <summary>
        /// Gets Value for Key from App.Config. Returns null if key does not exist.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string GetFromApplicationSetting(ApplicationSettingKey key)
        {
            return ConfigurationManager.AppSettings.Get(key.ToString());
        }
        public static string GetFromApplicationSetting(string key)
        {
            return ConfigurationManager.AppSettings.Get(key);
        }

        public static void SaveApplicationSetting(string value , string key)
        {
            Configuration oConfig = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            oConfig.AppSettings.Settings[key].Value = value;
            oConfig.Save(ConfigurationSaveMode.Full);
            ConfigurationManager.RefreshSection("AppSettings");
        }
        #endregion
    }

}
