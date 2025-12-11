using UnityEngine;
using UnityEngine.Splines;

public class AnimationOnSpline : MonoBehaviour
{
    public SplineAnimate splineAnimate;
    private bool hasPaused = false;
    private bool hasResumed = false;
    private FruitOrderManager fruitOrderManager;
    private Animator animator;
    private Humenextations human;

    private void Start()
    {
        fruitOrderManager = FindFirstObjectByType<FruitOrderManager>();
        human = FindObjectOfType<Humenextations>();
        animator = GetComponent<Animator>();
        animator.SetBool("IsWalking", true);
    }

    private void Update()
    {
        if (splineAnimate.NormalizedTime <= 0.1f)
        {
            hasPaused = false;
            hasResumed = false;
            human.SetActiveFalse();
            human.SetHumanThings();
        }
        
        if (!hasPaused && splineAnimate.NormalizedTime >= 0.5f)
        {
            splineAnimate.Pause();
            hasPaused = true;
            fruitOrderManager.NewOrder();
            animator.SetBool("IsWalking", false);
        }
    }

    public void ResumeAnimation()
    {
        if (hasPaused && !hasResumed)
        {
            splineAnimate.Play();
            hasResumed = true;
            animator.SetBool("IsWalking", true);
        }
    }
}