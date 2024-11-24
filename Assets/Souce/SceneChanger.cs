using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Unity.Services.Core;
using Unity.Services.Analytics;

public class SceneChanger : MonoBehaviour
{
    private async void Start()
    {
        await InitializeUnityServices();
        UnityServices.InitializeAsync();
        AnalyticsService.Instance.StartDataCollection();
    }

    public void GoTo1PGame()
    {
        PlayerPrefs.SetInt("Left", 0);
        PlayerPrefs.SetInt("Right", 0);
        PlayerPrefs.SetString("GameMode", "1P_Game");
        LogGameModeSelection("1P_Game");
        SceneManager.LoadScene("Game1P");
    }

    public void GoTo2PGame()
    {
        PlayerPrefs.SetInt("Left", 0);
        PlayerPrefs.SetInt("Right", 0);
        PlayerPrefs.SetString("GameMode", "2P_Game");
        LogGameModeSelection("2P_Game");
        SceneManager.LoadScene("Game");
    }

    public void GoToMenuMain()
    {
        PlayerPrefs.SetInt("Left", 0);
        PlayerPrefs.SetInt("Right", 0);
        LogGameModeSelection("MainMenu");
        SceneManager.LoadScene("MainMenu");
    }

    private async System.Threading.Tasks.Task InitializeUnityServices()
    {
        try
        {
            await UnityServices.InitializeAsync();
            Debug.Log("Unity Services initialized successfully.");
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"Unity Services initialization failed: {ex.Message}");
        }
    }

    private void LogGameModeSelection(string gameMode)
    {
        if (!UnityServices.State.Equals(ServicesInitializationState.Initialized))
        {
            Debug.LogWarning("Unity Services are not initialized. Event not sent.");
            return;
        }

        // Создание и отправка события
        AnalyticsEvent gameModeEvent = new AnalyticsEvent
        {
            SelectedGameMode = gameMode
        };

        AnalyticsService.Instance.RecordEvent(gameModeEvent);
        Debug.Log($"GameModeSelect event sent: {gameMode}");
    }
}

// Определение кастомного события
public class AnalyticsEvent : Unity.Services.Analytics.Event
{
    public AnalyticsEvent() : base("GameModeSelect")
    {
    }

    public string SelectedGameMode
    {
        set { SetParameter("selectedGameMode", value); }
    }
}
