using UnityEngine;
using UnityEngine.InputSystem; // REQUIRED for the New Input System

public class FlashlightSystem : MonoBehaviour
{
    [Header("References")]
    public Light flashlightLight; 
    public Transform cameraTransform; 
    public GameObject cameraCanvas; 

    [Header("Flashlight Settings")]
    public float flashlightRange = 10f;
    private bool isFlashlightOn = false;

    [Header("Interaction Settings")]
    public float interactRange = 3f;

    private bool isCameraOpen = false;

    void Start()
    {
        if (flashlightLight != null)
            flashlightLight.enabled = false;

        // Hide and lock cursor at start
        LockCursor(true);
    }

    void Update()
    {
        // 1. Monitor Toggle (TAB key) using New Input System
        if (Keyboard.current.tabKey.wasPressedThisFrame)
        {
            ToggleCameraView();
        }

        if (!isCameraOpen)
        {
            // 2. Flashlight Toggle (SPACEBAR key)
            if (Keyboard.current.spaceKey.wasPressedThisFrame)
            {
                ToggleFlashlight();
            }

            // 3. Interaction Check (LEFT MOUSE CLICK)
            if (Mouse.current.leftButton.wasPressedThisFrame) 
            {
                TryInteract();
            }

            // 4. Constant Bonnie Check
            if (isFlashlightOn && cameraTransform != null)
            {
                CheckForBonnie();
            }
        }
    }

    void ToggleCameraView()
    {
        isCameraOpen = !isCameraOpen;

        if (isCameraOpen)
        {
            LockCursor(false);
            if (cameraCanvas != null) cameraCanvas.SetActive(true);
            Debug.Log("Camera open: Mouse unlocked.");
        }
        else
        {
            LockCursor(true);
            if (cameraCanvas != null) cameraCanvas.SetActive(false);
            Debug.Log("Camera closed: Mouse locked.");
        }
    }

    void LockCursor(bool shouldLock)
    {
        if (shouldLock)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    void ToggleFlashlight()
    {
        isFlashlightOn = !isFlashlightOn;
        if (flashlightLight != null) flashlightLight.enabled = isFlashlightOn;
    }

    void TryInteract()
    {
        if (cameraTransform == null) return;

        Ray ray = new Ray(cameraTransform.position, cameraTransform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, interactRange))
        {
            BreakerSwitch breaker = hit.transform.GetComponent<BreakerSwitch>();
            if (breaker != null)
            {
                breaker.ToggleLight(); 
            }
        }
    }

    void CheckForBonnie()
    {
        Ray ray = new Ray(cameraTransform.position, cameraTransform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, flashlightRange))
        {
            FrozenBonnie bonnie = hit.transform.GetComponent<FrozenBonnie>();
            if (bonnie != null)
            {
                bonnie.ResetBonnie();
            }
        }
    }
}