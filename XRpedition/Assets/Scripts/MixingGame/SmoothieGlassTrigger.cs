using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmoothieGlassTrigger : MonoBehaviour
{
    private Blender blender;
    private AnimationOnSpline AnimationOnSpline;
    [SerializeField] private Animator animator;
    [SerializeField] private GameObject Glass;

    private bool glassDeliveredThisRound = false;

    private void Start()
    {
        blender = FindFirstObjectByType<Blender>();
        AnimationOnSpline = FindFirstObjectByType<AnimationOnSpline>();
        Glass.SetActive(false);
        glassDeliveredThisRound = false;
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (glassDeliveredThisRound) return; 

        if (other.gameObject.CompareTag("SmoothieGlas"))
        {
            glassDeliveredThisRound = true;
            StartCoroutine(SmoothieGlassTriggerCoroutine());
        }
    }

    private IEnumerator SmoothieGlassTriggerCoroutine()
    {
        if (blender.IsCorrect)
        {
            animator.SetTrigger("Goed");
            blender.BlenderText.text = "Goed Gedaan";
        }
        else
        {
            animator.SetTrigger("Fout");
            blender.BlenderText.text = "Fout Gedaan";
        }
        
        Glass.SetActive(true);
        yield return new WaitForSeconds(2f);
        AnimationOnSpline.ResumeAnimation();
        blender.BlenderText.text = "Wacht op de volgende klant";
        yield return new WaitForSeconds(1.5f);
        Glass.SetActive(false);
    }
    
    public void ResetGlassTrigger()
    {
        glassDeliveredThisRound = false;
    }
}