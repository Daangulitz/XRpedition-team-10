using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class CamFilterChange : MonoBehaviour
{
    private OVRPassthroughLayer passthroughLayer;
    [SerializeField] private Volume volume;
    [Header("ColorBlindFilters")]
    [Header("Protanopia")]
    [SerializeField] private Texture2D NoRed;
    [Header("Deuteranopia")]
    [SerializeField] private Texture2D NoGreen;
    [Header("Tritanopia")]
    [SerializeField] private Texture2D NoBlue;
    [Header("Achromatopsie")]
    [SerializeField] private Texture2D NoColor;
    
    [SerializeField] private float LutWeight;
    private ColorLookup colorLookup;

    private void Awake()
    {
        if (volume == null)
        {
            volume = FindObjectOfType<Volume>();
        }

        if (volume != null && volume.profile != null)
        {
            if (!volume.profile.TryGet<ColorLookup>(out colorLookup))
            {
                Debug.LogWarning("No ColorLookup found");
            }
        }
    }
    
    private void Update()
    {
        if (passthroughLayer == null)
        {
            passthroughLayer = FindObjectOfType<OVRPassthroughLayer>();
        }
    }


    public void NoRedLut()
    {
        var Nored = new OVRPassthroughColorLut(NoRed, flipY: false);
        passthroughLayer.SetColorLut(Nored, LutWeight);
        SetLUT(NoRed);
    }

    public void NoGreenLut()
    {
        var Nogreen = new OVRPassthroughColorLut(NoGreen, flipY: false);
        passthroughLayer.SetColorLut(Nogreen, LutWeight);
        SetLUT(NoGreen);
    }

    public void NoBlueLut()
    {
        var Noblue = new OVRPassthroughColorLut(NoBlue, flipY: false);
        passthroughLayer.SetColorLut(Noblue, LutWeight);
        SetLUT(NoBlue);
    }

    public void BlackWhiteLut()
    {
        var Nocolor = new OVRPassthroughColorLut(NoColor, flipY: false);
        passthroughLayer.SetColorLut(Nocolor, LutWeight);
        SetLUT(NoColor);
    }
    private void SetLUT(Texture2D lut)
    {
        if (colorLookup == null)
            return;

        colorLookup.texture.overrideState = true;
        colorLookup.texture.value = lut;

        colorLookup.contribution.overrideState = true;
        colorLookup.contribution.value = LutWeight;
    }
}
