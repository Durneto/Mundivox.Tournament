using System;

namespace Mundivox.Tournament.Model
{
    public class Key : ICloneable
    {
        public int Id { get; set; }
        public Team Team_A { get; set; }
        public Team Team_B { get; set; }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}