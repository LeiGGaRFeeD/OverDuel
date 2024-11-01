using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScoreManager : MonoBehaviour
{
    public int targetScore = 10; // Цель для перехода на другую сцену
    private int currentScore = 0;

    // Метод для увеличения счёта, вызывается при попадании
    public void AddScore(int points)
    {
        currentScore += points;

    }
    private void Update()
    {

        if (PlayerPrefs.GetInt("Left") >= targetScore)
        {
            LoadNextScene();
        }
        if (PlayerPrefs.GetInt("Right") >= targetScore)
        {
            LoadNextScene();
        }
    }
    private void LoadNextScene()
    {
        SceneManager.LoadScene("ResultScene"); // Замените "NextScene" на название вашей сцены
    }
}
