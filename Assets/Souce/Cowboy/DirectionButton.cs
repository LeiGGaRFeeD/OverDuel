using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DirectionButton : MonoBehaviour
{
    public Cowboy cowboy; // ������ �� ������ Cowboy
    public Button changeDirectionButton; // ������ �� ������ UI

    private void Start()
    {
        // ��������� ��������� ������� � ������
        if (changeDirectionButton != null)
        {
            changeDirectionButton.onClick.AddListener(ChangeDirection);
        }
    }

    private void ChangeDirection()
    {
        // ������ ����������� ������
        cowboy.AndroidMode();
    }
}
