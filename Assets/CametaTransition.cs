using UnityEngine;

public class CameraTransition : MonoBehaviour
{
    public Transform startTransform; // Starting position and rotation
    public Transform endTransform;   // Ending position and rotation
    public float transitionDuration = 2.0f; // Duration of the transition in seconds

    private float transitionTime = 0f; // Time elapsed during the transition
    private bool isTransitioning = false; // Whether the transition is active

    [SerializeField] private GameObject handUI;
    [SerializeField] private FirstPersonLook firstPersonLook;
    [SerializeField] private GameManager gameManager;

    private void Start()
    {

        handUI.SetActive(false);
        firstPersonLook.enabled = (false);
        //StartTransition();
    }

    void Update()
    {
        if (isTransitioning)
        {
            // Increment the transition time
            transitionTime += Time.deltaTime;

            // Calculate the normalized time (0 to 1)
            float t = Mathf.Clamp01(transitionTime / transitionDuration);

            // Lerp the position
            transform.position = Vector3.Lerp(startTransform.position, endTransform.position, t);

            // Slerp the rotation (spherical interpolation for smooth rotation)
            transform.rotation = Quaternion.Slerp(startTransform.rotation, endTransform.rotation, t);

            // Check if the transition is complete
            if (t >= 1.0f)
            {
                isTransitioning = false;
                Debug.Log("Transition complete!");
                handUI.SetActive(true);
                gameManager.StartGame();
                firstPersonLook.enabled = (true);
            }
        }
    }

    // Call this method to start the transition
    public void StartTransition()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        if (startTransform == null || endTransform == null)
        {
            Debug.LogError("Start and End transforms must be assigned!");
            return;
        }

        // Reset transition time and start transitioning
        transitionTime = 0f;
        isTransitioning = true;

    }
}