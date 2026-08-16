using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement; // Crucial for restarting the scene

public class OfficeTemperature : MonoBehaviour
{
    [Header("References")]
    public BreakerSwitch breakerSwitch;
    public Image fadeScreenImage;

    [Header("Game Over UI Components")]
    public GameObject gameOverUI;

    [Header("Temperature Settings")]
    public float currentTemperature = 15f;
    public float heatSpeed = 1.5f;
    public float coolSpeed = 2f;
    public float darkCoolSpeed = 3f;
    public float minTemperature = -15f; 

    [Header("Warning Settings")]
    public float fadeStartThreshold = 0f;

    private bool isHeaterOn = true;
    private bool isGameOver = false;

    void Update()
    {
        if (!isGameOver)
        {
            CalculateTemperature();
            UpdateFadeEffect();
        }
    }

    void CalculateTemperature()
    {
        if (isHeaterOn)
        {
            currentTemperature += heatSpeed * Time.deltaTime;
        }
        else
        {
            float activeCoolSpeed = (breakerSwitch != null && !breakerSwitch.isLightOn) ? darkCoolSpeed : coolSpeed;
            currentTemperature -= activeCoolSpeed * Time.deltaTime;

            if (currentTemperature <= minTemperature)
            {
                currentTemperature = minTemperature;
                TriggerFreezingGameOver();
            }
        }
    }

    void UpdateFadeEffect()
    {
        if (fadeScreenImage == null) return;

        float fadeAlpha = 0f;

        if (currentTemperature <= fadeStartThreshold)
        {
            float totalRange = fadeStartThreshold - minTemperature;
            float progressInRange = fadeStartThreshold - currentTemperature;
            fadeAlpha = Mathf.Clamp01(progressInRange / totalRange);
        }

        Color color = fadeScreenImage.color;
        color.a = fadeAlpha;
        fadeScreenImage.color = color;

        if (fadeAlpha > 0.001f)
        {
            if (!fadeScreenImage.gameObject.activeSelf)
                fadeScreenImage.gameObject.SetActive(true);
        }
        else if (fadeAlpha <= 0f && !isGameOver)
        {
            if (fadeScreenImage.gameObject.activeSelf)
                fadeScreenImage.gameObject.SetActive(false);
        }
    }

    void TriggerFreezingGameOver()
    {
        isGameOver = true;
        Debug.Log("YOU FROZE TO DEATH! Game Over.");

        // 1. Force the screen to be solid, opaque black
        if (fadeScreenImage != null)
        {
            fadeScreenImage.gameObject.SetActive(true);
            Color color = fadeScreenImage.color;
            color.a = 1f;
            fadeScreenImage.color = color;
            
            // Re-enable Raycast Target so the player can click the restart button!
            fadeScreenImage.raycastTarget = true; 
        }

        // 2. Reveal the "You Died" text and the Restart Button
        if (gameOverUI != null) gameOverUI.SetActive(true);

        // 3. STOP THE GAME CLOCK: This freezes AI, animations, and timers entirely!
        Time.timeScale = 0f; 
    }

    // NEW FUNCTION: Call this from the Restart Button to try again
    public void RestartGame()
    {
        // Reset the game clock back to normal speed before reloading!
        Time.timeScale = 1f; 
        
        // Reload the current scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void ToggleHeater()
    {
        if (isGameOver) return;
        isHeaterOn = !isHeaterOn;
    }

    public float GetTemperature()
    {
        return currentTemperature;
    }
}