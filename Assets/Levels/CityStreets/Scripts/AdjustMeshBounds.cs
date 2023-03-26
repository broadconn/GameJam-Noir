using System.Collections.Generic;
using UnityEngine;

// The use of vertex shaders in this game means the bounds of meshes are often not aligned with the visual position of the mesh.
// The camera's frustum culling uses mesh bounds to decide when the mesh is offscreen and can be excluded. With vertex shaders, models may be culled too early.
// Use this script as a simple way to change how the camera perceives the position of the object (usually increasing the bounds to fit the vertex shader modifications).
public class AdjustMeshBounds : MonoBehaviour
{
    [SerializeField] private Vector3 boundsMultiplier = new (1, 1, 1); 
    [SerializeField] private bool includeChildren = false; 
    
    private void Start()
    {
        var meshFilters = new List<MeshFilter>();
        
        var mf = GetComponent<MeshFilter>();
        if(!mf.Equals(null))
            meshFilters.Add(mf);
        
        if(includeChildren)
            meshFilters.AddRange(GetComponentsInChildren<MeshFilter>());
        
        foreach (var meshFilter in meshFilters)
        {
            var mesh = meshFilter.mesh;
            var trSc = meshFilter.transform.localScale;
            mesh.bounds = new Bounds(
                center: Vector3.zero, 
                size: new Vector3(trSc.x * boundsMultiplier.x, trSc.y * boundsMultiplier.y, trSc.z * boundsMultiplier.z)
                );
        }
    }
}
