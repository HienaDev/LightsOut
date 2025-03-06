using UnityEngine;

public class HoverEffect : MonoBehaviour
{
    [SerializeField] private float hoverHeight = 0.5f; // How high the object hovers
    [SerializeField] private float hoverSpeed = 1f;    // Speed of the hover motion

    private Vector3 startPosition; // Original position of the object

    private void Start()
    {
        // Store the original position of the object
        startPosition = transform.position;
    }

    private void Update()
    {
        // Calculate the new Y position using a sine wave
        float newY = startPosition.y + Mathf.Sin(Time.time * hoverSpeed) * hoverHeight;

        // Update the object's position
        transform.position = new Vector3(transform.position.x, newY, transform.position.z);
    }
}