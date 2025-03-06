using UnityEngine;

public class EndSmashEvent : MonoBehaviour
{

    [SerializeField] private CameraShakeWithObject cameraShakeWithObject;
    [SerializeField] private GameObject bloodyScreen;



    public void EndShakeEvent()
    {
        bloodyScreen.SetActive(true);
        cameraShakeWithObject.endShakeEvent = true;
    }
}
