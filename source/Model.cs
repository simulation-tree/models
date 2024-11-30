using Data.Components;
using Meshes;
using Models.Components;
using System;
using Unmanaged;
using Worlds;

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
                return new Entity(entity.world, meshEntity).As<Mesh>();
            }
        }

        readonly World IEntity.World => entity.world;
        readonly uint IEntity.Value => entity.value;
        readonly Definition IEntity.Definition => new Definition().AddComponentType<IsModel>().AddArrayType<ModelMesh>();

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

            if (address.TryLastIndexOf('.', out uint extensionIndex))
            {
                USpan<char> extension = address.Slice(extensionIndex + 1);
                entity.AddComponent(new IsModelRequest(extension));
            }
            else
            {
                entity.AddComponent(new IsModelRequest([]));
            }
        }

        public Model(World world, string address)
        {
            entity = new(world);
            entity.AddComponent(new IsDataRequest(address));

            int extensionIndex = address.LastIndexOf('.');
            if (extensionIndex != -1)
            {
                ReadOnlySpan<char> extension = address.AsSpan().Slice(extensionIndex + 1);
                entity.AddComponent(new IsModelRequest(extension));
            }
            else
            {
                entity.AddComponent(new IsModelRequest(""));
            }
        }

        public Model(World world, FixedString address)
        {
            entity = new(world);
            entity.AddComponent(new IsDataRequest(address));

            if (address.TryLastIndexOf('.', out uint extensionIndex))
            {
                FixedString extension = address.Slice(extensionIndex + 1);
                entity.AddComponent(new IsModelRequest(extension));
            }
            else
            {
                entity.AddComponent(new IsModelRequest([]));
            }
        }

        public readonly void Dispose()
        {
            entity.Dispose();
        }

        public readonly override string ToString()
        {
            return entity.ToString();
        }

        public static implicit operator Entity(Model model)
        {
            return model.entity;
        }
    }
}
