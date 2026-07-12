using UnityEngine;

public class CameraFrozenFoxy : MonoBehaviour
{
    [Header("References")]
    public OfficeTemperature tempSystem; // Drag your object with the OfficeTemperature script here

    [Header("Foxy Aggression Settings")]
    public float aggressionMeter = 0f;
    public float maxAggressionBeforeJumpscare = 100f;
    
    // How fast he gets mad per degree above 10°C
    public float aggressionMultiplier = 2f; 

    private bool isJumpscaring = false;

    void Update()
    {
        if (isJumpscaring || tempSystem == null) return;

        float currentTemp = tempSystem.GetTemperature();

        // FOXY MECHANIC: Aggressive only if temperature is above 10°C
        if (currentTemp > 10f)
        {
            // Calculate how many degrees above 10 we are (e.g., 14°C means 4 degrees over)
            float degreesOverLimit = currentTemp - 10f;

            // Build up aggression. The higher the temperature, the faster it grows!
            aggressionMeter += degreesOverLimit * aggressionMultiplier * Time.deltaTime;

            if (aggressionMeter >= maxAggressionBeforeJumpscare)
            {
                TriggerFoxyJumpscare();
            }
        }
        else
        {
            // If the player successfully cools the office below 10°C, Foxy calms down over time
            aggressionMeter -= 10f * Time.deltaTime;
            if (aggressionMeter < 0f) aggressionMeter = 0f;
        }
    }

    void TriggerFoxyJumpscare()
    {
        isJumpscaring = true;
        Debug.Log("FOXY JUMPSCARE! Office temperature was too high!");
        // TODO: Trigger your game over or jumpscare animation here
    }
}