using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 5f;

    void Update()
    {
        transform.Translate(Vector2.right * speed * Time.deltaTime);
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
        }

        if (collision.collider.CompareTag("Player"))
        {
            PlayerCowboy player = collision.collider.GetComponent<PlayerCowboy>();
            if (player != null)
            {
                // Логика попадания в игрока, если требуется
            }
        }

        Destroy(gameObject);
    }
}
