using System;
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
    }
}