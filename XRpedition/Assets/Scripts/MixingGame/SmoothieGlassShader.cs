using System.Collections;
using UnityEngine;

public class SmoothieGlassShader : MonoBehaviour
{
    [SerializeField] private Renderer _renderer;
    [SerializeField] private Material material;
    private float matfill = 1;

    private void Start()
    {
        if (_renderer == null)
        {
            material = _renderer.material;
            material.SetFloat("_Fill", matfill);
        }
    }

    private void Update()
    {
        if (material == null)
        {
            material = _renderer.material;
        }
    }

    public void FillGlass()
    {
        StartCoroutine(SmoothFill(1f, 1.2f));
    }

    private IEnumerator SmoothFill(float targetfill, float duration)
    {
        float startfill = matfill;
        float time = 0f;

        while (time < duration)
        {
            time +=  Time.deltaTime;
            matfill = Mathf.Lerp(startfill, targetfill, time / duration);

            if (material != null)
            {
                material.SetFloat("_Fill", matfill);
            }
            
            yield return null;
        }
    }
}
