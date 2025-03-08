# Models
Definitions for 3D models and the [meshes](https://github.com/simulation-tree/meshes) that they contain.

### Importing models
When models are imported, they will contain a list of meshes, each with their own data:
```cs
using World world = new();
Model model = new(world, "*/model.fbx");
while (!model.Is())
{
    world.Submit(new DataUpdate()); //to load the bytes
    world.Submit(new ModelUpdate()); //load import the model from the bytes
    world.Poll();
}

//after the model is loaded
uint meshCount = model.MeshCount;
for (uint i = 0; i < meshCount; i++)
{
    Mesh mesh = model[i];
    ReadOnlySpan<Vector3> vertices = mesh.Positions.AsSpan();
    ReadOnlySpan<uint> indices = mesh.Indices.AsSpan();
}

```
