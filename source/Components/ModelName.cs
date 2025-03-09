using System;
using Unmanaged;

namespace Models.Components
{
    public readonly struct ModelName
    {
        public readonly ASCIIText256 value;

        public ModelName(Span<char> value)
        {
            this.value = new(value);
        }

        public ModelName(ASCIIText256 value)
        {
            this.value = value;
        }

        public ModelName(string value)
        {
            this.value = new(value);
        }
    }
}
