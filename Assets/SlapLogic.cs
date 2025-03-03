using UnityEngine;

public class SlapLogic : MonoBehaviour
{

    [SerializeField] private RotateObject rotateObject;

    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void ActivateSlapAnimation()
    {
        animator.SetTrigger("Slap");
    }

    public void Slap()
    {
        rotateObject.Rotate360();
    }
}
