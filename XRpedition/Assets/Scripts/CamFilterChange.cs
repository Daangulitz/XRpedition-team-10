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

    [SerializeField] private float LutWeight = 1f;

    [SerializeField] private ColorLookup colorLookup;

    private void Awake()
    {
        // Automatically find the Volume if not assigned
        if (volume == null)
        {
            volume = FindObjectOfType<Volume>();
            if (volume == null)
            {
                Debug.LogWarning("No Volume found in the scene!");
            }
        }

        // Get the ColorLookup effect from the Volume profile
        if (volume != null && volume.profile != null)
        {
            if (!volume.profile.TryGet<ColorLookup>(out colorLookup))
            {
                Debug.LogWarning("No ColorLookup effect found in the Volume profile!");
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
        if (passthroughLayer != null)
        {
            var Nored = new OVRPassthroughColorLut(NoRed, flipY: false);
            passthroughLayer.SetColorLut(Nored, LutWeight);
        }
        SetLUT(NoRed);
    }

    public void NoGreenLut()
    {
        if (passthroughLayer != null)
        {
            var Nogreen = new OVRPassthroughColorLut(NoGreen, flipY: false);
            passthroughLayer.SetColorLut(Nogreen, LutWeight);
        }
        SetLUT(NoGreen);
    }

    public void NoBlueLut()
    {
        if (passthroughLayer != null)
        {
            var Noblue = new OVRPassthroughColorLut(NoBlue, flipY: false);
            passthroughLayer.SetColorLut(Noblue, LutWeight);
        }
        SetLUT(NoBlue);
    }

    public void BlackWhiteLut()
    {
        if (passthroughLayer != null)
        {
            var Nocolor = new OVRPassthroughColorLut(NoColor, flipY: false);
            passthroughLayer.SetColorLut(Nocolor, LutWeight);
        }
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
