using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EndSceneController : MonoBehaviour
{
    public Text LeftResult;
    public Text RightResult;
    public Button playAgainButton;
    public Button secondaryButton; // Вторая кнопка, пока неактивная

    private void Start()
    {
        playAgainButton.onClick.AddListener(PlayAgain);
        secondaryButton.onClick.AddListener(SecondaryAction);
    }
    private void Update()
    {
        LeftResult.text = PlayerPrefs.GetInt("Left").ToString();
        RightResult.text = PlayerPrefs.GetInt("Right").ToString() ;
    }
    private void PlayAgain()
    {
        PlayerPrefs.SetInt("Left", 0);
        PlayerPrefs.SetInt("Right", 0);
        SceneManager.LoadScene("Game"); // Название сцены с игрой
    }

    private void SecondaryAction()
    {
        Debug.Log("Secondary button pressed - feature not implemented yet.");
    }
}
