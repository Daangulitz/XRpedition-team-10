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
    
    private void Start()
    {
        passthroughLayer = FindObjectOfType<OVRPassthroughLayer>();
    }

    public void NoRedLut()
    {
        var Nored = new OVRPassthroughColorLut(NoRed, flipY: false);
        passthroughLayer.SetColorLut(Nored, LutWeight);
    }

    public void NoGreenLut()
    {
        var Nogreen = new OVRPassthroughColorLut(NoGreen, flipY: false);
        passthroughLayer.SetColorLut(Nogreen, LutWeight);
    }

    public void NoBlueLut()
    {
        var Noblue = new OVRPassthroughColorLut(NoBlue, flipY: false);
        passthroughLayer.SetColorLut(Noblue, LutWeight);
    }

    public void BlackWhiteLut()
    {
        passthroughLayer.DisableColorMap();
        
        passthroughLayer.SetBrightnessContrastSaturation(
            brightness: 0f,
            contrast: 0f,
            saturation: -1f 
        );
    }
}
