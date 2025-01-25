using Worlds;

namespace Models.Tests
{
    public class ModelEntityTests : ModelTests
    {
        [Test]
        public void VerifyModelsAreModels()
        {
            using World world = CreateWorld();
            Model model = new(world, []);
            Assert.That(model.Is(), Is.True);
        }
    }
}