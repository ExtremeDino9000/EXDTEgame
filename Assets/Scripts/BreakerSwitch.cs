using UnityEngine;

public class BreakerSwitch : MonoBehaviour
{
    [Header("Lights to Control")]
    public Light mainRoomLight;
    public Light sideLight;
    public Light hallwayLight;

    [Header("Enemy Reference")]
    public FrozenFreddy frozenFreddyScript;

    [Header("Switch Settings")]
    public Transform switchHandle;
    public float offRotationX = -30f;
    public float onRotationX = 30f;

    [Header("Audio Settings")]
    public AudioSource officeAudioSource;
    public AudioClip switchSound;

    public bool isLightOn = true;

    void Start()
    {
        // Make sure the light matches the starting state
        if (mainRoomLight != null)
        {
            mainRoomLight.enabled = isLightOn;
        }

        if (sideLight != null)
        {
            sideLight.enabled = isLightOn;
        }

        UpdateSwitchVisual();
    }

    public void ToggleLight()
    {
        isLightOn = !isLightOn;

        if (mainRoomLight != null)
        {
            PlaySwitchSound();
            mainRoomLight.enabled = isLightOn;
        }

        if (sideLight != null)
        {
            sideLight.enabled = isLightOn;
        }

        if (hallwayLight != null)
        {
            hallwayLight.enabled = isLightOn;
        }

        if (frozenFreddyScript != null)
        {
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