using UnityEngine;

public class PassThroughMaterialLink : MonoBehaviour
{
    public OVRPassthroughLayer passthroughLayer;
    public Material cvdMaterial;

    void Start()
    {
        if (passthroughLayer != null && cvdMaterial != null)
        {
            // Assign your shader material to the passthrough layer
            //passthroughLayer.customMaterial = cvdMaterial;
        }
    }
}
