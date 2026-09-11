using UnityEngine;
using TMPro;

public class OfficeUI : MonoBehaviour
{
    [Header("References")]
    public OfficeTemperature tempSystem;
    public TextMeshProUGUI temperatureText;

    void Update()
    {
        if (tempSystem != null && temperatureText != null)
        {
            // Get the current temperature from the system
            float currentTemp = tempSystem.GetTemperature();

            // Round it to a whole number so it doesn't show decimals
            int roundedTemp = Mathf.RoundToInt(currentTemp);

            // Update the text on the player's screen
            temperatureText.text = "TEMP: " + roundedTemp + "°C";

            // Turn text red if Foxy is getting aggressive (> 20°C)
            if (currentTemp > 20f)
            {
                temperatureText.color = Color.red;
            }
            else
            {
                temperatureText.color = Color.white;
            }
        }
    }
}