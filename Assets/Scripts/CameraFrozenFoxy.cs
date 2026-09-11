using UnityEngine;
using UnityEngine.Video;

public class CameraFrozenFoxy : MonoBehaviour
{
    [Header("References")]
    public OfficeTemperature tempSystem;
    public VideoPlayer jumpscareVideo;

    [Header("UI Elements")]
    public GameObject gameoverscreen;

    [Header("Sounds")]
    public AudioSource jumpscareAudioSource;
    public AudioClip scareSound;

    [Header("Foxy Aggression Settings")]
    public float aggressionMeter = 0f;
    public float maxAggressionBeforeJumpscare = 100f;

    [Header("Foxy Duplicates")]
    public GameObject[] foxyStages;
    private int currentStage = -1;
    
    // How fast he gets mad per degree above 20°C
    public float aggressionMultiplier = 2f; 

    private bool isJumpscaring = false;

    void Update()
    {
        if (isJumpscaring || tempSystem == null) return;

        float currentTemp = tempSystem.GetTemperature();

        // Aggressive only if temperature is above 20°C
        if (currentTemp > 20f)
        {
            aggressionMeter += aggressionMultiplier * Time.deltaTime;

            if (aggressionMeter >= maxAggressionBeforeJumpscare)
            {
                TriggerFoxyJumpscare();
            }
        }
        else
        {
            // If the player cools the office below 20°C, Foxy calms down over time
            aggressionMeter -= 20f * Time.deltaTime;
            if (aggressionMeter < 0f) aggressionMeter = 0f;
        }

        UpdateCameraVisuals();

    }

    void UpdateCameraVisuals()
    {
        int newStage = 0;

        if (aggressionMeter >= 90f)       newStage = 3;
        else if (aggressionMeter >= 50f)  newStage = 2;
        else if (aggressionMeter >= 25f)  newStage = 1;
        else                              newStage = 0;

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
            jumpscareAudioSource.PlayOneShot(scareSound);
            jumpscareVideo.Play();
            Invoke("ShowGameOver", 3f);
        }
    }

    void OnJumpscareFinished(VideoPlayer vp)
    {
        ShowGameOver();
    }

    void ShowGameOver()
    {
        if (gameoverscreen != null)
        {
            gameoverscreen.SetActive(true);
        }

        Time.timeScale = 0f;
    }
}