using UnityEngine;

public class ComputerCowboy : MonoBehaviour
{
    public CowboySettings settings; // Настройки ковбоя из ScriptableObject
    public GameObject bulletPrefab; // Префаб пули
    public Transform shootPoint; // Точка стрельбы
    public AudioClip shootSound; // Звук выстрела
    private AudioSource audioSource; // Компонент AudioSource

    private Transform[] waypoints; // Массив точек, между которыми движется ковбой
    private int currentWaypointIndex = 0; // Индекс текущей точки
    public bool hasShot = false; // Отслеживает, стрелял ли компьютер
    public bool isBoss = false; // Флаг, указывающий, является ли ковбой боссом

    public float speedIncreasePercentage = 10f; // Процент увеличения скорости каждые 5 уровней

    void Start()
    {
        if (GameManager.Instance != null)
        {
            waypoints = GameManager.Instance.GetComputerWaypoints(); // Получаем точки из GameManager
        }

        if (waypoints == null || waypoints.Length < 2)
        {
            Debug.LogError("Waypoints для компьютера не настроены!");
            return;
        }

        hasShot = true; // Устанавливаем начальное состояние, чтобы избежать двойного выстрела
        Invoke(nameof(ResetShootState), 0.1f); // Сбрасываем состояние через небольшой промежуток времени

        UpdateSpeedBasedOnLevel(); // Обновляем скорость на основе текущего уровня

        // Получаем или добавляем AudioSource
        audioSource = gameObject.GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    // Сброс состояния "стрельбы"
    void ResetShootState()
    {
        hasShot = false;
    }

    void Update()
    {
        if (waypoints == null || waypoints.Length < 2) return;

        Move(); // Движение между точками

        if (!hasShot)
        {
            Shoot(); // Выполняем выстрел, если не стреляли
        }
    }

    // Движение между точками
    void Move()
    {
        if (settings == null)
        {
            Debug.LogError("Настройки ковбоя не установлены!");
            return;
        }

        Transform target = waypoints[currentWaypointIndex];
        transform.position = Vector2.MoveTowards(transform.position, target.position, settings.speed * Time.deltaTime);

        Debug.Log($"Current Speed: {settings.speed}"); // Отладка скорости

        // Если достигли текущей точки, переключаемся на следующую
        if (Vector2.Distance(transform.position, target.position) < 0.1f)
        {
            currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
        }
    }

    // Стрельба
    void Shoot()
    {
        if (settings == null)
        {
            Debug.LogError("Настройки ковбоя не установлены!");
            return;
        }

        PlayShootSound(); // Воспроизводим звук выстрела

        // Выстрел для обычного ковбоя
        if (!isBoss)
        {
            Instantiate(bulletPrefab, shootPoint.position, Quaternion.identity); // Создаём пулю
        }
        else
        {
            // Двойной выстрел для босса
            Instantiate(bulletPrefab, shootPoint.position, Quaternion.identity); // Первый выстрел
            Invoke(nameof(BossSecondShot), 0.2f); // Второй выстрел через 0.2 секунды
        }

        hasShot = true; // Устанавливаем флаг "стрельбы"
        GameManager.Instance.CheckReloadState(); // Проверяем состояние перезарядки
    }

    // Второй выстрел для босса
    void BossSecondShot()
    {
        Instantiate(bulletPrefab, shootPoint.position, Quaternion.identity); // Второй выстрел
    }

    // Метод воспроизведения звука выстрела
    void PlayShootSound()
    {
        if (shootSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(shootSound); // Воспроизводим звук
        }
        else
        {
            Debug.LogWarning("Отсутствует звук выстрела или AudioSource!");
        }
    }

    // Метод вызывается при попадании
    public void OnHit()
    {
        Destroy(gameObject); // Уничтожаем объект
        GameManager.Instance.NextLevel(); // Переход к следующему уровню
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        OnHit(); // Если произошло столкновение, обрабатываем попадание
    }

    // Метод для обновления настроек ковбоя
    public void UpdateSettings(CowboySettings newSettings)
    {
        settings = newSettings;
        UpdateSpeedBasedOnLevel(); // Обновляем скорость при изменении настроек
    }

    // Обновляем скорость ковбоя на основе текущего уровня
    private void UpdateSpeedBasedOnLevel()
    {
        int currentLevel = PlayerPrefs.GetInt("CurrentLevel", 1); // Получаем текущий уровень из PlayerPrefs

        // Каждые 5 уровней увеличиваем скорость
        if (currentLevel % 5 == 0)
        {
            float multiplier = 1 + (speedIncreasePercentage / 100f);
            settings.speed *= multiplier;
            Debug.Log($"Speed increased! New Speed: {settings.speed}");
        }
    }
}
