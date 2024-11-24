using System.Collections.Generic;
using UnityEngine;

public class RandomFullScreenImage : MonoBehaviour
{
    [Tooltip("Список GameObject с изображениями.")]
    public List<GameObject> images; // Список картинок

    private void Start()
    {
        if (images == null || images.Count == 0)
        {
            Debug.LogError("Список изображений пуст! Добавьте изображения в инспекторе.");
            return;
        }

        // Выбираем случайное изображение
        GameObject selectedImage = images[Random.Range(0, images.Count)];

        // Включаем выбранное изображение и растягиваем его на весь экран
        EnableAndStretchImage(selectedImage);
    }

    private void EnableAndStretchImage(GameObject image)
    {
        // Включаем изображение
        image.SetActive(true);

        // Проверяем наличие компонента RectTransform
        RectTransform rectTransform = image.GetComponent<RectTransform>();
        if (rectTransform == null)
        {
            Debug.LogError($"Объект {image.name} не является UI элементом (отсутствует RectTransform).");
            return;
        }

        // Растягиваем на весь экран
        rectTransform.anchorMin = Vector2.zero; // Нижний левый угол
        rectTransform.anchorMax = Vector2.one;  // Верхний правый угол
        rectTransform.offsetMin = Vector2.zero; // Убираем смещение снизу
        rectTransform.offsetMax = Vector2.zero; // Убираем смещение сверху
    }
}
