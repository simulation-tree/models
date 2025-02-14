using Unmanaged;

namespace Models.Components
{
    public readonly struct ModelName
    {
        public readonly FixedString value;

        public ModelName(USpan<char> value)
        {
            this.value = new(value);
        }

        public ModelName(FixedString value)
        {
            this.value = value;
        }

        public ModelName(string value)
        {
            this.value = new(value);
        }
    }
}
