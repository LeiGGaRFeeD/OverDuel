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
    public string actionKeyName; // ���������� ��� ��� ��������� ������� ��������
    public KeyCode defaultActionKey; // ������� �������� �� ���������
    public bool isLeftCowboy;
    public bool autoShoot; // ����� ���������� ��� ������������

    private KeyCode actionKey; // ������������� ������� ��������
    private int hitCounter = 0;
    private bool hasGun = true;
    private float moveSpeed = 2f;
    private bool movingToPointB = true;

    public SpriteRenderer cowboySprite;
    private bool canShoot = true;
    private int score = 0;

    private GameController gameController;
    private float autoShootCooldown = 2f; // ����� ����� ��������������� ����������
    private float autoShootTimer = 0f; // ������ ��� ������������

    private void Start()
    {
        gameController = FindObjectOfType<GameController>();

        cowboyWithGun.SetActive(true);
        cowboyWithoutGun.SetActive(false);

        // �������� ���������� ������� �������� ��� ������������� ������� �� ���������
        actionKey = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString(actionKeyName, defaultActionKey.ToString()));
        UpdateHitCounterText();
    }

    private void Update()
    {
        Move();

        if (autoShoot && hasGun && canShoot) // ������ ������������
        {
            autoShootTimer += Time.deltaTime;
            if (autoShootTimer >= autoShootCooldown)
            {
                autoShootTimer = 0f;
                Shoot();
            }
        }

        if (!autoShoot && Input.GetKeyDown(actionKey)) // ������ ������� ����������
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
        if (!canShoot) return; // ���������� �������� �� ����� ��������

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

    // ������� ��� �������� � ��������� ���������� ��������
    private IEnumerator HandleBlinkingAndCooldown()
    {
        canShoot = false;
        for (int i = 0; i < 5; i++) // �������� 5 ���
        {
            cowboySprite.enabled = !cowboySprite.enabled;
            yield return new WaitForSeconds(0.2f);
        }
        cowboySprite.enabled = true; // �������������� ���������

        yield return new WaitForSeconds(2); // ����� ���������� ��������
        canShoot = true;
    }

    // �������� ����������� ��������
    public bool CanShoot()
    {
        return canShoot;
    }

    // ����� ��� ��������� ������� �������� (��������, �� ���� ��������)
    public void ChangeActionKey(KeyCode newKey)
    {
        actionKey = newKey;
        PlayerPrefs.SetString(actionKeyName, newKey.ToString());
        PlayerPrefs.Save();
    }
}
