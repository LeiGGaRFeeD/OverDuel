using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 5f; // ��������� �������� ����
    public int maxRicochets = 3; // ������������ ���������� ���������
    public float ricochetAngle = 45f; // ���� ���������� ��� �������� (� ��������)
    public float ricochetSpeed = 3f; // �������� ���� ����� ��������

    private int currentRicochets = 0; // ������� ���������

    void Update()
    {
        transform.Translate(Vector2.right * speed * Time.deltaTime); // ������� ����
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
            Destroy(gameObject); // ���������� ���� ����� ��������� � ���������
        }
        else if (collision.collider.CompareTag("Player"))
        {
            PlayerCowboy player = collision.collider.GetComponent<PlayerCowboy>();
            if (player != null)
            {
                // ������ ��������� � ������, ���� ���������
            }
            Destroy(gameObject); // ���������� ���� ����� ��������� � ������
        }
        else if (collision.collider.CompareTag("Cactus")) // ������ �������� �� �������
        {
            Ricochet(collision.contacts[0].normal);
        }
        else
        {
            Destroy(gameObject); // ����������� ��� ������������ � ������� ���������
        }
    }

    // ������ ��������
    public void Ricochet(Vector2 collisionNormal)
    {
        if (currentRicochets >= maxRicochets)
        {
            Destroy(gameObject); // ���������� ����, ���� ��������� �������� ���������
            return;
        }

        currentRicochets++;

        // ����������� ���� �������� �� �������� � �������
        float angle = ricochetAngle * Mathf.Deg2Rad;

        // ������������ ����� ����������� � �����������
        Vector2 incomingDirection = transform.right; // ������� ����������� ����
        Vector2 ricochetDirection = Vector2.Reflect(incomingDirection, collisionNormal).normalized; // ����������� �������

        // ��������� ����������� �� �������� ����
        ricochetDirection = Quaternion.Euler(0, 0, ricochetAngle) * ricochetDirection;

        // ��������� �������� ���� ����� ��������
        speed = ricochetSpeed;

        // ��������� ����������� ����
        transform.right = ricochetDirection; // ����������� ���� ��������
    }
    public void ModifySpeed(float deltaSpeed)
    {
        speed += deltaSpeed;
    //    speed = Mathf.Max(0, speed); // ��������, ��� �������� �� ������ �������������
    }

}
