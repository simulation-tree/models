using Data.Components;
using Data.Events;
using Meshes;
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
            entity.CreateList<ModelMesh>();

            world.Submit(new DataUpdate());
            world.Submit(new ModelUpdate());
            world.Poll();
        }

        public Model(World world, FixedString address)
        {
            entity = new(world);
            entity.AddComponent(new IsDataRequest(address));
            entity.AddComponent(new IsModel());
            entity.CreateList<ModelMesh>();

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

        public readonly uint GetMeshCount()
        {
            return entity.GetList<ModelMesh>().Count;
        }

        public readonly Mesh GetMesh(uint index)
        {
            ReadOnlySpan<ModelMesh> meshList = entity.GetList<ModelMesh>().AsSpan();
            ModelMesh mesh = meshList[(int)index];
            return new(entity.world, mesh.value);
        }

        public static implicit operator Entity(Model model)
        {
            return model.entity;
        }
    }
}
