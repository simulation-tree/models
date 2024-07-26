using Meshes;
using Models;

public static class ModelFunctions
{
    public static uint GetMeshCount<T>(this T model) where T : unmanaged, IModel
    {
        return model.GetList<T, ModelMesh>().Count;
    }

    public static Mesh GetMesh<T>(this T model, uint index) where T : unmanaged, IModel
    {
        ModelMesh mesh = model.GetList<T, ModelMesh>()[index];
        return new(model.World, mesh.value);
    }
}