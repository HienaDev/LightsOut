using UnityEngine;

public class CameraShakeWithObject : MonoBehaviour
{
    [Header("Shake Settings")]
    public GameObject shakeObject;          // The object to shake around (required)
    public float maxShakeIntensity = 0.5f;  // Maximum intensity of the shake
    public float shakeRampUpSpeed = 1f;     // Speed at which the shake intensity increases
    public float shakeRampDownSpeed = 2f;   // Speed at which the shake intensity decreases
    public float shakeFrequency = 10f;      // Frequency of the shake (how fast it oscillates)
    public float initialDelay = 1f;         // Delay before shaking starts
    public bool canStartShaking = false;    // Whether shaking is allowed to start

    private Vector3 originalCameraPosition; // The original position of the camera
    private Quaternion originalCameraRotation; // The original rotation of the camera
    private float currentShakeIntensity = 0f; // Current intensity of the shake
    private GameObject shakeReferenceObject; // The object used as a reference when shaking starts
    private float delayTimer = 0f;          // Timer for the initial delay
    private bool isShaking = false;         // Whether the camera is currently shaking

    [SerializeField] private FirstPersonLook firstPersonLook;

    [SerializeField] private Transform cameraEndPosition;

    [SerializeField] private RotateObject rotateObject;

    [SerializeField] private float timeToBlowUpBulb = 5f;

    [SerializeField] private GameManager gameManager;

    public bool endShakeEvent = false;

    [SerializeField] private AudioSource intenseSoundSource;
    [SerializeField] private float startingTime = 1f;
    private float defaultVolume;
    public void SetObject(GameObject sObject)
    {
        shakeObject = sObject;
    }

    void Start()
    {
        defaultVolume = intenseSoundSource.volume;
        intenseSoundSource.volume = 0f;
        // Store the original position and rotation of the camera
        originalCameraPosition = cameraEndPosition.transform.position;
        originalCameraRotation = cameraEndPosition.transform.rotation;
    }

    void Update()
    {

        if (endShakeEvent)
        {
            // Generate smooth random shake using Perlin noise
            float shakeX = Mathf.PerlinNoise(Time.time * shakeFrequency, 0) * 2 - 1;
            float shakeY = Mathf.PerlinNoise(0, Time.time * shakeFrequency) * 2 - 1;
            float shakeZ = Mathf.PerlinNoise(Time.time * shakeFrequency, Time.time * shakeFrequency) * 2 - 1;

            // Combine the shake values into a Vector3 and scale by the intensity
            Vector3 shakeOffset = new Vector3(shakeX, shakeY, shakeZ) * maxShakeIntensity * 2;

            // Apply the shake to the camera's position relative to its original position
            transform.position = originalCameraPosition + shakeOffset;
            return;
        }
            

        // If no object is assigned, do nothing
        if (shakeObject == null)
        {
            if(!rotateObject.isRotating)
                firstPersonLook.ToggleMove(true); // Disable player movement while shaking

            return;
        }
            

        // If shaking is not allowed, do nothing
        if (!canStartShaking)
            return;


        if(Input.GetMouseButtonDown(0))
        {
            
            intenseSoundSource.time = startingTime;
        }

        if (Input.GetMouseButtonUp(0))
        {
            intenseSoundSource.volume = 0;
        }

        // Check if the left mouse button is held down
        if (Input.GetMouseButton(0))
        {
            delayTimer += Time.deltaTime;
            intenseSoundSource.volume += 0.35f * Time.deltaTime;
            if(intenseSoundSource.volume > defaultVolume)
            {
                intenseSoundSource.volume = defaultVolume;
            }
            if (delayTimer > timeToBlowUpBulb)
            {
                shakeObject.GetComponent<Bulb>().BlowUp();
                intenseSoundSource.volume = 0;
                gameManager.BreakBulb();
                shakeObject = null;
                delayTimer = 0f;
                return;
            }

            firstPersonLook.ToggleMove(false); // Disable player movement while shaking
            // Handle the initial delay
            if (delayTimer < initialDelay)
            {
                // Smoothly rotate the camera to look at the shakeObject during the delay
                Quaternion targetRotation = Quaternion.LookRotation(shakeObject.transform.position - transform.position);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, delayTimer / initialDelay);

                return; // Exit early until the delay is over
            }

            // If shaking hasn't started yet, store the current object as the reference
            if (shakeReferenceObject == null)
            {
                shakeReferenceObject = shakeObject;
                isShaking = true;
            }

            // Ramp up the shake intensity until it reaches the maximum
            currentShakeIntensity = Mathf.Lerp(currentShakeIntensity, maxShakeIntensity, Time.deltaTime * shakeRampUpSpeed);
        }
        else
        {
            firstPersonLook.ToggleMove(true); // Disable player movement while shaking
            delayTimer = 0f; // Reset the delay timer if the mouse button is released
            // Ramp down the shake intensity until it reaches zero
            currentShakeIntensity = Mathf.Lerp(currentShakeIntensity, 0f, Time.deltaTime * shakeRampDownSpeed);

            // If the shake intensity is close to zero, reset the reference object and stop shaking
            if (currentShakeIntensity < 0.01f)
            {
                shakeReferenceObject = null;
                isShaking = false;
            }
        }

        // Apply the camera shake if there is any intensity
        if (currentShakeIntensity > 0 && isShaking)
        {
            // Generate smooth random shake using Perlin noise
            float shakeX = Mathf.PerlinNoise(Time.time * shakeFrequency, 0) * 2 - 1;
            float shakeY = Mathf.PerlinNoise(0, Time.time * shakeFrequency) * 2 - 1;
            float shakeZ = Mathf.PerlinNoise(Time.time * shakeFrequency, Time.time * shakeFrequency) * 2 - 1;

            // Combine the shake values into a Vector3 and scale by the intensity
            Vector3 shakeOffset = new Vector3(shakeX, shakeY, shakeZ) * currentShakeIntensity;

            // Apply the shake to the camera's position relative to its original position
            transform.position = originalCameraPosition + shakeOffset;
        }
        else
        {
            // Reset the camera to its original position and rotation when there is no shake
            //transform.position = originalCameraPosition;
            //transform.rotation = Quaternion.Slerp(transform.rotation, originalCameraRotation, Time.deltaTime * shakeRampDownSpeed);
            
        }
    }
}