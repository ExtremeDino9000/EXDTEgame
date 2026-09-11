using UnityEngine;
using UnityEngine.InputSystem;

public class ChairInteraction : MonoBehaviour
{
    [Header("References")]
    public Transform sitViewPosition;
    public Transform mainCamera;
    
    [Header("Settings")]
    public float interactionDistance = 3f;
    public float sitSpeed = 5f;

    private Vector3 originalCameraPosition;
    private Quaternion originalCameraRotation;
    private bool isSitting = false;
    private bool isTransitioning = false;
    private MonoBehaviour movementScript;

    void Start()
    {
        // Automatically try to find movement script on the camera or its parent
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
        // Check for E key press using the input system
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

        // Slide the camera into position
        if (isTransitioning)
        {
            HandleTransition();
        }
    }

    void TryToSit()
    {
        Ray ray = new Ray(mainCamera.position, mainCamera.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, interactionDistance))
        {
            if (hit.transform == transform || hit.transform.IsChildOf(transform))
            {
                originalCameraPosition = mainCamera.position;
                originalCameraRotation = mainCamera.rotation;

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