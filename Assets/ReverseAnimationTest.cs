using UnityEngine;

public class ReverseAnimationTest : MonoBehaviour
{

    private Animator animator;
    [SerializeField] private AnimationClip idleHand;
    [SerializeField] private AnimationClip reachInHand;
    [SerializeField] private AnimationClip reachInHandReverse;

    private int currentSpeed = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = GetComponent<Animator>();
        
    }

    // Update is called once per frame
    void Update()
    {

        if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1 || animator.GetCurrentAnimatorStateInfo(0).normalizedTime <= 0)
        {
            animator.SetFloat("AnimationSpeedMultiplier", 0f);
        }

        if (Input.GetMouseButtonDown(0))
        {
            animator.SetFloat("AnimationSpeedMultiplier", 1f);
        }
        else if (Input.GetMouseButtonUp(0))
        {
            animator.SetFloat("AnimationSpeedMultiplier", -1f);
        }

 
    }
}
