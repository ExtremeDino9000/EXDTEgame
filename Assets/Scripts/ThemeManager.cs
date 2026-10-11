using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using TMPro;

public class ThemeManager : MonoBehaviour
{
    [Header("UI & Video Components")]
    public RawImage backgroundDisplay; 
    public VideoPlayer videoPlayer;
    public TextMeshProUGUI themeButtonText;

    [Header("Audio Components")]
    public AudioSource menuAudioSource;
    public AudioClip officeMusic;
    public AudioClip voidMusic;
    public AudioClip remasteredMusic; 

    [Header("Theme Assets")]
    public VideoClip officeVideoClip; 
    public VideoClip remasteredClip; 
    public Texture voidThemeTexture; 

    [Header("State")]
    public string currentTheme = "Office";

    void Start()
    {
        currentTheme = PlayerPrefs.GetString("SelectedTheme", "Office");
        ApplyTheme(currentTheme);
    }

    public void ToggleTheme()
    {
        Debug.Log($"ToggleTheme pressed. Current theme was: {currentTheme}");

        if (currentTheme == "Office")
        {
            currentTheme = "Remastered";
        }
        else if (currentTheme == "Remastered")
        {
            currentTheme = "Void";
        }
        else
        {
            currentTheme = "Office";
        }

        ApplyTheme(currentTheme);
    }

    public void ApplyTheme(string themeName)
    {
        currentTheme = themeName;
        Debug.Log($"Applying theme: {currentTheme}");

        if (currentTheme == "Void")
        {
            if (videoPlayer != null && videoPlayer.isPlaying) videoPlayer.Stop();
            if (backgroundDisplay != null && voidThemeTexture != null) 
            {
                backgroundDisplay.texture = voidThemeTexture;
            }

            SwitchMusic(voidMusic);
        }
        else if (currentTheme == "Remastered")
        {
            PlayVideoTheme(remasteredClip);
            SwitchMusic(remasteredMusic);
        }
        else
        {
            currentTheme = "Office";
            PlayVideoTheme(officeVideoClip);
            SwitchMusic(officeMusic);
        }

        if (themeButtonText != null)
        {
            themeButtonText.text = currentTheme.ToUpper();
        }

        PlayerPrefs.SetString("SelectedTheme", currentTheme);
        PlayerPrefs.Save();
    }

    private void PlayVideoTheme(VideoClip clip)
    {
        if (clip == null)
        {
            Debug.LogWarning($"ThemeManager: VideoClip is missing for theme {currentTheme}!");
            return;
        }

        if (videoPlayer != null)
        {
            videoPlayer.Stop();
            videoPlayer.clip = clip;

            if (videoPlayer.targetTexture != null && backgroundDisplay != null)
            {
                backgroundDisplay.texture = videoPlayer.targetTexture;
            }

            videoPlayer.Play();
        }
        else
        {
            Debug.LogWarning("ThemeManager: VideoPlayer reference is missing in the Inspector!");
        }
    }

    private void SwitchMusic(AudioClip newClip)
    {
        if (menuAudioSource == null)
        {
            Debug.LogWarning("ThemeManager: menuAudioSource reference is missing!");
            return;
        }

        if (newClip == null)
        {
            Debug.LogWarning($"ThemeManager: AudioClip missing for theme {currentTheme}!");
            return;
        }

        if (menuAudioSource.clip == newClip && menuAudioSource.isPlaying) return;

        menuAudioSource.Stop();
        menuAudioSource.clip = newClip;
        menuAudioSource.loop = true;
        menuAudioSource.Play();
    }
}