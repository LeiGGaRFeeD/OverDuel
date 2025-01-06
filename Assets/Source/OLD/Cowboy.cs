using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Cowboy : MonoBehaviour
{
    public Transform pointA;
    public Transform pointB;
    public Transform shootPoint;
    public GameObject bulletPrefab;
    public GameObject cowboyWithGun;
    public GameObject cowboyWithoutGun;
    public Text hitCounterText;
    public string actionKeyName; // Уникальное имя для настройки клавиши действия
    public KeyCode defaultActionKey; // Клавиша действия по умолчанию
    public bool isLeftCowboy;
    public bool autoShoot; // Новая переменная для автострельбы

    private KeyCode actionKey; // Настраиваемая клавиша действия
    private int hitCounter = 0;
    private bool hasGun = true;
    private float moveSpeed = 2f;
    private bool movingToPointB = true;

    public SpriteRenderer cowboySprite;
    private bool canShoot = true;
    private int score = 0;

    private GameController gameController;
    private float autoShootCooldown = 2f; // Время между автоматическими выстрелами
    private float autoShootTimer = 0f; // Таймер для автострельбы

    private void Start()
    {
        gameController = FindObjectOfType<GameController>();

        cowboyWithGun.SetActive(true);
        cowboyWithoutGun.SetActive(false);

        // Загрузка сохранённой клавиши действия или использование клавиши по умолчанию
        actionKey = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString(actionKeyName, defaultActionKey.ToString()));
        UpdateHitCounterText();
    }

    private void Update()
    {
        Move();

        if (autoShoot && hasGun && canShoot) // Логика автострельбы
        {
            autoShootTimer += Time.deltaTime;
            if (autoShootTimer >= autoShootCooldown)
            {
                autoShootTimer = 0f;
                Shoot();
            }
        }

        if (!autoShoot && Input.GetKeyDown(actionKey)) // Логика ручного управления
        {
            if (hasGun)
            {
                Shoot();
            }
            else
            {
                ChangeDirection();
            }
        }
    }

    public void AndroidMode()
    {
        if (hasGun)
        {
            Shoot();
        }
        else
        {
            ChangeDirection();
        }
    }

    private void Move()
    {
        if (movingToPointB)
        {
            transform.position = Vector2.MoveTowards(transform.position, pointB.position, moveSpeed * Time.deltaTime);
            if (Vector2.Distance(transform.position, pointB.position) < 0.1f)
            {
                movingToPointB = false;
            }
        }
        else
        {
            transform.position = Vector2.MoveTowards(transform.position, pointA.position, moveSpeed * Time.deltaTime);
            if (Vector2.Distance(transform.position, pointA.position) < 0.1f)
            {
                movingToPointB = true;
            }
        }
    }

    private void Shoot()
    {
        if (!canShoot) return; // Блокировка стрельбы во время кулдауна

        GameObject bullet = Instantiate(bulletPrefab, shootPoint.position, Quaternion.identity);
        Bullet bulletScript = bullet.GetComponent<Bullet>();
     //   bulletScript.Initialize(this);

        cowboyWithGun.SetActive(false);
        cowboyWithoutGun.SetActive(true);
        hasGun = false;

        gameController.RegisterShot(isLeftCowboy);
    }

    public void ChangeDirection()
    {
        movingToPointB = !movingToPointB;
    }

    public void Reload()
    {
        cowboyWithGun.SetActive(true);
        cowboyWithoutGun.SetActive(false);
        hasGun = true;
    }

    public void RegisterHit()
    {
        hitCounter++;
        if (isLeftCowboy == true)
        {
            PlayerPrefs.SetInt("Left", PlayerPrefs.GetInt("Left") + 1);
        }
        if (isLeftCowboy == false)
        {
            PlayerPrefs.SetInt("Right", PlayerPrefs.GetInt("Right") + 1);
        }
        UpdateHitCounterText();
        StartCoroutine(HandleBlinkingAndCooldown());
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        RegisterHit();
    }

    private void UpdateHitCounterText()
    {
        hitCounterText.text = hitCounter.ToString();
    }

    // Корутин для моргания и временной блокировки стрельбы
    private IEnumerator HandleBlinkingAndCooldown()
    {
        canShoot = false;
        for (int i = 0; i < 5; i++) // Моргание 5 раз
        {
            cowboySprite.enabled = !cowboySprite.enabled;
            yield return new WaitForSeconds(0.2f);
        }
        cowboySprite.enabled = true; // Восстановление видимости

        yield return new WaitForSeconds(2); // Время блокировки стрельбы
        canShoot = true;
    }

    // Проверка возможности стрельбы
    public bool CanShoot()
    {
        return canShoot;
    }

    // Метод для изменения клавиши действия (например, из меню настроек)
    public void ChangeActionKey(KeyCode newKey)
    {
        actionKey = newKey;
        PlayerPrefs.SetString(actionKeyName, newKey.ToString());
        PlayerPrefs.Save();
    }
}
