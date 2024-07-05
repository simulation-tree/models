using Data.Components;
using Data.Events;
using Meshes;
using Models.Components;
using Models.Events;
using Simulation;
using System;
using Unmanaged.Collections;

namespace Models
{
    public readonly struct Model : IDisposable
    {
        public readonly Entity entity;

        private readonly UnmanagedList<ModelMesh> meshes;

        public readonly bool IsDestroyed => entity.IsDestroyed;
        public readonly uint MeshCount => meshes.Count;
        public readonly Mesh this[uint index]
        {
            get
            {
                ModelMesh mesh = meshes[index];
                return new(entity.world, mesh.value);
            }
        }

        public Model()
        {
            throw new NotImplementedException();
        }

        public Model(World world, Entity existingEntity)
        {
            entity = new(world, existingEntity);
            meshes = entity.GetCollection<ModelMesh>();
        }

        public Model(World world, ReadOnlySpan<char> address)
        {
            entity = new(world);
            entity.AddComponent(new IsDataRequest(address));
            entity.AddComponent(new IsModel());
            meshes = entity.CreateCollection<ModelMesh>();

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
    }
}
