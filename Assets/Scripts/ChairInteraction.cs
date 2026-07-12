using UnityEngine;
using UnityEngine.InputSystem; // Using your New Input System!

public class ChairInteraction : MonoBehaviour
{
    [Header("References")]
    public Transform sitViewPosition; // Drag your SitViewPosition object here
    public Transform mainCamera;       // Drag your Main Camera here
    
    [Header("Settings")]
    public float interactionDistance = 3f;
    public float sitSpeed = 5f;

    private Vector3 originalCameraPosition;
    private Quaternion originalCameraRotation;
    private bool isSitting = false;
    private bool isTransitioning = false;
    private MonoBehaviour movementScript; // To temporarily freeze your walking script

    void Start()
    {
        // Automatically try to find your movement script on the camera or its parent
        if (mainCamera != null)
        {
            movementScript = mainCamera.GetComponent<MonoBehaviour>();
            if (movementScript == null && mainCamera.parent != null)
            {
                movementScript = mainCamera.parent.GetComponent<MonoBehaviour>();
            }
        }
    }

    void Update()
    {
        // Check for 'E' key press using the New Input System
        if (Keyboard.current != null && Keyboard.current.eKey.wasPressedThisFrame && !isTransitioning)
        {
            if (isSitting)
            {
                StandUp();
            }
            else
            {
                TryToSit();
            }
        }

        // Smoothly slide the camera into position
        if (isTransitioning)
        {
            HandleTransition();
        }
    }

    void TryToSit()
    {
        // Shoot a laser forward from the center of the screen to see if we are looking at the chair
        Ray ray = new Ray(mainCamera.position, mainCamera.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, interactionDistance))
        {
            // Check if the object we hit is our Chair
            if (hit.transform == transform || hit.transform.IsChildOf(transform))
            {
                // Save original spot so we can stand back up later
                originalCameraPosition = mainCamera.position;
                originalCameraRotation = mainCamera.rotation;

                // Disable player walking so they don't wander off while sitting
                if (movementScript != null) movementScript.enabled = false;

                isSitting = true;
                isTransitioning = true;
            }
        }
    }

    void StandUp()
    {
        isSitting = false;
        isTransitioning = true;
    }

    void HandleTransition()
    {
        Vector3 targetPos = isSitting ? sitViewPosition.position : originalCameraPosition;
        Quaternion targetRot = isSitting ? sitViewPosition.rotation : originalCameraRotation;

        // Smoothly interpolate position and rotation
        mainCamera.position = Vector3.Lerp(mainCamera.position, targetPos, Time.deltaTime * sitSpeed);
        mainCamera.rotation = Quaternion.Slerp(mainCamera.rotation, targetRot, Time.deltaTime * sitSpeed);

        // Stop transitioning once we are close enough
        if (Vector3.Distance(mainCamera.position, targetPos) < 0.05f)
        {
            mainCamera.position = targetPos;
            mainCamera.rotation = targetRot;
            isTransitioning = false;

            // Give control back to the player if they stood up
            if (!isSitting && movementScript != null)
            {
                movementScript.enabled = true;
            }
        }
    }
}