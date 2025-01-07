using UnityEngine;

public class Box : MonoBehaviour
{
    public float bulletSpeedReduction = 2f; // На сколько уменьшить скорость пули

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Bullet"))
        {
            Bullet bullet = collision.GetComponent<Bullet>();
            if (bullet != null)
            {
                // Замедляем скорость пули
                bullet.ModifySpeed(-bulletSpeedReduction);

                // Уничтожаем коробку
                Destroy(gameObject);
            }
        }
    }
}
