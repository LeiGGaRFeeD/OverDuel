using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 5f; // Начальная скорость пули
    public int maxRicochets = 3; // Максимальное количество рикошетов
    public float ricochetAngle = 45f; // Угол отклонения при рикошете (в градусах)
    public float ricochetSpeed = 3f; // Скорость пули после рикошета

    private int currentRicochets = 0; // Счётчик рикошетов

    void Update()
    {
        transform.Translate(Vector2.right * speed * Time.deltaTime); // Двигаем пулю
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Computer"))
        {
            ComputerCowboy computer = collision.collider.GetComponent<ComputerCowboy>();
            if (computer != null)
            {
                computer.OnHit();
            }
            Destroy(gameObject); // Уничтожаем пулю после попадания в компьютер
        }
        else if (collision.collider.CompareTag("Player"))
        {
            PlayerCowboy player = collision.collider.GetComponent<PlayerCowboy>();
            if (player != null)
            {
                // Логика попадания в игрока, если требуется
            }
            Destroy(gameObject); // Уничтожаем пулю после попадания в игрока
        }
        else if (collision.collider.CompareTag("Cactus")) // Логика рикошета от кактуса
        {
            Ricochet(collision.contacts[0].normal);
        }
        else
        {
            Destroy(gameObject); // Уничтожение при столкновении с другими объектами
        }
    }

    // Логика рикошета
    public void Ricochet(Vector2 collisionNormal)
    {
        if (currentRicochets >= maxRicochets)
        {
            Destroy(gameObject); // Уничтожаем пулю, если достигнут максимум рикошетов
            return;
        }

        currentRicochets++;

        // Преобразуем угол рикошета из градусов в радианы
        float angle = ricochetAngle * Mathf.Deg2Rad;

        // Рассчитываем новое направление с отклонением
        Vector2 incomingDirection = transform.right; // Текущее направление пули
        Vector2 ricochetDirection = Vector2.Reflect(incomingDirection, collisionNormal).normalized; // Стандартный рикошет

        // Отклоняем направление на заданный угол
        ricochetDirection = Quaternion.Euler(0, 0, ricochetAngle) * ricochetDirection;

        // Обновляем скорость пули после рикошета
        speed = ricochetSpeed;

        // Обновляем направление пули
        transform.right = ricochetDirection; // Направление пули меняется
    }
    public void ModifySpeed(float deltaSpeed)
    {
        speed += deltaSpeed;
    //    speed = Mathf.Max(0, speed); // Убедимся, что скорость не станет отрицательной
    }

}
