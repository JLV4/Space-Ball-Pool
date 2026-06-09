using UnityEngine;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    public GameSettings gameSettings; 

    [Header("UI Links")]
    public Slider powerSlider;
    public Slider wallBouncinessSlider;

    public Button randomizeButton;
    public Button defaultButton;

    void Start()
    {
        if (gameSettings == null)
        {
            Debug.LogError("GameSettings asset is not linked in the Inspector!");
            return;
        }

        //Max power slider setup
        powerSlider.minValue = GameSettings.MIN_POWER;
        powerSlider.maxValue = GameSettings.MAX_POWER;
        powerSlider.value = gameSettings.maxShotPower;

        //Wall bounciness slider setup
        wallBouncinessSlider.minValue = 0f;
        wallBouncinessSlider.maxValue = 1f;
        wallBouncinessSlider.value = gameSettings.wallBounciness;

        powerSlider.onValueChanged.AddListener(OnPowerChanged);
        wallBouncinessSlider.onValueChanged.AddListener(OnWallBouncinessChanged);

        //randomizer button listener
        if (randomizeButton != null) {
            randomizeButton.onClick.AddListener(OnRandomize);
        }

        //default button listener
        if (defaultButton != null)
        {
            defaultButton.onClick.AddListener(OnResetDefaults);
        }   
    }


    public void OnWallBouncinessChanged(float value)
    {
        if (gameSettings != null)
            gameSettings.wallBounciness = value;
    }
    public void OnPowerChanged(float value)
    {
        if (gameSettings != null)
            gameSettings.maxShotPower = value;
    }

    public void OnRandomize()
    {
        if (gameSettings != null)
        {
            gameSettings.RandomizeAll();

            powerSlider.value = gameSettings.maxShotPower;
            wallBouncinessSlider.value = gameSettings.wallBounciness;
        }
    }

    /// <summary>
    /// Called by the Default Button.
    /// </summary>
    public void OnResetDefaults()
    {
        if (gameSettings != null)
        {
            gameSettings.ResetToDefaults();
            
            powerSlider.value = gameSettings.maxShotPower;
            wallBouncinessSlider.value = gameSettings.wallBounciness;
        }
    }
}