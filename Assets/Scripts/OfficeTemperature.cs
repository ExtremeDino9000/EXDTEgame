using UnityEngine;

public class OfficeTemperature : MonoBehaviour
{
    [Header("Temperature Settings")]
    public float currentTemperature = 15f; // Starts at 15°C
    public float heatSpeed = 1.5f;        // How fast it heats up when heater is ON
    public float coolSpeed = 2f;          // How fast it cools down when heater is OFF
    public float minTemperature = 5f;     // Cannot go lower than 5°C

    private bool isHeaterOn = true; // Started as ON by default

    void Update()
    {
        if (isHeaterOn)
        {
            // Heat up the room
            currentTemperature += heatSpeed * Time.deltaTime;
        }
        else
        {
            // Cool down the room
            currentTemperature -= coolSpeed * Time.deltaTime;
            if (currentTemperature < minTemperature)
            {
                currentTemperature = minTemperature;
            }
        }
    }

    // Call this function when the player clicks the "Turn Off Heater" button on the camera
    // Replace your old ToggleHeater function with this one:
    // Replace your old ToggleHeater function with this exact code:
    public void ToggleHeater()
    {
        isHeaterOn = !isHeaterOn; // Automatically flips true to false, or false to true
        Debug.Log("Heater toggled! Is it running now? " + isHeaterOn);
    }

    public float GetTemperature()
    {
        return currentTemperature;
    }

    public bool IsHeaterOn()
    {
        return isHeaterOn;
    }
}