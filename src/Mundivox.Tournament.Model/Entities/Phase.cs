using System.Collections.Generic;

namespace Mundivox.Tournament.Model
{
    public class Phase
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<Key> Keys { get; set; } = new List<Key>();
    }
}
