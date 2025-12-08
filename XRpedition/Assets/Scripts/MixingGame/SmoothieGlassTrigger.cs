using UnityEngine;

public class SmoothieGlassTrigger : MonoBehaviour
{
    private Blender blender;

    private void Start()
    {
        blender = FindFirstObjectByType<Blender>();
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("SmoothieGlas"))
        {
            AnimationOnSpline.ResumeAnimation();
        }
    }
}
