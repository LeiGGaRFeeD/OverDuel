using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DirectionButton : MonoBehaviour
{
    public Cowboy cowboy; // Ссылка на скрипт Cowboy
    public Button changeDirectionButton; // Ссылка на кнопку UI

    private void Start()
    {
        // Добавляем слушатель события к кнопке
        if (changeDirectionButton != null)
        {
            changeDirectionButton.onClick.AddListener(ChangeDirection);
        }
    }

    private void ChangeDirection()
    {
        // Меняем направление ковбоя
        cowboy.AndroidMode();
    }
}
