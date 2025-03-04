using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class MouseHoverDetector : MonoBehaviour
{
    [SerializeField] private float checkCooldown = 0.2f; // Cooldown time in seconds
    private float nextCheckTime = 0f; // Timer to track next allowed check

    private BulbManager bulbManager;

    private bool hasBulb = false;

    public bool hasButton = false;

    [SerializeField] private CameraShakeWithObject cameraShakeWithObject;   

    [SerializeField] private Image mouseUI;
    [SerializeField] private Sprite mouseClick;
    [SerializeField] private Sprite mouseHold;

    private void Start()
    {
        bulbManager = GetComponent<BulbManager>();
    }

    void Update()
    {
        if (Time.time >= nextCheckTime)
        {
            DetectHoveredObject();
            nextCheckTime = Time.time + checkCooldown; // Set next check time
        }
    }

    void DetectHoveredObject()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;



        if (Physics.Raycast(ray, out hit))
        {
            Bulb bulb = hit.collider.gameObject.GetComponent<Bulb>();

            if (bulb == null && hit.collider.gameObject.GetComponent<ButtonPress>() == null)
            {
                mouseUI.gameObject.SetActive(false);
            }

            if (hit.collider.gameObject.GetComponent<ButtonPress>() != null)
            {
                if(cameraShakeWithObject.canStartShaking)
                    mouseUI.gameObject.SetActive(true);

                mouseUI.sprite = mouseClick;
                hasButton = true;
            }
            else
            {
                hasButton = false;
            }

            if (bulb != null)
            {
                if (bulb.bulbIndex != bulbManager.bulbIndex)
                {
                    hasBulb = true;
                    bulbManager.SelectBulb(bulb.bulbIndex);
                }

                if (cameraShakeWithObject.canStartShaking)
                    mouseUI.gameObject.SetActive(true);
                mouseUI.sprite = mouseHold;
            }
            else if (hasBulb)
            {
                hasBulb = false;
                bulbManager.SelectBulb(-1);
            }
            //Debug.Log("Hovering over: " + hit.collider.gameObject.name);
        }
        else
        {
            mouseUI.gameObject.SetActive(false);
            if (hasBulb)
            {
                hasBulb = false;
                bulbManager.SelectBulb(-1);
            }

        }

    }
}
