using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelPlayerPrefs : MonoBehaviour
{
    public Text CurrentLevel;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        CurrentLevel.text = PlayerPrefs.GetInt("CurrentLevel").ToString();
        if (Input.GetKeyDown(KeyCode.Z))
        {
            PlayerPrefs.SetInt("CurrentLevel", 1);
        }
    }
}
