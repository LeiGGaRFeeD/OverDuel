using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UILoseV2 : MonoBehaviour
{
    [SerializeField] private Text _text;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    public void GoToR()
    {
        PlayerPrefs.SetInt("CurrentLevel",0);
        SceneManager.LoadScene("Game1P");
    }

    // Update is called once per frame
    void Update()
    {
        _text.text = PlayerPrefs.GetInt("CurrentLevel").ToString();
    }
}
