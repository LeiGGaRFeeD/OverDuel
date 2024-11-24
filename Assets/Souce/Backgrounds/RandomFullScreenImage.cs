using System.Collections.Generic;
using UnityEngine;

public class RandomFullScreenImage : MonoBehaviour
{
    [Tooltip("������ GameObject � �������������.")]
    public List<GameObject> images; // ������ ��������

    private void Start()
    {
        if (images == null || images.Count == 0)
        {
            Debug.LogError("������ ����������� ����! �������� ����������� � ����������.");
            return;
        }

        // �������� ��������� �����������
        GameObject selectedImage = images[Random.Range(0, images.Count)];

        // �������� ��������� ����������� � ����������� ��� �� ���� �����
        EnableAndStretchImage(selectedImage);
    }

    private void EnableAndStretchImage(GameObject image)
    {
        // �������� �����������
        image.SetActive(true);

        // ��������� ������� ���������� RectTransform
        RectTransform rectTransform = image.GetComponent<RectTransform>();
        if (rectTransform == null)
        {
            Debug.LogError($"������ {image.name} �� �������� UI ��������� (����������� RectTransform).");
            return;
        }

        // ����������� �� ���� �����
        rectTransform.anchorMin = Vector2.zero; // ������ ����� ����
        rectTransform.anchorMax = Vector2.one;  // ������� ������ ����
        rectTransform.offsetMin = Vector2.zero; // ������� �������� �����
        rectTransform.offsetMax = Vector2.zero; // ������� �������� ������
    }
}
