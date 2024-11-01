using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private float speed = 10f;
    private Vector2 direction;
    private Cowboy shooter;
    private float lifetime = 5f; // ¬рем€ жизни пули, после которого она исчезнет

    public void Initialize(Cowboy cowboy)
    {
        shooter = cowboy;
        direction = shooter.isLeftCowboy ? Vector2.right : Vector2.left;
        Destroy(gameObject, lifetime); // ”ничтожение пули через заданное врем€
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
            if (hitCowboy != shooter)
            {
                hitCowboy.RegisterHit();
                Destroy(gameObject); // ”ничтожаем пулю при попадании в ковбо€
            }
        }
        else if (collision.CompareTag("Cactus"))
        {
            Ricochet();
        }
        else
        {
            Destroy(gameObject); // ”ничтожаем пулю при попадании в любые другие объекты
        }
    }

    public void Ricochet()
    {
        // –икошет мен€ет направление случайно влево или вправо, позвол€€ многократные рикошеты
        direction = new Vector2(-direction.x, Random.Range(-0.5f, 0.5f));
    }
}
