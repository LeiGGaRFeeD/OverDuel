using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    private bool leftPlayerShot = false;
    private bool rightPlayerShot = false;

    public void RegisterShot(bool isLeftCowboy)
    {
        // ������������� �������� ��� ������� ������ � ����������� �� ����, ��� ��������
        if (isLeftCowboy)
        {
            leftPlayerShot = true;
        }
        else
        {
            rightPlayerShot = true;
        }

        // ��������, ���� ��� ������ ������� �������, �������� �����������
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

        // ����� �������� ���������
        leftPlayerShot = false;
        rightPlayerShot = false;
    }
}
