using UnityEngine;

public class PlayerCowboy : MonoBehaviour
{
    public Transform pointA;
    public Transform pointB;
    public float speed = 2f;
    public GameObject bulletPrefab;
    public Transform shootPoint;

    private bool movingToB = true;
    public bool hasShot = false; // Отслеживает, стрелял ли игрок
    public int ammoCount = 5; // Количество патронов

    public AudioClip shootSound; // Звук выстрела
    private AudioSource audioSource; // Компонент AudioSource

    void Start()
    {
        // Получаем или добавляем компонент AudioSource
        audioSource = gameObject.GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    void Update()
    {
        // Если hasShot true, игрок меняет направление при нажатии на Space
        if (hasShot && Input.GetKeyDown(KeyCode.Space))
        {
            ChangeDirection();
        }
        // Если hasShot false, игрок стреляет при нажатии на Space
        else if (!hasShot && Input.GetKeyDown(KeyCode.Space))
        {
            Shoot();
        }

        // Движение игрока
        Move();
    }
    public void ForButtonShoot()
    {
        // Если hasShot true, игрок меняет направление при нажатии на Space
        if (hasShot )
        {
            ChangeDirection();
        }
        // Если hasShot false, игрок стреляет при нажатии на Space
        else if (!hasShot )
        {
            Shoot();
        }

        // Движение игрока
        Move();
    }

    void Move()
    {
        Transform target = movingToB ? pointB : pointA;
        transform.position = Vector2.MoveTowards(transform.position, target.position, speed * Time.deltaTime);

        if (Vector2.Distance(transform.position, target.position) < 0.1f)
        {
            movingToB = !movingToB;
        }
    }

    void Shoot()
    {
        if (ammoCount > 0) // Проверка на наличие патронов
        {
            Instantiate(bulletPrefab, shootPoint.position, Quaternion.identity);
            hasShot = true; // Устанавливаем, что игрок сделал выстрел
            ammoCount--; // Уменьшаем количество патронов
            PlayShootSound(); // Воспроизводим звук выстрела
            GameManager.Instance.CheckReloadState();
        }
    }

    void PlayShootSound()
    {
        if (shootSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(shootSound); // Воспроизводим звук
        }
        else
        {
            Debug.LogWarning("Отсутствует звук или компонент AudioSource!");
        }
    }

    // Метод для смены направления движения
    void ChangeDirection()
    {
        movingToB = !movingToB;
    }

    // Метод для перезарядки (вызывается GameManager'ом)
    public void Reload(int newAmmo)
    {
        ammoCount = newAmmo; // Восстанавливаем патроны
        hasShot = false; // Сбрасываем флаг выстрела
    }
}
