using System.Collections;
using UnityEngine;

public class CamFilterChange : MonoBehaviour
{
    private OVRPassthroughLayer passthroughLayer;
    [Header("ColorBlindFilters")]
    [Header("Protanopia")]
    [SerializeField] private Texture2D NoRed;
    [Header("Deuteranopia")]
    [SerializeField] private Texture2D NoGreen;
    [Header("Tritanopia")]
    [SerializeField] private Texture2D NoBlue;
    
    [SerializeField] private float LutWeight;

    private bool IsRunning = false;

    private void Start()
    {
        passthroughLayer = FindObjectOfType<OVRPassthroughLayer>();
    }

    private void Update()
    {
        if (!IsRunning)
        {
            StartCoroutine(SwapColorLut());
        }
    }

    IEnumerator SwapColorLut()
    {
        IsRunning = true;

        var Nored = new OVRPassthroughColorLut(NoRed, flipY: false);
        passthroughLayer.SetColorLut(Nored, LutWeight);
        yield return new WaitForSeconds(10f);

        var Nogreen = new OVRPassthroughColorLut(NoGreen, flipY: false);
        passthroughLayer.SetColorLut(Nogreen, LutWeight);
        yield return new WaitForSeconds(10f);

        var Noblue = new OVRPassthroughColorLut(NoBlue, flipY: false);
        passthroughLayer.SetColorLut(Noblue, LutWeight);
        yield return new WaitForSeconds(10f);

        passthroughLayer.DisableColorMap();
        
        passthroughLayer.SetBrightnessContrastSaturation(
            brightness: 0f,
            contrast: 0f,
            saturation: -1f 
        );
        yield return new WaitForSeconds(10f);
        IsRunning = false;
        yield return new WaitForSeconds(0.01f);
        
    }
}
