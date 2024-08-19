using Data.Components;
using Meshes;
using Models.Components;
using Simulation;
using System;
using Unmanaged;

namespace Models
{
    public readonly struct Model : IModel, IDisposable
    {
        private readonly Entity entity;

        public readonly bool IsLoaded => entity.ContainsComponent<IsModel>();
        public readonly uint MeshCount => entity.GetList<ModelMesh>().Count;
        public readonly Mesh this[uint index]
        {
            get
            {
                ModelMesh mesh = entity.GetListElement<ModelMesh>(index);
                eint meshEntity = entity.GetReference(mesh.value);
                return new(entity.world, meshEntity);
            }
        }

        World IEntity.World => entity.world;
        eint IEntity.Value => entity.value;

#if NET
        [Obsolete("Default constructor not available", true)]
        public Model()
        {
            throw new NotImplementedException();
        }
#endif

        public Model(World world, eint existingEntity)
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

        public static implicit operator Entity(Model model)
        {
            return model.entity;
        }
    }
}
