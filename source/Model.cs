using Data;
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
                ModelMesh mesh = entity.GetArrayElement<ModelMesh>(index);
                uint meshEntity = entity.GetReference(mesh.value);
                return new Entity(entity.world, meshEntity).As<Mesh>();
            }
        }

        readonly World IEntity.World => entity.world;
        readonly uint IEntity.Value => entity.value;

        readonly void IEntity.Describe(ref Archetype archetype)
        {
            archetype.AddComponentType<IsModel>();
            archetype.AddArrayElementType<ModelMesh>();
        }

#if NET
        [Obsolete("Default constructor not available", true)]
        public Model()
        {
            throw new NotImplementedException();
        }
#endif

        /// <summary>
        /// Creates a request to load a model from the given <paramref name="address"/>.
        /// </summary>
        public Model(World world, Address address)
        {
            FixedString extension = default;
            if (address.value.TryLastIndexOf('.', out uint extensionIndex))
            {
                extension = address.value.Slice(extensionIndex + 1);
            }

            entity = new Entity<IsDataRequest, IsModelRequest>(world, new(address), new(extension));
        }

        /// <summary>
        /// Creates a model with the given <paramref name="meshes"/>.
        /// </summary>
        public Model(World world, USpan<Mesh> meshes)
        {
            entity = new Entity<IsModel>(world);
            USpan<ModelMesh> array = entity.CreateArray<ModelMesh>(meshes.Length);
            for (uint i = 0; i < meshes.Length; i++)
            {
                array[i] = new(entity.AddReference(meshes[i]));
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