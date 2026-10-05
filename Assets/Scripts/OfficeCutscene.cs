using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.InputSystem;

public class OfficeCutscene : MonoBehaviour
{
    [Header("Cutscene")]
    public PlayableDirector director;
    public GameObject cutsceneCamera;
    public GameObject cutsceneCanvas;
    public GameObject cutsceneMusic;

    [Header("Controls")]
    public Key skipKey = Key.Space;

    [Header("Managers")]
    public GameObject manager1;
    public GameObject manager2;
    public GameObject manager3;

    [Header("Gameplay")]
    public GameObject normalMusic;
    public GameObject officeCanvas;

    private bool isFinished = false;

    void Start()
    {
        isFinished = false;

        Time.timeScale = 1f;

        if (manager1 != null) manager1.SetActive(false);
        if (manager2 != null) manager2.SetActive(false);
        if (manager3 != null) manager3.SetActive(false);
        if (normalMusic != null) normalMusic.SetActive(false);
        if (officeCanvas != null) officeCanvas.SetActive(false);

        if (cutsceneCamera != null) cutsceneCamera.SetActive(true);
        if (cutsceneCanvas != null) cutsceneCanvas.SetActive(true);
        if (cutsceneMusic != null) cutsceneMusic.SetActive(true);

        if (director != null)
        {
            director.timeUpdateMode = DirectorUpdateMode.UnscaledGameTime;
            director.stopped += CutsceneFinished;
            director.Play();
        }
    }

    void Update()
    {
        if (!isFinished && Keyboard.current != null && Keyboard.current[skipKey].wasPressedThisFrame)
        {
            if (director != null)
            {
                director.Stop();
            }
            else
            {
                CutsceneFinished(null);
            }
        }
    }

    void CutsceneFinished(PlayableDirector d)
    {
        if (isFinished) return;
        isFinished = true;

        // Safely disable cutscene systems
        if (cutsceneCamera != null) cutsceneCamera.SetActive(false);
        if (cutsceneCanvas != null) cutsceneCanvas.SetActive(false);
        if (cutsceneMusic != null) cutsceneMusic.SetActive(false);

        // Safely re-enable gameplay systems
        if (manager1 != null) manager1.SetActive(true);
        if (manager2 != null) manager2.SetActive(true);
        if (manager3 != null) manager3.SetActive(true);

        if (normalMusic != null) normalMusic.SetActive(true);
        if (officeCanvas != null) officeCanvas.SetActive(true);

        // Stop listening for event
        if (director != null)
        {
            director.stopped -= CutsceneFinished;
        }
    }
}