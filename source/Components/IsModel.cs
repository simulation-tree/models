using Worlds;

namespace Models.Components
{
    [Component]
    public struct IsModel
    {
        public uint version;

        public IsModel(uint version)
        {
            this.version = version;
        }
    }
}
