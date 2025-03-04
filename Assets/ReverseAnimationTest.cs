using UnityEngine;

public class ReverseAnimationTest : MonoBehaviour
{

    private Animator animator;


    [SerializeField] private CameraShakeWithObject cameraShakeWithObject;

    private int currentSpeed = 0;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = GetComponent<Animator>();

        // Store the original position of the object


    }

    // Update is called once per frame
    void Update()
    {

        
      
            
            if (Input.GetMouseButton(0) && animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1 && cameraShakeWithObject.shakeObject != null && cameraShakeWithObject.canStartShaking)
            {
                animator.SetFloat("AnimationSpeedMultiplier", 1f);
                Debug.Log("lets go");
            }
            else if ((Input.GetMouseButtonUp(0) || cameraShakeWithObject.shakeObject == null) && animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0)
            {
                animator.SetFloat("AnimationSpeedMultiplier", -1f);
            }
            else if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1)
            {
                animator.SetFloat("AnimationSpeedMultiplier", 0f);
                
                
            }
            else if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime <= 0)
            {
                animator.SetFloat("AnimationSpeedMultiplier", 0f);
                
            }
        



    }


}
