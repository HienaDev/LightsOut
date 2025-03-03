using NaughtyAttributes;
using UnityEngine;
using System.Collections;

public class RotateObject : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = 90f; // Degrees per second

    public bool isRotating = false; // Track if the object is currently rotating

    [SerializeField] private FirstPersonLook firstPersonLook;


    private void Update()
    {
        // Toggle swapping on/off with the space bar
        if (Input.GetKeyDown(KeyCode.P))
        {
            Rotate360();
        }
    }

    // Method to rotate the object 360 degrees around its Y axis
    [Button]
    public void Rotate360()
    {
        firstPersonLook.ToggleMove(false);
        if (!isRotating)
        {
            StartCoroutine(Rotate360Coroutine());
        }
    }

    // Coroutine to handle the 360-degree rotation
    private IEnumerator Rotate360Coroutine()
    {

        isRotating = true;

        float degreesRotated = 0f; // Track how many degrees have been rotated

        while (degreesRotated < 360f)
        {
            // Calculate the rotation amount for this frame
            float rotationAmount = rotationSpeed * Time.deltaTime;
            degreesRotated += Mathf.Abs(rotationAmount);

            // Rotate the object around its Y axis
            transform.Rotate(0, rotationAmount, 0);

            yield return null; // Wait until the next frame
        }

        // Ensure the object ends exactly at 360 degrees
        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, 360f, transform.rotation.eulerAngles.z);

        isRotating = false; // Rotation complete

        firstPersonLook.ToggleMove(true);
    }
}


