using UnityEngine;

public class SlapLogic : MonoBehaviour
{

    [SerializeField] private RotateObject rotateObject;

    private Animator animator;

    private PlaySound playSound;

    private void Start()
    {
        playSound = GetComponent<PlaySound>();
        animator = GetComponent<Animator>();
    }

    public void ActivateSlapAnimation()
    {
        animator.SetTrigger("Slap");
    }

    public void Slap()
    {
        rotateObject.Rotate360();
        playSound.SoundDo();
    }
}
