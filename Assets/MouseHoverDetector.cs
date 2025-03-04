using UnityEngine;

public class MouseHoverDetector : MonoBehaviour
{
    [SerializeField] private float checkCooldown = 0.2f; // Cooldown time in seconds
    private float nextCheckTime = 0f; // Timer to track next allowed check

    private BulbManager bulbManager;

    private bool hasBulb = false;

    public bool hasButton = false;
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

            if(hit.collider.gameObject.GetComponent<ButtonPress>() != null)
            {
                hasButton = true;
            }
            else
            {
                hasButton = false;
            }

            if (bulb != null)
            {
                if(bulb.bulbIndex != bulbManager.bulbIndex)
                {
                    hasBulb = true;
                    bulbManager.SelectBulb(bulb.bulbIndex);
                }
            }
            else if(hasBulb)
            {
                hasBulb = false;
                bulbManager.SelectBulb(-1);
            }
            //Debug.Log("Hovering over: " + hit.collider.gameObject.name);
        }
        else
        {
            if(hasBulb)
            {
                hasBulb = false;
                bulbManager.SelectBulb(-1);
            }
        }

    }
}
