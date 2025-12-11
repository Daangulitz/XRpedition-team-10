using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;

public class ResetGame : MonoBehaviour
{
    private Volume volume;
    private ColorLookup colorLookup;
    private OVRPassthroughLayer passthroughLayer;
    
    void Start()
    {
        volume = FindObjectOfType<Volume>();
        passthroughLayer = FindObjectOfType<OVRPassthroughLayer>();
        
        
        if (!volume.profile.TryGet<ColorLookup>(out colorLookup))
        {
            Debug.LogWarning("No ColorLookup effect found in the Volume profile!");
        }

        StartCoroutine(Resetgame(4f));
    }
    
    private IEnumerator Resetgame(float time)
    {
        Texture2D activeLUT = (Texture2D)colorLookup.texture.value;
        float startContribution = colorLookup.contribution.value;

        float elapsed = 0f;

        while (elapsed < time)
        {
            elapsed += Time.deltaTime;
            float pct = elapsed / time;
            
            colorLookup.contribution.value = Mathf.Lerp(startContribution, 0f, pct);
            colorLookup.texture.value = activeLUT;

            float w = Mathf.Lerp(startContribution, 0f, pct);
            passthroughLayer.SetColorLut(new OVRPassthroughColorLut(activeLUT, flipY: false), w);

            yield return null;
        }
        
        colorLookup.contribution.value = 0f;
        passthroughLayer.SetColorLut(new OVRPassthroughColorLut(activeLUT, flipY: false), 0f);

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

}
