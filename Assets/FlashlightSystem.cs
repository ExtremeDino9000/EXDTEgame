using UnityEngine;
using UnityEngine.InputSystem; // Uses Unity's New Input System

public class FlashlightSystem : MonoBehaviour
{
    [Header("References")]
    public Light flashlightLight; // Drag your Light component here
    public Transform cameraTransform; // Drag your Main Camera here

    [Header("Settings")]
    public float flashlightRange = 10f;
    public Key toggleKey = Key.F; // Default to 'F' key, change to LeftClick if preferred

    private bool isOn = false;

    void Start()
    {
        // Start with the flashlight turned off
        if (flashlightLight != null)
            flashlightLight.enabled = false;
    }

    void Update()
    {
        // 1. Handle Turning the Flashlight On/Off
        if (Keyboard.current != null && Keyboard.current[toggleKey].wasPressedThisFrame)
        {
            ToggleFlashlight();
        }

        // Alternatively, if you want Left Mouse Click to toggle it, uncomment below:
        /*
        if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
        {
            ToggleFlashlight();
        }
        */

        // 2. If the light is ON, constantly check if we are blinding Bonnie
        if (isOn && cameraTransform != null)
        {
            CheckForBonnie();
        }
    }

    void ToggleFlashlight()
    {
        isOn = !isOn;
        if (flashlightLight != null)
        {
            flashlightLight.enabled = isOn;
        }
        Debug.Log("Flashlight is now: " + (isOn ? "ON" : "OFF"));
    }

    void CheckForBonnie()
    {
        // Shoot a ray straight out from where the camera is looking
        Ray ray = new Ray(cameraTransform.position, cameraTransform.forward);
        RaycastHit hit;

        // Visualizes the flashlight beam in the Scene view (for debugging)
        Debug.DrawRay(cameraTransform.position, cameraTransform.forward * flashlightRange, Color.cyan);

        if (Physics.Raycast(ray, out hit, flashlightRange))
        {
            // Look for the FrozenBonnie script on whatever object the laser hits
            FrozenBonnie bonnie = hit.transform.GetComponent<FrozenBonnie>();
            
            if (bonnie != null)
            {
                // We found him! Tell him he is being flashed
                bonnie.SetBeingFlashed(true);
            }
        }
    }
}