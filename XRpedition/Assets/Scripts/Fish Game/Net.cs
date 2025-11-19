using Unity.VisualScripting;
using UnityEngine;

public class Net : MonoBehaviour
{
    private FishMovement FishScript;

    private void OnTriggerEnter(Collider other)
    {
        FishScript = other.GetComponent<FishMovement>();
        FishScript.Caught();
    }
}
