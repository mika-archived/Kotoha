using System.Runtime.Serialization;

using Kotoha.Plugin;

namespace Kotoha.Impl
{
    public class KotohaTalker : IKotohaTalker
    {
        [DataMember(Name = "id")]
        public string Id { get; set; }

        [DataMember(Name = "name")]
        public string Name { get; set; }

        [DataMember(Name = "engine")]
        public string Engine { get; set; }
    }
}