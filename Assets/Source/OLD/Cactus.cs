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
                // ������� ������� ����������� ������� ��� ������� ��������
                Vector2 collisionNormal = (collision.transform.position - transform.position).normalized;
                bullet.Ricochet(collisionNormal);
            }
        }
    }
}
