using Data.Components;
using Meshes;
using Models.Components;
using Simulation;
using System;
using Unmanaged;

namespace Models
{
    public readonly struct Model : IEntity
    {
        private readonly Entity entity;

        public readonly uint MeshCount => entity.GetArrayLength<ModelMesh>();
        public readonly Mesh this[uint index]
        {
            get
            {
                ModelMesh mesh = entity.GetArrayElementRef<ModelMesh>(index);
                uint meshEntity = entity.GetReference(mesh.value);
                return new Entity(entity, meshEntity).As<Mesh>();
            }
        }

        World IEntity.World => entity;
        uint IEntity.Value => entity;

#if NET
        [Obsolete("Default constructor not available", true)]
        public Model()
        {
            throw new NotImplementedException();
        }
#endif

        public Model(World world, uint existingEntity)
        {
            entity = new(world, existingEntity);
        }

        public Model(World world, ReadOnlySpan<char> address)
        {
            entity = new(world);
            entity.AddComponent(new IsDataRequest(address));
            entity.AddComponent(new IsModelRequest());
        }

        public Model(World world, FixedString address)
        {
            entity = new(world);
            entity.AddComponent(new IsDataRequest(address));
            entity.AddComponent(new IsModelRequest());
        }

        public readonly override string ToString()
        {
            return entity.ToString();
        }

        Query IEntity.GetQuery(World world)
        {
            return new(world, RuntimeType.Get<IsModel>());
        }

        public static implicit operator Entity(Model model)
        {
            return model.entity;
        }
    }
}
