using Meshes;
using Models.Components;
using System;
using Unmanaged;
using Worlds;

namespace Models
{
    public readonly partial struct Model : IEntity
    {
        public readonly int MeshCount => GetArrayLength<ModelMesh>();

        public readonly Mesh this[int index]
        {
            get
            {
                ModelMesh mesh = GetArrayElement<ModelMesh>(index);
                uint meshEntity = GetReference(mesh.value);
                return new Entity(world, meshEntity).As<Mesh>();
            }
        }

        /// <summary>
        /// Creates a request to load a model from the given <paramref name="address"/>.
        /// </summary>
        public Model(World world, ASCIIText256 address, double timeout = default)
        {
            ASCIIText256 extension = default;
            if (address.TryLastIndexOf('.', out int extensionIndex))
            {
                extension = address.Slice(extensionIndex + 1);
            }

            this.world = world;
            value = world.CreateEntity(new IsModelRequest(extension, address, timeout));
        }

        /// <summary>
        /// Creates a model with the given <paramref name="meshes"/>.
        /// </summary>
        public Model(World world, Span<Mesh> meshes)
        {
            this.world = world;
            value = world.CreateEntity(new IsModel());
            Span<ModelMesh> array = CreateArray<ModelMesh>(meshes.Length);
            for (int i = 0; i < meshes.Length; i++)
            {
                array[i] = new(AddReference(meshes[i]));
            }
        }

        readonly void IEntity.Describe(ref Archetype archetype)
        {
            archetype.AddComponentType<IsModel>();
            archetype.AddArrayType<ModelMesh>();
        }


        public readonly override string ToString()
        {
            return value.ToString();
        }

        public readonly ASCIIText256 GetName(Mesh mesh)
        {
            return mesh.GetComponent<ModelName>().value;
        }
    }
}