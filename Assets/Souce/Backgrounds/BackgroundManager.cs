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
        int selectedBackgroundIndex = PlayerPrefs.GetInt("SelectedBackground", 0);
        SetBackground(selectedBackgroundIndex);
    }

    private void SetBackground(int index)
    {
        // Убедитесь, что индекс находится в пределах массива
        if (index >= 0 && index < backgrounds.Length)
        {
            // Удаляем предыдущий фон, если он существует
            if (activeBackground != null)
            {
                Destroy(activeBackground);
            }

            // Создаем новый фон
            activeBackground = Instantiate(backgrounds[index], Vector3.zero, Quaternion.identity);

            // Настраиваем размер фона под камеру
            ResizeBackgroundToFitCamera(activeBackground);
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
