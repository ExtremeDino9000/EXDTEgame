using UnityEngine;

public class CameraGlitchEffect : MonoBehaviour
{
    [Header("UI / Monitor Overlays")]
    public GameObject[] monitorStaticOverlays; // Drag all 3 monitor static objects here

    [Header("Sounds (Optional)")]
    public AudioSource staticAudioSource;
    public AudioClip staticZapSound;

    public void TriggerGlitch(float duration = 0.25f)
    {
        // Turn on static overlay for all monitors
        for (int i = 0; i < monitorStaticOverlays.Length; i++)
        {
            if (monitorStaticOverlays[i] != null)
            {
                monitorStaticOverlays[i].SetActive(true);
            }
        }

        // Play audio glitch zap
        if (staticAudioSource != null && staticZapSound != null)
        {
            staticAudioSource.PlayOneShot(staticZapSound);
        }

        // Automatically turn them all off after the delay
        CancelInvoke("HideGlitch");
        Invoke("HideGlitch", duration);
    }

    private void HideGlitch()
    {
        for (int i = 0; i < monitorStaticOverlays.Length; i++)
        {
            if (monitorStaticOverlays[i] != null)
            {
                monitorStaticOverlays[i].SetActive(false);
            }
        }
    }
}