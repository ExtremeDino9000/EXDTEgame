using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class PosterEasterEgg : MonoBehaviour

{
    [Header("Model")]
    public GameObject goldenFreddyModel;
    
    [Header("Textures")]
    public Texture normalTexture;  // Regular Freddy poster
    public Texture creepyTexture;  // Golden Freddy poster

    [Header("Audio")]
    public AudioClip creepySound;
    public AudioClip officePresenceLoop;

    [Header("Settings")]
    [Range(0, 100)] public float chanceToChange = 5f; // % chance to change
    public float checkInterval = 10f;

    private Renderer posterRenderer;
    private AudioSource audioSource;
    private bool isCreepy = false;
    private float timer;

    void Start()
    {
        posterRenderer = GetComponent<Renderer>();
        audioSource = GetComponent<AudioSource>();
        
        audioSource.playOnAwake = false;
        audioSource.loop = false;

        // Make sure it starts with the normal texture
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
            SpawnGoldenFreddy();
            Debug.Log("The poster has changed...");
        }

        // Play sound effect
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

    public void SpawnGoldenFreddy()
    {
        if (goldenFreddyModel != null)
        {
            goldenFreddyModel.SetActive(true);
        }

        if (audioSource != null && officePresenceLoop != null)
        {
            audioSource.clip = officePresenceLoop;
            audioSource.loop = true;
            audioSource.Play();
        }

        Invoke("DespawnGoldenFreddy", 5f);
    }

    public void DespawnGoldenFreddy()
    {
        if (goldenFreddyModel != null)
        {
            goldenFreddyModel.SetActive(false);
        }

        if (audioSource != null)
        {
            audioSource.Stop();
            audioSource.loop = false;
        }

        posterRenderer.material.mainTexture = normalTexture;
    }
}