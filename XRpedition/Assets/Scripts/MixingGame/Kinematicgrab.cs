using Oculus.Interaction;
using UnityEngine;

public class ForceKinematicOnGrab_Poll : MonoBehaviour
{
    private Rigidbody rb;
    private Grabbable grabbable;
    private bool wasGrabbed;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        grabbable = GetComponent<Grabbable>();
    }

    private void Update()
    {
        bool nowGrabbed = grabbable.SelectingPointsCount > 0;

        if (nowGrabbed && !wasGrabbed)
        {
            // Grab started
            rb.isKinematic = true;
            rb.useGravity = false;
        }
        else if (!nowGrabbed && wasGrabbed)
        {
            // Grab ended
            rb.isKinematic = false;
            rb.useGravity = true;
        }

        wasGrabbed = nowGrabbed;
    }
}