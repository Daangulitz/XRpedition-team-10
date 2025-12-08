using Oculus.Interaction;
using UnityEngine;
using UnityEngine.AI;

public class AIGrabHandler_Poll : MonoBehaviour
{
    private NavMeshAgent agent;
    private Grabbable grabbable;
    private bool wasGrabbed;
    private float groundY;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        grabbable = GetComponent<Grabbable>();

        if (agent == null)
            Debug.LogError("AIGrabHandler_Poll requires a NavMeshAgent on the same object!");
        if (grabbable == null)
            Debug.LogError("AIGrabHandler_Poll requires a Grabbable component!");
    }

    private void Update()
    {
        bool nowGrabbed = grabbable.SelectingPointsCount > 0;

        if (nowGrabbed && !wasGrabbed)
        {
            // Grab started
            wasGrabbed = true;

            // Store current Y position
            groundY = transform.position.y;

            // Disable NavMeshAgent
            if (agent.enabled)
            {
                agent.ResetPath();
                agent.enabled = false;
            }
        }
        else if (!nowGrabbed && wasGrabbed)
        {
            // Grab ended
            wasGrabbed = false;

            // Re-enable NavMeshAgent
            if (!agent.enabled)
            {
                agent.enabled = true;
                agent.Warp(transform.position); // snap to current position
            }
        }

        // While grabbed, lock Y-axis
        if (wasGrabbed)
        {
            Vector3 pos = transform.position;
            pos.y = groundY;
            transform.position = pos;
        }
    }
}