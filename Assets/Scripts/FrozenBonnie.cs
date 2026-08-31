using UnityEngine;

public class FrozenBonnie : MonoBehaviour
{
    [Header("Closet Waypoints")]
    public Transform stage0_Back;   // Deep hidden spot in the closet
    public Transform stage1_Middle; // Mid-way forward in the closet
    public Transform stage2_Hole;   // Pressed right up against the office wall hole

    [Header("Timing Settings")]
    public float movementInterval = 10f; // Tries to move every 10 seconds
    [Range(0, 20)] public int AILevel = 5;  // Higher number = moves faster
    public float jumpscareTimerLimit = 4f; // Seconds player has to flash him at Stage 2

    [Header("Audio Settings")]
    public AudioSource officeAudioSource;
    public AudioClip movementSound;

    [Header("Jumpscare Settings")]
    public UnityEngine.Video.VideoPlayer jumpscareVideoPlayer;
    public AudioSource jumpscareAudioSource;
    public AudioClip scareSound;
    public AudioClip cameraSound;

    [Header("Game Over UI")]
    public GameObject gameoverscreen;

    private int currentStage = 0;
    private float movementTimer;
    private float jumpscareTimer;
    private bool isBeingFlashed = false;

    public CameraGlitchEffect glitchSystem;

    void Start()
    {
        // Put Bonnie at the very back of the closet when the game starts
        ResetBonnie();
    }

    void Update()
    {
        // 1. AI Routine: If he hasn't reached the hole yet, count down to move forward
        if (currentStage < 2)
        {
            movementTimer += Time.deltaTime;
            if (movementTimer >= movementInterval)
            {
                movementTimer = 0f;
                TryToAdvance();
            }
        }
        // 2. Danger Zone: If he is staring through the hole, start the kill timer
        else if (currentStage == 2)
        {
            jumpscareTimer += Time.deltaTime;

            if (isBeingFlashed)
            {
                ResetBonnie();
            }
            else if (jumpscareTimer >= jumpscareTimerLimit)
            {
                TriggerJumpscare();
            }
        }
    }

    void TryToAdvance()
    {
        // Classic FNaF RNG: Roll a number between 1 and 20
        int roll = Random.Range(1, 21);
        if (roll <= AILevel)
        {
            PlayAdvanceSound();
            CameraSound();
            CameraGlitchEffect glitchEffect = glitchSystem.GetComponent<CameraGlitchEffect>();
            if (glitchEffect != null)
            {
                glitchEffect.TriggerGlitch(0.25f);
            }
            currentStage++;
            TeleportToStage(currentStage);
            Debug.Log($"Bonnie advanced to Stage {currentStage}!");
        }
    }

    public void ResetBonnie()
    {
        currentStage = 0;
        movementTimer = 0f;
        jumpscareTimer = 0f;
        isBeingFlashed = false;
        TeleportToStage(0);
        Debug.Log("Bonnie was flashed and snapped back to Stage 0.");
    }

    void TeleportToStage(int stage)
    {
        Transform targetWaypoint = null;

        switch (stage)
        {
            case 0: targetWaypoint = stage0_Back; break;
            case 1: targetWaypoint = stage1_Middle; break;
            case 2: targetWaypoint = stage2_Hole; break;
        }

        if (targetWaypoint != null)
        {
            // Instantly snap Bonnie's 3D model to the exact spot and rotation
            transform.position = targetWaypoint.position;
            transform.rotation = targetWaypoint.rotation;
        }
    }

    // Your Flashlight script will call this method when aiming at him
    public void SetBeingFlashed(bool state)
    {
        if (currentStage == 2)
        {
            isBeingFlashed = state;
        }
    }

    void TriggerJumpscare()
    {
        currentStage = 3;
        Debug.Log("GAME OVER: Frozen Bonnie jumpscared you through the hole!");
        JumpscareVideo();
    }

    void PlayAdvanceSound()
    {
        if (officeAudioSource != null && movementSound != null)
        {
            officeAudioSource.PlayOneShot(movementSound, 0.1f);
        }
    }

    void JumpscareVideo()
    {
        if (jumpscareVideoPlayer != null)
        {
            jumpscareAudioSource.PlayOneShot(scareSound);
            jumpscareVideoPlayer.Play();
            Invoke("ShowGameOver", 3f);
        }
    }

    void CameraSound()
    {
        if (jumpscareAudioSource != null && cameraSound != null)
        {
            jumpscareAudioSource.PlayOneShot(cameraSound, 0.2f);
        }
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