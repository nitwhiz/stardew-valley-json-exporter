using System;
using System.Text.RegularExpressions;
using Newtonsoft.Json;

namespace JsonExporter.data.wrapped
{
    [JsonObject(MemberSerialization.OptIn)]
    public abstract class WrappedInstance<T>
    {
        [JsonProperty("id")] public readonly string Id;
        
        public readonly T Original;

        protected WrappedInstance(T original)
        {
            Id = Guid.NewGuid().ToString();
            
            Original = original;
        }

        public static string Normalize(string str)
        {
            var nonAlphaNumericRegex = new Regex("[^A-Za-z0-9_]");
            var doubleHyphenRegex = new Regex("-{2,}");

            return doubleHyphenRegex.Replace(nonAlphaNumericRegex.Replace(str, "-"), "-").ToLower();
        }
    }
}