using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundManager : MonoBehaviour
{
    public GameObject[] backgrounds;          // ������ �����
    public Camera mainCamera;                 // ������, � ������� �������������� ���

    private GameObject activeBackground;      // ������� �������� ���

    private void Start()
    {
        int selectedBackgroundIndex = PlayerPrefs.GetInt("SelectedBackground", 0);
        SetBackground(selectedBackgroundIndex);
    }

    private void SetBackground(int index)
    {
        // ���������, ��� ������ ��������� � �������� �������
        if (index >= 0 && index < backgrounds.Length)
        {
            // ������� ���������� ���, ���� �� ����������
            if (activeBackground != null)
            {
                Destroy(activeBackground);
            }

            // ������� ����� ���
            activeBackground = Instantiate(backgrounds[index], Vector3.zero, Quaternion.identity);

            // ����������� ������ ���� ��� ������
            ResizeBackgroundToFitCamera(activeBackground);
        }
        else
        {
            Debug.LogWarning("������ ���������� ���� ��� ��������� �������.");
        }
    }

    private void ResizeBackgroundToFitCamera(GameObject background)
    {
        SpriteRenderer spriteRenderer = background.GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            float cameraHeight = mainCamera.orthographicSize * 2f;
            float cameraWidth = cameraHeight * mainCamera.aspect;

            Vector2 backgroundSize = spriteRenderer.sprite.bounds.size;
            background.transform.localScale = new Vector3(cameraWidth / backgroundSize.x, cameraHeight / backgroundSize.y, 1);
        }
        else
        {
            Debug.LogWarning("� ���� ����������� SpriteRenderer.");
        }
    } 
}
