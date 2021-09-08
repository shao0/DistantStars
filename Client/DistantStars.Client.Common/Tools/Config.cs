using System.Configuration;
using System.Linq;
using DistantStars.Client.Common.Tools.Interfaces;

namespace DistantStars.Client.Common.Tools
{
    public class Config : IConfig
    {
        public string ReadByKey(string key)
        {
            var configuration = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            string result = string.Empty;
            if (configuration.AppSettings.Settings.AllKeys.Any(k => k == key))
                result = configuration.AppSettings.Settings[key].Value;
            return result;
        }
    }
}
