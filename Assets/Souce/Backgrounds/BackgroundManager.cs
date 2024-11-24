using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundManager : MonoBehaviour
{
    public GameObject[] backgrounds;          // Массив фонов
    public Camera mainCamera;                 // Камера, к которой подстраивается фон

    private GameObject activeBackground;      // Текущий активный фон

    private void Start()
    {
        // Выбираем случайный индекс фона
        int selectedBackgroundIndex = Random.Range(0, backgrounds.Length);

        // Сохраняем выбранный фон, чтобы можно было использовать при следующем запуске
        PlayerPrefs.SetInt("SelectedBackground", selectedBackgroundIndex);
        PlayerPrefs.Save();

        // Устанавливаем случайный фон
        SetBackground(selectedBackgroundIndex);
    }

    private void Update()
    {
        // Обновляем размер фона каждый кадр
        if (activeBackground != null)
        {
            ResizeBackgroundToFitCamera(activeBackground);
        }
    }

    private void SetBackground(int index)
    {
        // Убедитесь, что индекс находится в пределах массива
        if (index >= 0 && index < backgrounds.Length)
        {
            // Деактивируем все фоны
            foreach (var bg in backgrounds)
            {
                bg.SetActive(false);
            }

            // Активируем выбранный фон
            activeBackground = backgrounds[index];
            activeBackground.SetActive(true);
        }
        else
        {
            Debug.LogWarning("Индекс выбранного фона вне диапазона массива.");
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
            Debug.LogWarning("У фона отсутствует SpriteRenderer.");
        }
    }
}
