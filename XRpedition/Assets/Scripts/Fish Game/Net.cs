using Unity.VisualScripting;
using UnityEngine;

public class Net : MonoBehaviour
{
    private FishMovement FishScript;
    
    private AudioSource audioSource;

    [SerializeField] private AudioClip NetSwingSound;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }
    
    private void OnTriggerEnter(Collider other)
    {
        FishScript = other.GetComponent<FishMovement>();
        if (FishScript != null)
        {
            FishScript.Caught();
        }
    }
}
