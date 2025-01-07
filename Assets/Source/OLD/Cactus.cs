using UnityEngine;

public class Cactus : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Bullet"))
        {
            Bullet bullet = collision.GetComponent<Bullet>();
            if (bullet != null)
            {
                // Передаём нормаль поверхности кактуса для расчёта рикошета
                Vector2 collisionNormal = (collision.transform.position - transform.position).normalized;
                bullet.Ricochet(collisionNormal);
            }
        }
    }
}
