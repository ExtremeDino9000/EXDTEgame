using UnityEngine;
using UnityEngine.Video;

public class CameraFrozenFoxy : MonoBehaviour
{
    [Header("References")]
    public OfficeTemperature tempSystem; // Drag your object with the OfficeTemperature script here
    public VideoPlayer jumpscareVideo;

    [Header("Foxy Aggression Settings")]
    public float aggressionMeter = 0f;
    public float maxAggressionBeforeJumpscare = 100f;

    [Header("Foxy Duplicates")]
    public GameObject[] foxyStages; // This holds your 3 Foxy duplicates
    private int currentStage = -1;  // Start at -1 so it forces the first update
    
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
            aggressionMeter += aggressionMultiplier * Time.deltaTime;

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

        UpdateCameraVisuals();

    }

    void UpdateCameraVisuals()
    {
        int newStage = 0;

        // Manually control the exact meter breaks
        if (aggressionMeter >= 90f)       newStage = 3; // Gone / Running
        else if (aggressionMeter >= 50f)  newStage = 2; // Standing outside (Stage 2)
        else if (aggressionMeter >= 25f)  newStage = 1; // Peeking (Stage 1)
        else                              newStage = 0; // Hidden (Stage 0)

        if (newStage != currentStage)
        {
            currentStage = newStage;

            for (int i = 0; i < foxyStages.Length; i++)
            {
                if (foxyStages[i] != null)
                {
                    // This turns on the model only if it matches the current stage index
                    foxyStages[i].SetActive(i == currentStage);
                }
            }
        }
    }

    void TriggerFoxyJumpscare()
    {
        isJumpscaring = true;
        Debug.Log("FOXY JUMPSCARE! Office temperature was too high!");
        if (jumpscareVideo != null)
        {
            jumpscareVideo.Play();
        }
    }
}