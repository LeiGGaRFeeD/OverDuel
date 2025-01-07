using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundManager : MonoBehaviour
{
    public GameObject[] backgrounds;          // ������ �����
    public Camera mainCamera;                 // ������, � ������� �������������� ���
    public int levelInterval = 5;             // �������� ������� ��� ����� ����

    private GameObject activeBackground;      // ������� �������� ���
    private int lastLevelChecked = -1;        // ��������� ����������� �������
    private int lastSelectedBackground = -1;  // ������ ���������� ���������� ����

    private void Start()
    {
        // ������������� ���������� ��������� ������
        lastLevelChecked = PlayerPrefs.GetInt("CurrentLevel", 1);

        // ������������� ��� �� ������
        int savedBackgroundIndex = PlayerPrefs.GetInt("SelectedBackground", 0);
        SetBackground(savedBackgroundIndex);
    }

    private void Update()
    {
        // ��������� ������� �������
        int currentLevel = PlayerPrefs.GetInt("CurrentLevel", 1);

        // ���� ������� ��������� � ������ levelInterval, ������� ���
        if (currentLevel != lastLevelChecked && currentLevel % levelInterval == 0)
        {
            int newBackgroundIndex = GetRandomBackgroundIndex();

            // ��������� ����� ��������� ���
            PlayerPrefs.SetInt("SelectedBackground", newBackgroundIndex);
            PlayerPrefs.Save();

            // ������ ���
            SetBackground(newBackgroundIndex);
        }

        // ��������� ��������� ����������� �������
        lastLevelChecked = currentLevel;

        // ��������� ������ ��������� ����
        if (activeBackground != null)
        {
            ResizeBackgroundToFitCamera(activeBackground);
        }
    }

    private int GetRandomBackgroundIndex()
    {
        int randomIndex;

        do
        {
            randomIndex = Random.Range(0, backgrounds.Length);
        }
        while (randomIndex == lastSelectedBackground);

        lastSelectedBackground = randomIndex;
        return randomIndex;
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
