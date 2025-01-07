using UnityEngine;

public class ComputerCowboy : MonoBehaviour
{
    public CowboySettings settings; // Настройки ковбоя из ScriptableObject
    public GameObject bulletPrefab; // Префаб пули
    public Transform shootPoint; // Точка стрельбы

    private Transform[] waypoints; // Массив точек, между которыми движется ковбой
    private int currentWaypointIndex = 0; // Индекс текущей точки
    public bool hasShot = false; // Отслеживает, стрелял ли компьютер

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

        Instantiate(bulletPrefab, shootPoint.position, Quaternion.identity); // Создаём пулю
        hasShot = true; // Устанавливаем флаг "стрельбы"
        GameManager.Instance.CheckReloadState(); // Проверяем состояние перезарядки
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
    }
}
