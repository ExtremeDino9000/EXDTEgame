using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class VoidManager : MonoBehaviour
{
    [Header("Script References")]
    public BreakerSwitch LightScript;
    public Camera mainCamera;

    [Header("Game Over Settings")]
    public GameObject gameOverScreen;

    [Header("Void Object")]
    public Transform voidTransform;
    public Vector3 maxScale = new Vector3(60f, 20f, 60f);
    public Vector3 minScale = new Vector3(4f, 20f, 4f);

    [Header("Post-Processing Sync")]
    public Volume globalVolume;
    public float minVignette = 0.2f;
    public float maxVignette = 0.85f;

    [Header("Speed Settings")]
    public float shrinkSpeed = 1f;
    public float pushbackSpeed = 25f;

    [Header("Direct Camera Controls")]
    public float normalFOV = 60f;
    public float panicFOV = 45f;

    [Header("Camera Shake Settings")]
    public float maxShakeIntensity = 0.08f; // Strength of the shake at peak panic
    public float shakeSpeed = 25f;           // Speed of the shake vibration

    // Post-Processing Overrides
    private Vignette vignette;
    private FilmGrain filmGrain;
    private ChromaticAberration chromaticAberration;

    private bool lightsAreOn = true;
    private Vector3 originalCameraPosition;

    void Start()
    {
        if (mainCamera == null)
        {
            mainCamera = Camera.main;
        }

        if (mainCamera != null)
        {
            originalCameraPosition = mainCamera.transform.localPosition;
        }

        // Fetch overrides directly from the Global Volume Profile
        if (globalVolume != null && globalVolume.profile != null)
        {
            globalVolume.profile.TryGet(out vignette);
            globalVolume.profile.TryGet(out filmGrain);
            globalVolume.profile.TryGet(out chromaticAberration);

            if (vignette != null) vignette.intensity.overrideState = true;
            if (filmGrain != null) filmGrain.intensity.overrideState = true;
            if (chromaticAberration != null) chromaticAberration.intensity.overrideState = true;
        }
    }

    void Update()
    {
        if (voidTransform == null || LightScript == null) return;

        lightsAreOn = LightScript.isLightOn;

        if (!lightsAreOn)
        {
            // Lights OFF: Void closes in
            voidTransform.localScale = Vector3.MoveTowards(
                voidTransform.localScale, 
                minScale, 
                shrinkSpeed * Time.deltaTime
            );

            if (Vector3.Distance(voidTransform.localScale, minScale) < 0.1f)
            {
                // Trigger Game Over
                if (gameOverScreen != null)
                {
                    gameOverScreen.SetActive(true);
                }   
                
                StopAllSounds();  

            }          
        }
        else
        {
            // Lights ON: Void pushes back out
            voidTransform.localScale = Vector3.MoveTowards(
                voidTransform.localScale, 
                maxScale, 
                pushbackSpeed * Time.deltaTime
            );
        }

        UpdateVisualEffects();
    }

    void UpdateVisualEffects()
    {
        float denominator = maxScale.x - minScale.x;
        if (Mathf.Approximately(denominator, 0f)) return;

        // 0.0 = Safe (maxScale), 1.0 = Max Panic (minScale)
        float panicPercent = 1f - ((voidTransform.localScale.x - minScale.x) / denominator);
        panicPercent = Mathf.Clamp01(panicPercent);

        // 1. Camera FOV Zoom
        if (mainCamera != null)
        {
            mainCamera.fieldOfView = Mathf.Lerp(normalFOV, panicFOV, panicPercent);

            // 2. Procedural Camera Shake
            if (panicPercent > 0.1f)
            {
                float shakeOffsetDeltaX = (Mathf.Sin(Time.time * shakeSpeed) * maxShakeIntensity) * panicPercent;
                float shakeOffsetDeltaY = (Mathf.Cos(Time.time * shakeSpeed * 0.9f) * maxShakeIntensity) * panicPercent;
                mainCamera.transform.localPosition = originalCameraPosition + new Vector3(shakeOffsetDeltaX, shakeOffsetDeltaY, 0f);
            }
            else
            {
                mainCamera.transform.localPosition = originalCameraPosition;
            }
        }

        // 3. Vignette (Screen Edges Darkening)
        if (vignette != null)
        {
            vignette.intensity.value = Mathf.Lerp(minVignette, maxVignette, panicPercent);
        }

        // 4. Film Grain (Noise Intensity)
        if (filmGrain != null)
        {
            filmGrain.intensity.value = Mathf.Lerp(0.0f, 1.0f, panicPercent);
        }

        // 5. Chromatic Aberration (Color Fringe Distortion)
        if (chromaticAberration != null)
        {
            chromaticAberration.intensity.value = Mathf.Lerp(0.0f, 1.0f, panicPercent);
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