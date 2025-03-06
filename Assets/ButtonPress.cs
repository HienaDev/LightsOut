using UnityEngine;

public class ButtonPress : MonoBehaviour
{

    [SerializeField] private BulbManager bulbManager;
    public float pressDistance = 0.1f; // How far the button moves down
    public float pressDuration = 0.2f; // How long the button press takes

    private Vector3 originalPosition;  // Stores the original position of the button
    private bool isPressed = false;   // Tracks if the button is currently pressed
    private float pressTimer = 0f;    // Tracks the time elapsed during the press

    [SerializeField] private MouseHoverDetector mouseHoverDetector;


    [SerializeField] private GameManager gameManager;

    private PlaySound playSound;

    void Start()
    {
        // Save the original position of the button
        originalPosition = transform.position;
        playSound = GetComponent<PlaySound>();
    }

    void Update()
    {
        // Example: Press the button when the spacebar is pressed
        if (Input.GetMouseButtonDown(0) && mouseHoverDetector.hasButton && gameManager.waitingForPress)
        {
            PressButton();
        }

        // Handle the button press animation
        if (isPressed)
        {
            // Increment the timer
            pressTimer += Time.deltaTime;

            // Calculate the progress of the press (0 to 1)
            float progress = Mathf.Clamp01(pressTimer / pressDuration);

            // Move the button down and then back up
            if (progress <= 0.5f)
            {
                // Move down
                transform.position = originalPosition - Vector3.up * (pressDistance * progress * 2f);
            }
            else
            {
                // Move back up
                transform.position = originalPosition - Vector3.up * (pressDistance * (1f - progress) * 2f);
            }

            // Check if the animation is complete
            if (progress >= 1f)
            {
                isPressed = false; // Reset the button state
                pressTimer = 0f;  // Reset the timer
                transform.position = originalPosition; // Ensure the button is exactly at the original position
            }
        }
    }


    // Call this method to trigger the button press effect
    public void PressButton()
    {
        if (!isPressed)
        {
            isPressed = true;
        }

        gameManager.press = true;
        playSound.SoundDo();
    }


}
