using UnityEngine;

public class PlayerCowboy : MonoBehaviour
{
    public Transform pointA;
    public Transform pointB;
    public float speed = 2f;
    public GameObject bulletPrefab;
    public Transform shootPoint;

    private bool movingToB = true;
    public bool hasShot = false; // �����������, ������� �� �����
    public int ammoCount = 5; // ���������� ��������

    void Update()
    {
        // ���� hasShot true, ����� ������ ����������� ��� ������� �� Space
        if (hasShot && Input.GetKeyDown(KeyCode.Space))
        {
            ChangeDirection();
        }
        // ���� hasShot false, ����� �������� ��� ������� �� Space
        else if (!hasShot && Input.GetKeyDown(KeyCode.Space))
        {
            Shoot();
        }

        // �������� ������
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
        if (ammoCount > 0) // �������� �� ������� ��������
        {
            Instantiate(bulletPrefab, shootPoint.position, Quaternion.identity);
            hasShot = true; // �������������, ��� ����� ������ �������
            ammoCount--; // ��������� ���������� ��������
            GameManager.Instance.CheckReloadState();
        }
    }

    // ����� ��� ����� ����������� ��������
    void ChangeDirection()
    {
        movingToB = !movingToB;
    }

    // ����� ��� ����������� (���������� GameManager'��)
    public void Reload(int newAmmo)
    {
        ammoCount = newAmmo; // ��������������� �������
        hasShot = false; // ���������� ���� ��������
    }
}
