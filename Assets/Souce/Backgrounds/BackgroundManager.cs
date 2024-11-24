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
        // �������� ��������� ������ ����
        int selectedBackgroundIndex = Random.Range(0, backgrounds.Length);

        // ��������� ��������� ���, ����� ����� ���� ������������ ��� ��������� �������
        PlayerPrefs.SetInt("SelectedBackground", selectedBackgroundIndex);
        PlayerPrefs.Save();

        // ������������� ��������� ���
        SetBackground(selectedBackgroundIndex);
    }

    private void Update()
    {
        // ��������� ������ ���� ������ ����
        if (activeBackground != null)
        {
            ResizeBackgroundToFitCamera(activeBackground);
        }
    }

    private void SetBackground(int index)
    {
        // ���������, ��� ������ ��������� � �������� �������
        if (index >= 0 && index < backgrounds.Length)
        {
            // ������������ ��� ����
            foreach (var bg in backgrounds)
            {
                bg.SetActive(false);
            }

            // ���������� ��������� ���
            activeBackground = backgrounds[index];
            activeBackground.SetActive(true);
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
