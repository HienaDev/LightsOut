using UnityEngine;

public class Bulb : MonoBehaviour
{
    public int bulbIndex = 0;

    [SerializeField] private float moveDistance = 5f; // Exact distance to move
    [SerializeField] private float moveSpeed = 2f; // Speed of movement

    private Vector3 targetPosition; // Target position to move toward
    private Vector3 startPosition; // Starting position of the object

    public Renderer bulbRenderer;

    [SerializeField] private GameObject normalBulb;
    [SerializeField] private GameObject brokenBulb;
    [SerializeField] private GameObject pointLight;

    [SerializeField] private Collider bulbCollider;
    [SerializeField] private Rigidbody rb;

    [SerializeField] public bool isBulb;

    private PlaySound playSound;

    [SerializeField] private GameObject bloodVFX;

    public bool blood = false;

    private void Start()
    {
        playSound = GetComponent<PlaySound>();
        if (!isBulb)
            return;
        // Store the starting position
        startPosition = transform.position;

        // Generate a random target position at the exact distance
        GenerateRandomTargetPosition();

        Debug.Log("Moving to target position: " + targetPosition);
    }

    private void Update()
    {
        if (!isBulb)
            return;
        // Move the object toward the target position
        MoveTowardTarget();
    }

    // Method to generate a random target position at the exact distance
    private void GenerateRandomTargetPosition()
    {
        // Generate a random direction on the X and Y axes
        Vector2 randomDirection = Random.insideUnitCircle.normalized;

        // Calculate the target position at the exact distance
        targetPosition = startPosition + new Vector3(randomDirection.x, randomDirection.y, 0) * moveDistance;

        transform.position = targetPosition;
    }

    // Method to move the object toward the target position
    private void MoveTowardTarget()
    {
        // Move the object toward the target position
        transform.position = Vector3.MoveTowards(transform.position, startPosition,  moveSpeed * Time.deltaTime);

        // Check if the object has reached the target position
        if (Vector3.Distance(transform.position, startPosition) < 0.01f)
        {
            Debug.Log("Reached target position.");
            enabled = false; // Stop updating once the target is reached
        }
    }

    public void BlowUp()
    {

        // Remove index of bulb to check which are still alive
        bulbIndex = 9999;

        bulbCollider.enabled = false;
        normalBulb.SetActive(false);
        brokenBulb.SetActive(true);
        playSound.SoundDo();

        if(blood)
        {
            GameObject blood = Instantiate(bloodVFX);
            blood.transform.position = transform.position;
        }

        if (!isBulb)
        {
            return;
        }
      
        rb.isKinematic = false;

        ToggleLight(false);
    }

    public void ToggleLight(bool toggle)
    {
        pointLight.SetActive(toggle);

        if(toggle)
        {
            bulbRenderer.material.color = new Color(1f, 1f, 0f, 0.1f);
            bulbRenderer.material.EnableKeyword("_EMISSION");
        }
        else
        {
            bulbRenderer.material.color = new Color(1f, 1f, 1f, 0.1f);
            bulbRenderer.material.DisableKeyword("_EMISSION");
        }
    }
}   
