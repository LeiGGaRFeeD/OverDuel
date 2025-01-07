using UnityEngine;

public class ComputerCowboy : MonoBehaviour
{
    public CowboySettings settings; // Настройки ковбоя из ScriptableObject
    public GameObject bulletPrefab;
    public Transform shootPoint;

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

        hasShot = true; // Устанавливаем начальное состояние
        Invoke(nameof(ResetShootState), 0.1f); // Сбрасываем состояние через небольшой промежуток времени
    }

    void ResetShootState()
    {
        hasShot = false; // Разрешаем выстрел после настройки
    }

    void Update()
    {
        if (waypoints == null || waypoints.Length < 2) return;

        Move();

        if (!hasShot)
        {
            Shoot();
        }
    }

    void Move()
    {
        Transform target = waypoints[currentWaypointIndex];
        transform.position = Vector2.MoveTowards(transform.position, target.position, settings.speed * Time.deltaTime);

        if (Vector2.Distance(transform.position, target.position) < 0.1f)
        {
            currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
        }
    }

    void Shoot()
    {
        if (waypoints == null || waypoints.Length < 2) return; // Проверяем настройки

        Instantiate(bulletPrefab, shootPoint.position, Quaternion.identity);
        hasShot = true; // Устанавливаем флаг после выстрела
        GameManager.Instance.CheckReloadState();
    }

    public void OnHit()
    {
        Destroy(gameObject);
        GameManager.Instance.NextLevel();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        OnHit();
    }
}
