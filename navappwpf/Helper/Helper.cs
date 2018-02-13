using System;
using System.Collections.Generic;
using System.Configuration;
using static navappwpf.Constants.Enums;

namespace navappwpf.Helper
{
    public static class Helper
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
        #endregion
    }

}
