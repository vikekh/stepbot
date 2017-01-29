using System.IO;
using Vikekh.Stepbot.Interfaces;

namespace Vikekh.Stepbot.Common
{
    public static class Config
    {
        public static string FindConfigFilePath()
        {
            if (File.Exists("config.json")) return "config.json";

            throw new FileNotFoundException();
        }

        public static T GetConfig<T>() where T : IConfig
        {
            var path = FindConfigFilePath();
            return Json.Read<T>(path);
        }
    }
}
