using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundManager : MonoBehaviour
{
    public GameObject[] backgrounds;          // Массив фонов
    public Camera mainCamera;                 // Камера, к которой подстраивается фон
    public int levelInterval = 5;             // Интервал уровней для смены фона

    private GameObject activeBackground;      // Текущий активный фон
    private int lastLevelChecked = -1;        // Последний проверенный уровень
    private int lastSelectedBackground = -1;  // Индекс последнего выбранного фона

    private void Start()
    {
        // Инициализация последнего активного уровня
        lastLevelChecked = PlayerPrefs.GetInt("CurrentLevel", 1);

        // Устанавливаем фон на старте
        int savedBackgroundIndex = PlayerPrefs.GetInt("SelectedBackground", 0);
        SetBackground(savedBackgroundIndex);
    }

    private void Update()
    {
        // Проверяем текущий уровень
        int currentLevel = PlayerPrefs.GetInt("CurrentLevel", 1);

        // Если уровень изменился и кратен levelInterval, сменить фон
        if (currentLevel != lastLevelChecked && currentLevel % levelInterval == 0)
        {
            int newBackgroundIndex = GetRandomBackgroundIndex();

            // Сохраняем новый выбранный фон
            PlayerPrefs.SetInt("SelectedBackground", newBackgroundIndex);
            PlayerPrefs.Save();

            // Меняем фон
            SetBackground(newBackgroundIndex);
        }

        // Обновляем последний проверенный уровень
        lastLevelChecked = currentLevel;

        // Обновляем размер активного фона
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
