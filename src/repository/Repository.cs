using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace JsonExporter.repository
{
    [JsonObject(MemberSerialization.OptIn)]
    public abstract class Repository<T, I> where T : Repository<T, I>, new()
    {
        private static T _instance;

        public static T GetInstance()
        {
            if (_instance == null)
            {
                _instance = new T();
                _instance.Populate();
            }

            return _instance;
        }

        public abstract void Populate();

        public abstract List<I> GetAll();

        public void ExportJson(string baseDir, string fileName)
        {
            var jsonFileName = Path.Combine(baseDir, fileName + ".json");
            
            File.WriteAllText(jsonFileName, JsonConvert.SerializeObject(this));
        }
    }
}