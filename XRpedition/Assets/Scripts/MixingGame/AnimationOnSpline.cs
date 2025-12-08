using UnityEngine;
using UnityEngine.Splines;

public class AnimationOnSpline : MonoBehaviour
{
    public SplineAnimate splineAnimate;
    private static bool pauseTriggered = false;
    private FruitOrderManager fruitOrderManager;
    private Animator animator;

    private void Start()
    {
        fruitOrderManager = FindFirstObjectByType<FruitOrderManager>();
        animator = GetComponent<Animator>();
    }
    
    private void Update()
    {
        if (!pauseTriggered && splineAnimate.NormalizedTime >= 0.5f)
        {
            splineAnimate.Pause();
            pauseTriggered = true;
            fruitOrderManager.NewOrder();
            animator.SetBool("IsWalking" , false);
        }

        if (!pauseTriggered)
        {
            splineAnimate.Play();
            animator.SetBool("IsWalking" , true);
        }
    }

    public static void ResumeAnimation()
    {
        if (pauseTriggered)
        {
            pauseTriggered = false;
        }
    }
}