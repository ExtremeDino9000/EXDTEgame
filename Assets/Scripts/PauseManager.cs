using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class PauseManager : MonoBehaviour
{
    public GameObject pauseMenuUI;
    public static bool isPaused = false;
    public MonoBehaviour fpsControllerScript;

    void Update()
    {
        if (Keyboard.current != null && (Keyboard.current.escapeKey.wasPressedThisFrame))
        {
            if (isPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        AudioListener.pause = false;
        isPaused = false;

        if (fpsControllerScript != null) fpsControllerScript.enabled = true;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        AudioListener.pause = true;
        isPaused = true;

        if (fpsControllerScript != null) fpsControllerScript.enabled = false;
        
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void QuitToMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
