using UnityEngine;
using TMPro;

public class GameClock : MonoBehaviour
{
    [Header("UI References")]
    public TextMeshProUGUI clockText; 
    public GameObject winScreenObject;

    [Header("Settings")]
    public float secondsPerHour = 60f;

    [Header("Audio")]
    public AudioSource winAudioSource;
    public AudioClip winSound;

    private int currentHour = 12;
    private float hourTimer = 0f;
    private bool gameEnded = false;

    void Start()
    {
        if (winScreenObject != null) 
        {
            winScreenObject.SetActive(false);
        }
        
        UpdateClockDisplay();
    }

    void Update()
    {
        if (gameEnded) return;

        // Keep track of time passing
        hourTimer += Time.deltaTime;

        // When the timer exceeds the limit, move to the next hour
        if (hourTimer >= secondsPerHour)
        {
            hourTimer = 0f;
            AdvanceHour();
        }
    }

    void AdvanceHour()
    {
        if (currentHour == 12) 
        {
            currentHour = 1;
        }
        else 
        {
            currentHour++;
        }

        UpdateClockDisplay();

        // Check for victory condition
        if (currentHour == 6)
        {
            WinGame();
        }
    }

    void UpdateClockDisplay()
    {
        string amPm = (currentHour == 6 || currentHour < 6 || currentHour == 12) ? "AM" : "PM";
        clockText.text = currentHour + " " + amPm;
    }

    void WinGame()
    {
        gameEnded = true;
        Debug.Log("6 AM! You survived the night!");
        
        // Show the victory screen overlay (turns on the black background and texts)
        if (winScreenObject != null)
        {
            winScreenObject.SetActive(true);
            Cursor.lockState = CursorLockMode.None;

        }

        StopAllSounds();

        // Play win audio
        if (winAudioSource != null)
        {
            winAudioSource.PlayOneShot(winSound);
        }

        // Free the mouse cursor so the player can exit to the menu
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        if (Camera.main != null)
        {
            MonoBehaviour[] cameraScripts = Camera.main.GetComponents<MonoBehaviour>();
            foreach (MonoBehaviour script in cameraScripts)
            {
                if (script != this) 
                {
                    script.enabled = false;
                }
            }
        }

        // Stop the game time
        Time.timeScale = 0f; 
    }

    void StopAllSounds()
    {
        AudioSource[] allAudioSources = FindObjectsOfType<AudioSource>();
        foreach (AudioSource audioSource in allAudioSources)
        {
            audioSource.Stop();
        }
    }
}