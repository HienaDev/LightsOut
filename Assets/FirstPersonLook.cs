using UnityEngine;

public class FirstPersonLook : MonoBehaviour
{
    [SerializeField] private float sensitivity = 2f; // Mouse sensitivity
    [SerializeField] private Transform playerBody;   // The player's body (to rotate the entire character)

    [SerializeField] private float minXRotation = -90f; // Minimum vertical rotation (up/down)
    [SerializeField] private float maxXRotation = 90f;  // Maximum vertical rotation (up/down)
    [SerializeField] private float minYRotation = -90f; // Minimum horizontal rotation (left/right)
    [SerializeField] private float maxYRotation = 90f;  // Maximum horizontal rotation (left/right)

    private float xRotation = 0f; // Vertical rotation (up/down)
    private float yRotation = 0f; // Horizontal rotation (left/right)

    private bool canMove = true;

    void Start()
    {
        // Lock and hide cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void ToggleMove(bool toggle) => canMove = toggle;

    void Update()
    {
        if (canMove)
            LookAround();
    }

    void LookAround()
    {
        // Get mouse input
        float mouseX = Input.GetAxis("Mouse X") * sensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * sensitivity;

        // Rotate the camera up/down (invert Y movement)
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, minXRotation, maxXRotation); // Limit vertical rotation

        // Rotate the player left/right
        yRotation += mouseX;
        yRotation = Mathf.Clamp(yRotation, minYRotation, maxYRotation); // Limit horizontal rotation

        // Apply rotations
        transform.localRotation = Quaternion.Euler(xRotation, yRotation, 0f); // Apply both vertical and horizontal rotation to the camera
    }
}