using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.IO;

namespace Vikekh.Stepbot.Common
{
    public static class Json
    {
        private static JsonSerializerSettings GetSettings()
        {
            return new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };
        }

        public static T Read<T>(string path)
        {
            var value = File.ReadAllText(path);
            return JsonConvert.DeserializeObject<T>(value, GetSettings());
        }

        public static void Write(string path, object value)
        {
            var contents = JsonConvert.SerializeObject(value, Formatting.Indented, GetSettings());
            File.WriteAllText(path, contents);
        }
    }
}
