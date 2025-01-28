using Types;
using Worlds;
using Worlds.Tests;

namespace Models.Tests
{
    public abstract class ModelTests : WorldTests
    {
        static ModelTests()
        {
            TypeRegistry.Load<ModelsTypeBank>();
        }

        protected override Schema CreateSchema()
        {
            Schema schema = base.CreateSchema();
            schema.Load<ModelsSchemaBank>();
            return schema;
        }
    }
}