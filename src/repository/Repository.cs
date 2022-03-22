using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace JsonExporter.repository
{
    [JsonObject(MemberSerialization.OptIn)]
    public abstract class Repository<TRepository, TItem> where TRepository : Repository<TRepository, TItem>, new()
    {
        private static TRepository _instance;

        public static TRepository GetInstance()
        {
            if (_instance == null)
            {
                _instance = new TRepository();
                _instance.Populate();
            }

            return _instance;
        }

        public abstract void Populate();

        public abstract List<TItem> GetAll();

        public void ExportJson(string baseDir, string fileName)
        {
            var jsonFileName = Path.Combine(baseDir, fileName + ".json");
            
            File.WriteAllText(jsonFileName, JsonConvert.SerializeObject(this));
        }
    }
}