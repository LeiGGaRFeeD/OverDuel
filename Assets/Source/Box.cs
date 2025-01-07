using UnityEngine;

public class Box : MonoBehaviour
{
    public float bulletSpeedReduction = 2f; // �� ������� ��������� �������� ����

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Bullet"))
        {
            Bullet bullet = collision.GetComponent<Bullet>();
            if (bullet != null)
            {
                // ��������� �������� ����
                bullet.ModifySpeed(-bulletSpeedReduction);

                // ���������� �������
                Destroy(gameObject);
            }
        }
    }
}
