using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;

public class RandomWalker : MonoBehaviour
{
    public float maxTimeBetweenMoves = 5f;   
    public float walkRadius = 0f;           

    private NavMeshAgent agent;
    private NavMeshSurface surface;
    private float timer;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        
        surface = FindAnyObjectByType<NavMeshSurface>();

        if (surface == null)
        {
            Debug.LogError("No NavMeshSurface found in the scene!");
            return;
        }

        // Auto-calc walking radius from surface bounds
        Bounds b = surface.navMeshData.sourceBounds;
        walkRadius = Mathf.Max(b.extents.x, b.extents.z);

        PickNewDestination();
    }

    void Update()
    {
        timer += Time.deltaTime;

        bool reached =
            !agent.pathPending &&
            agent.remainingDistance <= agent.stoppingDistance;

        // If reached OR time expired → new target
        if (reached || timer >= maxTimeBetweenMoves)
        {
            PickNewDestination();
            timer = 0f;
        }
    }

    void PickNewDestination()
    {
        Vector3 randomDirection = Random.insideUnitSphere * walkRadius;
        randomDirection += transform.position;

        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomDirection, out hit, walkRadius, NavMesh.AllAreas))
        {
            agent.SetDestination(hit.position);
        }
    }
}