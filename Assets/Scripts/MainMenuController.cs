using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    [Header("UI References")]
    public GameObject creditsMenuObject;

    public void StartGame()
    {
        SceneManager.LoadScene(1);
        Time.timeScale = 1f;
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Game Exited");
    }

    public void CreditsMenu()
    {
        creditsMenuObject.SetActive(true);
    }

    public void BackToMainMenu()
    {
        creditsMenuObject.SetActive(false);
    }

    public void BackToMainMenuFromWinScreen()
    {
        SceneManager.LoadScene(0);
    }
}