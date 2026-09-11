using UnityEngine;
using UnityEngine.UI; 
using UnityEngine.Video;
using UnityEngine.InputSystem;

public class OfficeManager : MonoBehaviour
{
    [Header("Power Settings")]
    public float currentPower = 100f;
    public float normalDrainRate = 0.5f;
    public float flashlightDrainRate = 2.0f;
    
    [Header("References")]
    public Light officeFlashlight;
    
    private Text powerText; 
    private VideoPlayer jumpscareVideoPlayer; 
    private bool isFlashlightOn = false;
    private bool isGameOver = false;

    void Start()
    {
        // Automatically find the text and video, but leave the flashlight to the Inspector assignment
        powerText = FindObjectOfType<Text>();
        jumpscareVideoPlayer = FindObjectOfType<VideoPlayer>();

        if (powerText == null) Debug.LogWarning("OfficeManager couldn't find a UI Text element!");
        if (jumpscareVideoPlayer == null) Debug.LogWarning("OfficeManager couldn't find a VideoPlayer!");
        
        // Force the flashlight to start off
        if (officeFlashlight != null) officeFlashlight.enabled = false;
    }

    void Update()
    {
        if (isGameOver) return;

        HandleInput();
        DrainPower();
        UpdateUI();
    }

    void HandleInput()
    {
        if (Keyboard.current != null && Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            if (officeFlashlight != null)
            {
                isFlashlightOn = !isFlashlightOn;
                officeFlashlight.enabled = isFlashlightOn;
            }
        }
    }

    void DrainPower()
    {
        float currentDrain = isFlashlightOn ? flashlightDrainRate : normalDrainRate;
        currentPower -= currentDrain * Time.deltaTime;

        if (currentPower <= 0)
        {
            currentPower = 0;
            TriggerPowerOutage();
        }
    }

    void UpdateUI()
    {
        if (powerText != null)
        {
            powerText.text = "Power: " + Mathf.CeilToInt(currentPower) + "%";
        }
    }

    void TriggerPowerOutage()
    {
        isGameOver = true;
        isFlashlightOn = false;
        if (officeFlashlight != null) officeFlashlight.enabled = false; 
        
        if (jumpscareVideoPlayer != null)
        {
            jumpscareVideoPlayer.Play();
        }
        
        Debug.Log("Power out! Freddy got you.");
    }
}