using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Unity.Services.Core;
using Unity.Services.Analytics;

public class EndSceneController : MonoBehaviour
{
    public Text LeftResult;
    public Text RightResult;
    public Button playAgainButton;
    public Button secondaryButton; // Вторая кнопка, пока неактивная

    private void Start()
    {
        InitializeUnityServices();

        playAgainButton.onClick.AddListener(PlayAgain);
        secondaryButton.onClick.AddListener(SecondaryAction);

        // Отправляем GameOverEvent
        LogGameOverEvent();
    }

    private void Update()
    {
        LeftResult.text = PlayerPrefs.GetInt("Left").ToString();
        RightResult.text = PlayerPrefs.GetInt("Right").ToString();
    }

    private void PlayAgain()
    {
        // Сброс результатов
        PlayerPrefs.SetInt("Left", 0);
        PlayerPrefs.SetInt("Right", 0);

        // Увеличение количества сыгранных игр
        int totalGames = PlayerPrefs.GetInt("TotalGames", 0) + 1;
        PlayerPrefs.SetInt("TotalGames", totalGames);

        // Проверяем выбранный режим игры
        string gameMode = PlayerPrefs.GetString("GameMode");

        if (gameMode == "2P_Game")
        {
            SceneManager.LoadScene("Game"); // Сцена для режима 2 игрока
        }
        else if (gameMode == "1P_Game")
        {
            SceneManager.LoadScene("Game1P"); // Сцена для режима 1 игрока
        }
        else
        {
            Debug.LogWarning("GameMode is not set or invalid. Defaulting to 'Game'.");
            SceneManager.LoadScene("Game"); // Резервный вариант
        }
    }

    private void SecondaryAction()
    {
        Debug.Log("Secondary button pressed - feature not implemented yet.");
    }

    private async void InitializeUnityServices()
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

    private void LogGameOverEvent()
    {
        if (!UnityServices.State.Equals(ServicesInitializationState.Initialized))
        {
            Debug.LogWarning("Unity Services are not initialized. Event not sent.");
            return;
        }

        // Получаем данные для ивента
        string gameMode = PlayerPrefs.GetString("GameMode", "Unknown");
        int leftScore = PlayerPrefs.GetInt("Left", 0);
        int rightScore = PlayerPrefs.GetInt("Right", 0);
        int totalGames = PlayerPrefs.GetInt("TotalGames", 0);

        // Создаем и отправляем событие GameOverEvent
        GameOverEvent gameOverEvent = new GameOverEvent
        {
            GameMode = gameMode,
            LeftScore = leftScore,
            RightScore = rightScore,
            TotalGamesPlayed = totalGames
        };

        AnalyticsService.Instance.RecordEvent(gameOverEvent);
        Debug.Log($"GameOverEvent sent: Mode={gameMode}, Left={leftScore}, Right={rightScore}, TotalGames={totalGames}");
    }
}

// Определение кастомного события
public class GameOverEvent : Unity.Services.Analytics.Event
{
    public GameOverEvent() : base("GameOverEvent")
    {
    }

    public string GameMode
    {
        set { SetParameter("gameMode", value); }
    }

    public int LeftScore
    {
        set { SetParameter("leftScore", value); }
    }

    public int RightScore
    {
        set { SetParameter("rightScore", value); }
    }

    public int TotalGamesPlayed
    {
        set { SetParameter("totalGamesPlayed", value); }
    }
}
