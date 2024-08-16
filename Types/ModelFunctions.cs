using Meshes;
using Models;
using System;

public static class ModelFunctions
{
    public static uint GetMeshCount<T>(this T model) where T : unmanaged, IModel
    {
        return model.GetList<T, ModelMesh>().Count;
    }

    public static Mesh GetMesh<T>(this T model, uint index) where T : unmanaged, IModel
    {
        ReadOnlySpan<ModelMesh> meshList = model.GetList<T, ModelMesh>().AsSpan();
        ModelMesh mesh = meshList[(int)index];
        return new(model.World, mesh.value);
    }
}