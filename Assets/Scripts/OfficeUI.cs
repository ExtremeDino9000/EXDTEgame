using UnityEngine;
using TMPro; // Crucial for talking to TextMeshPro elements

public class OfficeUI : MonoBehaviour
{
    [Header("References")]
    public OfficeTemperature tempSystem; // Drag your object with the OfficeTemperature script here
    public TextMeshProUGUI temperatureText; // Drag your TemperatureText UI object here

    void Update()
    {
        if (tempSystem != null && temperatureText != null)
        {
            // Get the current temperature from the system
            float currentTemp = tempSystem.GetTemperature();

            // Round it to a whole number so it doesn't show crazy decimals (e.g., 14.5323C)
            int roundedTemp = Mathf.RoundToInt(currentTemp);

            // Update the text on the player's screen
            temperatureText.text = "TEMP: " + roundedTemp + "°C";

            // OPTIONAL COLOR CHANGE: Turn text red if Foxy is getting aggressive (> 10°C)
            if (currentTemp > 10f)
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