using UnityEngine;

public class FirstPersonLook : MonoBehaviour
{
    [SerializeField] private float sensitivity = 2f; // Mouse sensitivity
    [SerializeField] private Transform playerBody;   // The player's body (to rotate the entire character)

    private float xRotation = 0f;

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


        if(canMove)
            LookAround();
    }

    void LookAround()
    {
        // Get mouse input
        float mouseX = Input.GetAxis("Mouse X") * sensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * sensitivity;

        // Rotate the camera up/down (invert Y movement)
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f); // Limit vertical rotation

        // Apply rotation
        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        playerBody.Rotate(Vector3.up * mouseX); // Rotate the player horizontally
    }
}
