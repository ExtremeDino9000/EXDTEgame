using UnityEngine;

public class BreakerSwitch : MonoBehaviour
{
    [Header("Lights to Control")]
    public Light mainRoomLight; // Drag your main room light here

    [Header("Enemy Reference")]
    public FrozenFreddy frozenFreddyScript; // Drag your Frozen Freddy manager here

    [Header("Switch Settings")]
    public Transform switchHandle; // OPTIONAL: Drag the moving part of the switch here
    public float offRotationX = -30f;
    public float onRotationX = 30f;

    [Header("Audio Settings")]
    public AudioSource officeAudioSource;
    public AudioClip switchSound;

    public bool isLightOn = true;

    void Start()
    {
        // Ensure the light matches our starting state
        if (mainRoomLight != null)
        {
            mainRoomLight.enabled = isLightOn;
        }
        UpdateSwitchVisual();
    }

    // This is the function your interaction script needs to call!
    public void ToggleLight()
    {
        isLightOn = !isLightOn;

        if (mainRoomLight != null)
        {
            PlaySwitchSound();
            mainRoomLight.enabled = isLightOn;
        }

        // --- NEW: Tell Frozen Freddy if the main lights are OFF ---
        if (frozenFreddyScript != null)
        {
            // If isLightOn is true, then areLightsOff is false (and vice versa)
            frozenFreddyScript.UpdateLightsState(!isLightOn);
        }

        UpdateSwitchVisual();
        Debug.Log("Breaker flipped! Light is now: " + (isLightOn ? "ON" : "OFF"));
    }

    void UpdateSwitchVisual()
    {
        // Rotates the handle mesh up or down so you can visually see it move
        if (switchHandle != null)
        {
            float targetX = isLightOn ? onRotationX : offRotationX;
            switchHandle.localRotation = Quaternion.Euler(targetX, 0f, 0f);
        }
    }

    void PlaySwitchSound()
    {
        officeAudioSource.PlayOneShot(switchSound, 0.4f);
    }
}