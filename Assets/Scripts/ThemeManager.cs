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

    [Header("Theme Assets")]
    public VideoClip officeVideoClip;  
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
        currentTheme = (currentTheme == "Office") ? "Void" : "Office";
        ApplyTheme(currentTheme);
    }

    public void ApplyTheme(string themeName)
    {
        currentTheme = themeName;

        if (currentTheme == "Void")
        {
            if (videoPlayer != null) videoPlayer.Stop();
            if (backgroundDisplay != null) backgroundDisplay.texture = voidThemeTexture;

            SwitchMusic(voidMusic);
        }
        else
        {
            if (videoPlayer != null && officeVideoClip != null)
            {
                videoPlayer.clip = officeVideoClip;
                
                if (videoPlayer.targetTexture != null && backgroundDisplay != null)
                {
                    backgroundDisplay.texture = videoPlayer.targetTexture;
                }
                
                videoPlayer.Play();
            }

            SwitchMusic(officeMusic);
        }

        if (themeButtonText != null)
        {
            themeButtonText.text = currentTheme.ToUpper();
        }

        PlayerPrefs.SetString("SelectedTheme", currentTheme);
        PlayerPrefs.Save();
    }

    private void SwitchMusic(AudioClip newClip)
    {
        if (menuAudioSource == null || newClip == null) return;

        if (menuAudioSource.clip == newClip && menuAudioSource.isPlaying) return;

        menuAudioSource.Stop();
        menuAudioSource.clip = newClip;
        menuAudioSource.loop = true;
        menuAudioSource.Play();
    }
}