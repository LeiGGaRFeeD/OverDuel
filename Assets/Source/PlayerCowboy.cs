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
        Move();

        // ���� ��� ��������, ������ ����������� ��� ������� �� Space
        if (ammoCount == 0 && Input.GetKeyDown(KeyCode.Space))
        {
            ChangeDirection();
        }
        // ���� ���� �������, �������� ��� ������� �� Space
        else if (Input.GetKeyDown(KeyCode.Space) && !hasShot && ammoCount > 0)
        {
            Shoot();
        }
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
        Instantiate(bulletPrefab, shootPoint.position, Quaternion.identity);
        hasShot = true;
        ammoCount--; // ��������� ���������� �������� ��� ��������
        GameManager.Instance.CheckReloadState();
    }

    // ����� ��� ����� ����������� ��������
    void ChangeDirection()
    {
        movingToB = !movingToB;
    }
}
