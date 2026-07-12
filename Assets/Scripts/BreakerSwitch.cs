using UnityEngine;

public class BreakerSwitch : MonoBehaviour
{
    [Header("Lights to Control")]
    public Light mainRoomLight; // Drag your main room light here

    [Header("Switch Settings")]
    public Transform switchHandle; // OPTIONAL: Drag the moving part of the switch here
    public float offRotationX = -30f;
    public float onRotationX = 30f;

    private bool isLightOn = true;

    void Start()
    {
        // Ensure the light matches our starting state
        if (mainRoomLight != null)
        {
            mainRoomLight.enabled = isLightOn;
        }
        UpdateSwitchVisual();
    }

    // This is the function our interaction script will call
    public void ToggleLight()
    {
        isLightOn = !isLightOn;

        if (mainRoomLight != null)
        {
            mainRoomLight.enabled = isLightOn;
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
}