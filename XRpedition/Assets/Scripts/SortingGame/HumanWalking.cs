using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;

public class RandomWalker : MonoBehaviour
{
    public float maxTimeBetweenMoves = 5f;
    public float walkRadius = 10f;

    public Transform player;
    public float maxDistanceFromPlayer = 15f;

    private NavMeshAgent agent;
    private NavMeshSurface surface;
    private float timer;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        surface = FindAnyObjectByType<NavMeshSurface>();

        player = GameObject.FindGameObjectWithTag("Player").transform;
        
        if (surface == null)
        {
            Debug.LogError("No NavMeshSurface found in the scene!");
            return;
        }

        if (player == null)
        {
            Debug.LogError("No player assigned to RandomWalker!");
            enabled = false;
            return;
        }

        // Auto-calc walking radius from navmesh surface
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

        if (reached || timer >= maxTimeBetweenMoves)
        {
            PickNewDestination();
            timer = 0f;
        }
    }

    void PickNewDestination()
    {
        // Try multiple times to get a valid destination
        for (int i = 0; i < 20; i++)
        {
            Vector3 randomDirection = Random.insideUnitSphere * walkRadius;

            // Center the wander around the PLAYER, not the AI
            randomDirection += player.position;

            NavMeshHit hit;
            if (NavMesh.SamplePosition(randomDirection, out hit, walkRadius, NavMesh.AllAreas))
            {
                // Check if position is not too far from the player
                float dist = Vector3.Distance(hit.position, player.position);

                if (dist <= maxDistanceFromPlayer)
                {
                    agent.SetDestination(hit.position);
                    return;
                }
            }
        }

        // Fallback: stay at current position
        agent.SetDestination(transform.position);
    }
}
