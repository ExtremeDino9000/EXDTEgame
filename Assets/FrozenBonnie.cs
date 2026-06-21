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

    private int currentStage = 0;
    private float movementTimer;
    private float jumpscareTimer;
    private bool isBeingFlashed = false;

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
    }
}