using UnityEngine;

[RequireComponent(typeof(AudioSource))] // Automatically ensures the poster has a speaker component
public class PosterEasterEgg : MonoBehaviour
{
    [Header("Textures")]
    public Texture normalTexture;  // Your regular Freddy poster
    public Texture creepyTexture;  // The Golden Freddy poster

    [Header("Audio")]
    public AudioClip creepySound;  // Drag your whisper, giggle, or distortion sound here

    [Header("Settings")]
    [Range(0, 100)] public float chanceToChange = 5f; // % chance to change
    public float checkInterval = 10f; // Checks every X seconds

    private Renderer posterRenderer;
    private AudioSource audioSource;
    private bool isCreepy = false;
    private float timer;

    void Start()
    {
        posterRenderer = GetComponent<Renderer>();
        audioSource = GetComponent<AudioSource>();
        
        // Setup audio source settings cleanly via code
        audioSource.playOnAwake = false;
        audioSource.loop = false;

        // Ensure it starts with the normal texture
        if (posterRenderer != null && normalTexture != null)
        {
            posterRenderer.material.mainTexture = normalTexture;
        }
    }

    void Update()
    {
        if (!isCreepy)
        {
            timer += Time.deltaTime;
            if (timer >= checkInterval)
            {
                timer = 0f;
                TryTriggerEasterEgg();
            }
        }
    }

    void TryTriggerEasterEgg()
    {
        float roll = Random.Range(0f, 100f);
        if (roll <= chanceToChange)
        {
            TriggerCreepyPoster();
        }
    }

    public void TriggerCreepyPoster()
    {
        isCreepy = true;
        if (posterRenderer != null && creepyTexture != null)
        {
            posterRenderer.material.mainTexture = creepyTexture;
            Debug.Log("The poster has changed...");
        }

        // PLAY THE SOUND EFFECT
        if (audioSource != null && creepySound != null)
        {
            audioSource.PlayOneShot(creepySound);
        }
    }

    public void ResetPoster()
    {
        isCreepy = false;
        timer = 0f;
        if (posterRenderer != null && normalTexture != null)
        {
            posterRenderer.material.mainTexture = normalTexture;
        }
        
        if (audioSource != null)
        {
            audioSource.Stop(); // Stop playing the audio if it resets
        }
    }
    
    public bool IsPosterCreepy()
    {
        return isCreepy;
    }
}