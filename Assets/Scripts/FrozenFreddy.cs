using UnityEngine;

public class FrozenFreddy : MonoBehaviour
{
    [Header("AI Settings")]
    public float movementInterval = 10f; 
    [Range(0, 20)] public int aiDifficulty = 5; 

    [Header("State Tracking")]
    public int currentStage = 0; 
    public float gracePeriod = 6f; // Time player has to turn off lights before a jumpscare

    [Header("Retreat Settings")]
    public float retreatDelay = 3f; // NEW: How many seconds the player must sit in the dark before Freddy leaves
    private float retreatTimer = 0f;  // NEW: Tracks how long the lights have been off

    [Header("Teleportation Setup")]
    public GameObject frozenFreddyModel; 
    public Transform[] stageWaypoints;   

    [Header("Audio")]
    public AudioSource officeAudioSource;
    public AudioClip ventThumpSound; 
    public AudioClip breathingSound;  

    private float movementTimer = 0f;
    private float attackTimer = 0f;
    private bool isMainLightsOff = false;
    private bool isJumpscared = false;

    void Start()
    {
        TeleportFreddy();
    }

    void Update()
    {
        if (isJumpscared) return;

        // 1. Regular Vent Movement (Stages 0 to 2)
        if (currentStage < 3)
        {
            movementTimer += Time.deltaTime;
            if (movementTimer >= movementInterval)
            {
                movementTimer = 0f;
                TryMoveFreddy();
            }
        }
        // 2. Office Vent Danger Zone (Stage 3)
        else if (currentStage == 3)
        {
            if (isMainLightsOff)
            {
                // NEW: Lights are off, so tick the retreat timer!
                retreatTimer += Time.deltaTime;
                
                if (retreatTimer >= retreatDelay)
                {
                    ResetFreddy(); // He finally leaves after sitting in the dark
                }
            }
            else
            {
                // NEW: If the player turns the lights back ON too early, reset his retreat timer!
                retreatTimer = 0f;

                // Lights are still on. Timer counts down to a jumpscare.
                attackTimer += Time.deltaTime;
                if (attackTimer >= gracePeriod)
                {
                    TriggerJumpscare();
                }
            }
        }
    }

    void TryMoveFreddy()
    {
        int roll = Random.Range(1, 21);
        if (roll <= aiDifficulty)
        {
            currentStage++;
            Debug.Log("Frozen Freddy advanced to Stage: " + currentStage);

            TeleportFreddy();

            if (currentStage == 3)
            {
                if (officeAudioSource != null && breathingSound != null)
                {
                    officeAudioSource.clip = breathingSound;
                    officeAudioSource.loop = true;
                    officeAudioSource.Play();
                }
            }
            else
            {
                if (officeAudioSource != null && ventThumpSound != null)
                {
                    officeAudioSource.PlayOneShot(ventThumpSound);
                }
            }
        }
    }

    public void UpdateLightsState(bool areLightsOff)
    {
        isMainLightsOff = areLightsOff;
    }

    public void TeleportFreddy()
    {
        if (frozenFreddyModel != null && stageWaypoints != null && currentStage < stageWaypoints.Length)
        {
            Transform targetWaypoint = stageWaypoints[currentStage];
            if (targetWaypoint != null)
            {
                frozenFreddyModel.transform.position = targetWaypoint.position;
                frozenFreddyModel.transform.rotation = targetWaypoint.rotation;
            }
        }
    }

    void ResetFreddy()
    {
        Debug.Log("Frozen Freddy was fooled by the darkness and went back to the start.");
        currentStage = 0;
        attackTimer = 0f;
        retreatTimer = 0f; // NEW: Reset our tracking timer

        TeleportFreddy();

        if (officeAudioSource != null)
        {
            officeAudioSource.Stop();
            officeAudioSource.loop = false;
        }
    }

    void TriggerJumpscare()
    {
        isJumpscared = true;
        Debug.Log("JUMPSCARE: Frozen Freddy got you from the vents!");
    }
}