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

    void Update()
    {
        Move();

        if (Input.GetKeyDown(KeyCode.Space) && !hasShot)
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
        GameManager.Instance.CheckReloadState();
    }
}
