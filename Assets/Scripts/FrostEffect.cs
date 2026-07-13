using UnityEngine;
using UnityEngine.UI;

public class FrostEffect : MonoBehaviour
{
    public OfficeTemperature tempSystem; // Reference to your temperature script
    public Image frostImage;             // Drag your Frost_Overlay here

    public float frostStartTemp = 5f;    // Temperature where frost starts appearing
    public float maximumFrostTemp = -5f; // Temperature where the screen is completely frozen

    void Update()
    {
        if (tempSystem == null || frostImage == null) return;

        float currentTemp = tempSystem.GetTemperature();

        if (currentTemp < frostStartTemp)
        {
            // Calculate how far into the freezing zone we are (0.0 to 1.0)
            float totalRange = frostStartTemp - maximumFrostTemp;
            float currentProgress = frostStartTemp - currentTemp;
            
            float targetAlpha = Mathf.Clamp01(currentProgress / totalRange);

            // Apply the alpha to the frost image color smoothly
            Color c = frostImage.color;
            c.a = targetAlpha;
            frostImage.color = c;
        }
        else
        {
            // If it's warm enough, make sure the frost is completely gone
            Color c = frostImage.color;
            c.a = 0f;
            frostImage.color = c;
        }
    }
}