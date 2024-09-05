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
        public readonly Entity entity;

        public readonly uint MeshCount => entity.GetArrayLength<ModelMesh>();
        public readonly Mesh this[uint index]
        {
            get
            {
                ModelMesh mesh = entity.GetArrayElementRef<ModelMesh>(index);
                uint meshEntity = entity.GetReference(mesh.value);
                return new Entity(entity.world, meshEntity).As<Mesh>();
            }
        }

        readonly World IEntity.World => entity.world;
        readonly uint IEntity.Value => entity.value;
        readonly Definition IEntity.Definition => new([RuntimeType.Get<IsModel>()], [RuntimeType.Get<ModelMesh>()]);

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

        public Model(World world, USpan<char> address)
        {
            entity = new(world);
            entity.AddComponent(new IsDataRequest(address));
            entity.AddComponent(new IsModelRequest());
        }

        public Model(World world, string address)
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
    }
}
