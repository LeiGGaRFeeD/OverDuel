using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxSpawner : MonoBehaviour
{
    public GameObject boxPrefab;                // Префаб коробки
    public int activeLevel = 1;                 // Уровень, на котором спавнер активен
    public float spawnInterval = 2f;            // Интервал спавна коробок
    public int maxBoxes = 10;                   // Максимальное количество коробок на экране
    public Transform spawnArea;                 // Трансформ области спавна
    public LayerMask boxLayer;                  // Слой, соответствующий коробкам
    public float boxRadius = 0.5f;              // Радиус проверки наложений для коробок

    private bool isSpawning = false;            // Включен ли спавн
    private Coroutine spawnCoroutine;           // Для хранения ссылки на корутину спавна
    private List<GameObject> spawnedBoxes = new List<GameObject>(); // Список всех заспавненных коробок

    private void Start()
    {
        CheckSpawnerStatus();
    }

    private void Update()
    {
        // Проверка активации спавнера в зависимости от текущего уровня
        CheckSpawnerStatus();
    }

    private void CheckSpawnerStatus()
    {
        // Получаем текущий уровень (например, из системы уровней)
        int currentLevel = GetCurrentLevel(); // Вставьте здесь логику получения уровня

        if (currentLevel >= activeLevel && !isSpawning)
        {
            StartSpawning();
        }
        else if (currentLevel < activeLevel && isSpawning)
        {
            StopSpawning();
        }
    }

    private void StartSpawning()
    {
        isSpawning = true;
        spawnCoroutine = StartCoroutine(SpawnBoxes());
    }

    private void StopSpawning()
    {
        isSpawning = false;
        if (spawnCoroutine != null)
        {
            StopCoroutine(spawnCoroutine);
            spawnCoroutine = null;
        }

        // Очистка списка и уничтожение всех заспавненных коробок
        foreach (var box in spawnedBoxes)
        {
            if (box != null)
            {
                Destroy(box);
            }
        }
        spawnedBoxes.Clear();
    }

    private IEnumerator SpawnBoxes()
    {
        while (isSpawning)
        {
            // Проверка на максимальное количество коробок
            if (spawnedBoxes.Count < maxBoxes)
            {
                Vector2 spawnPosition = GetRandomPositionInSpawnArea();

                // Проверка на наложение с другими коробками
                if (!Physics2D.OverlapCircle(spawnPosition, boxRadius, boxLayer))
                {
                    GameObject newBox = Instantiate(boxPrefab, spawnPosition, Quaternion.identity);
                    spawnedBoxes.Add(newBox);
                }
            }

            yield return new WaitForSeconds(spawnInterval);
        }
    }

    private Vector2 GetRandomPositionInSpawnArea()
    {
        // Получаем размеры области спавна
        Vector2 areaSize = spawnArea.localScale / 2f;

        // Случайная позиция внутри области
        float x = Random.Range(spawnArea.position.x - areaSize.x, spawnArea.position.x + areaSize.x);
        float y = Random.Range(spawnArea.position.y - areaSize.y, spawnArea.position.y + areaSize.y);

        return new Vector2(x, y);
    }

    private int GetCurrentLevel()
    {
        // Здесь нужно указать способ получения текущего уровня
        return PlayerPrefs.GetInt("CurrentLevel", 1); // Замените при необходимости
    }
}
