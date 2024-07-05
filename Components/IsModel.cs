namespace Models.Components
{
    public struct IsModel
    {
        /// <summary>
        /// Indicates that the undlerlying <see cref="byte"/> collection has
        /// updated.
        /// </summary>
        public bool changed;

        public IsModel()
        {
            changed = true;
        }

        public IsModel(bool changed)
        {
            this.changed = changed;
        }
    }
}
