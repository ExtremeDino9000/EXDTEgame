using UnityEngine;

public class SimplePanicBreathing : MonoBehaviour
{
    [Header("Breathing Settings")]
    public AudioSource breathingSource; 
    public AudioClip breathClip;        
    public float startAfterSeconds = 120f;
    public float repeatInterval = 8f;      

    [Header("Ambient Sound Settings")]
    public AudioSource ambientSource;   
    public AudioClip ambientClip;       
    public float nightDuration = 180f;  
    public float maxAmbientVolume = 1.0f; 

    private float timeElapsed = 0f;

    void Start()
    {
        // Setup and play the looping ambient track at 12am
        if (ambientSource != null && ambientClip != null)
        {
            ambientSource.clip = ambientClip;
            ambientSource.loop = true;
            ambientSource.volume = 0f; // Start completely silent
            ambientSource.Play();
        }

        // Schedule the breathing sound to start later in the night
        InvokeRepeating(nameof(PlayBreathing), startAfterSeconds, repeatInterval);
    }

    void Update()
    {
        // Smoothly scale the ambient volume over the entire 180-second night
        if (ambientSource != null && ambientSource.isPlaying)
        {
            timeElapsed += Time.deltaTime;
            
            // Calculate progress from 0.0 (12 AM) to 1.0 (6 AM)
            float nightProgress = Mathf.Clamp01(timeElapsed / nightDuration);

            // Gradually ramp up volume toward maxAmbientVolume
            ambientSource.volume = nightProgress * maxAmbientVolume;
        }
    }

    void PlayBreathing()
    {
        if (breathClip == null || breathingSource == null) return;

        // Randomize pitch slightly for variation
        breathingSource.pitch = Random.Range(0.8f, 1.2f);
        
        // Single volume setting for breathing
        breathingSource.volume = 1.1f; 

        breathingSource.PlayOneShot(breathClip);
    }
}