namespace Models.Components
{
    public readonly struct IsModel
    {
        public readonly uint version;

        public IsModel(uint version)
        {
            this.version = version;
        }

        public readonly IsModel IncrementVersion()
        {
            return new IsModel(version + 1);
        }
    }
}