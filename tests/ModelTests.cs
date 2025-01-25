using Types;
using Worlds;
using Worlds.Tests;

namespace Models.Tests
{
    public abstract class ModelTests : WorldTests
    {
        static ModelTests()
        {
            TypeRegistry.Load<Models.TypeBank>();
        }

        protected override Schema CreateSchema()
        {
            Schema schema = base.CreateSchema();
            schema.Load<Models.SchemaBank>();
            return schema;
        }
    }
}