using UnityEngine;
using Unity.AI.Navigation;

public class NavMeshSurfaceCreator : MonoBehaviour
{

    [SerializeField] private NavMeshSurface NMSurface;
    
    void Update()
    {
        if (NMSurface == null)
        {
            NMSurface.GetComponent<NavMeshSurface>();
        }
        NMSurface.BuildNavMesh();
    }
}
