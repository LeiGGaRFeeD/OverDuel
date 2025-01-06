using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    private bool leftPlayerShot = false;
    private bool rightPlayerShot = false;

    public void RegisterShot(bool isLeftCowboy)
    {
        // ”станавливаем значение дл€ каждого игрока в зависимости от того, кто стрел€ет
        if (isLeftCowboy)
        {
            leftPlayerShot = true;
        }
        else
        {
            rightPlayerShot = true;
        }

        // ѕроверка, если оба игрока сделали выстрел, вызываем перезар€дку
        if (leftPlayerShot && rightPlayerShot)
        {
            ReloadBothPlayers();
        }
    }

    private void ReloadBothPlayers()
    {
        Cowboy[] cowboys = FindObjectsOfType<Cowboy>();

        foreach (Cowboy cowboy in cowboys)
        {
            cowboy.Reload();
        }

        // —брос значений выстрелов
        leftPlayerShot = false;
        rightPlayerShot = false;
    }
}
