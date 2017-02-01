using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Vikekh.Stepbot.Common
{
    public static class Utils
    {
        public static string GetVersionString()
        {
            var version = Assembly.GetExecutingAssembly().GetName().Version;
            return string.Format("{0}.{1}.{2}", version.Major, version.Minor, version.Build);
        }

        public static string[] ParseMessage(string message)
        {
            return message.Split(' ')
                .Where(s => !string.IsNullOrEmpty(s))
                .ToArray();
        }
    }
}
