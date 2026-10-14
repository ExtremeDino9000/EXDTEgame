using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.Video;

public class GameClock : MonoBehaviour
{
    [Header("UI References")]
    public TextMeshProUGUI clockText; 
    public GameObject winScreenObject;
    public GameObject mainCanvas;
    public GameObject pauseManager;

    [Header("Videos")]
    public VideoPlayer cutsceneVideoPlayer;

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
        if (cutsceneVideoPlayer != null)
        {
            cutsceneVideoPlayer.loopPointReached += OnVideoFinished;
        }
        
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

        if (mainCanvas != null)
        {
            mainCanvas.SetActive(false);
        }

        pauseManager.SetActive(false);
        
        PlayCutscene();

        StopAllSounds();

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

    void PlayCutscene()
    {
        if (cutsceneVideoPlayer != null)
        {
            cutsceneVideoPlayer.Play();
        }
    }

    void OnVideoFinished(VideoPlayer vp)
    {
        Time.timeScale = 1f;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        SceneManager.LoadScene(0);
    }

    void OnDestroy()
    {
        if (cutsceneVideoPlayer != null)
        {
            cutsceneVideoPlayer.loopPointReached -= OnVideoFinished;
        }
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