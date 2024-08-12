using Data.Components;
using Data.Events;
using Models.Components;
using Models.Events;
using Simulation;
using System;
using Unmanaged;

namespace Models
{
    public readonly struct Model : IModel, IDisposable
    {
        private readonly Entity entity;

        World IEntity.World => entity.world;
        eint IEntity.Value => entity.value;

        public Model()
        {
            throw new NotImplementedException();
        }

        public Model(World world, eint existingEntity)
        {
            entity = new(world, existingEntity);
        }

        public Model(World world, ReadOnlySpan<char> address)
        {
            entity = new(world);
            entity.AddComponent(new IsDataRequest(address));
            entity.AddComponent(new IsModel());
            entity.CreateList<Entity, ModelMesh>();

            world.Submit(new DataUpdate());
            world.Submit(new ModelUpdate());
            world.Poll();
        }

        public readonly void Dispose()
        {
            entity.Dispose();
        }

        public readonly override string ToString()
        {
            return entity.ToString();
        }
        
        Query IEntity.GetQuery(World world)
        {
            return new(world, RuntimeType.Get<IsModel>());
        }
    }
}
