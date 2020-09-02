using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Newtonsoft.Json;

namespace GaslandsHQ.Core.Data
{
    public class LookupRepository
    {
        public T GetData<T>()
        {
            var name = typeof(T).Name;

            var json = GetJson(name);

            return JsonConvert.DeserializeObject<T>(json);
        }

        public IEnumerable<T> GetValues<T>()
        {
            var name = typeof(T).Name;

            var json = GetJson(name);

            return JsonConvert.DeserializeObject<IEnumerable<T>>(json);
        }

        string GetJson(string name)
        {
            var assembly = typeof(LookupRepository).Assembly;;
            var resourceName = $"{name}.json";

            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            using (StreamReader reader = new StreamReader(stream))
            {
                string result = reader.ReadToEnd();

                return result;
            }
        }
    }
}
