using System.Runtime.Serialization;

namespace Kotoha.Plugin.Impl
{
    public class KotohaTalker : IKotohaTalker
    {
        [DataMember(Name = "name")]
        public string Name { get; set; }

        [DataMember(Name = "engine")]
        public string Engine { get; set; }
    }
}