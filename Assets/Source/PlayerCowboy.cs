using UnityEngine;

public class PlayerCowboy : MonoBehaviour
{
    public Transform pointA;
    public Transform pointB;
    public float speed = 2f;
    public GameObject bulletPrefab;
    public Transform shootPoint;

    private bool movingToB = true;
    public bool hasShot = false; // ќтслеживает, стрел€л ли игрок
    public int ammoCount = 5; //  оличество патронов

    void Update()
    {
        Move();

        // ≈сли нет патронов, мен€ем направление при нажатии на Space
        if (ammoCount == 0 && Input.GetKeyDown(KeyCode.Space))
        {
            ChangeDirection();
        }
        // ≈сли есть патроны, стрел€ем при нажатии на Space
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
        ammoCount--; // ”меньшаем количество патронов при выстреле
        GameManager.Instance.CheckReloadState();
    }

    // ћетод дл€ смены направлени€ движени€
    void ChangeDirection()
    {
        movingToB = !movingToB;
    }
}
