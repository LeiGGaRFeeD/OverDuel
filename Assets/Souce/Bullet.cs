using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private float speed = 10f;
    private Vector2 direction;
    private Cowboy shooter;
    private float lifetime = 5f;

    public void Initialize(Cowboy cowboy)
    {
        shooter = cowboy;
        direction = shooter.isLeftCowboy ? Vector2.right : Vector2.left;
        Destroy(gameObject, lifetime);
    }

    private void Update()
    {
        transform.Translate(direction * speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Cowboy"))
        {
            Cowboy hitCowboy = collision.GetComponent<Cowboy>();
            if (hitCowboy != shooter && shooter.CanShoot())
            {
                hitCowboy.RegisterHit();
                Destroy(gameObject); // Уничтожение пули при попадании в ковбоя
            }
        }
        else if (collision.CompareTag("Cactus"))
        {
            Ricochet();
        }
        else if (collision.CompareTag("Box"))
        {
            SlowDown();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void Ricochet()
    {
        direction = new Vector2(-direction.x, Random.Range(-0.5f, 0.5f));
    }

    private void SlowDown()
    {
        speed /= 2; // Замедление скорости пули при попадании в ящик
    }

}
