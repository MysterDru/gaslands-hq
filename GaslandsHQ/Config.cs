using System;
using System.IO;
using System.Reflection;
using Newtonsoft.Json.Linq;

namespace GaslandsHQ
{
    public class Config
    {
        public static string AppCenterAndroidKey => GetJson()["AppCenter_Android_Secret"]?.ToString();

        private static JObject GetJson()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = "GaslandsHQ.secrets.json";

            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            using (StreamReader reader = new StreamReader(stream))
            {
                string result = reader.ReadToEnd();

                return JObject.Parse(result);
            }
        }
    }
}
