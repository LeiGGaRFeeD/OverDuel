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
    public KeyCode actionKey;
    public bool isLeftCowboy;

    private int hitCounter = 0;
    private bool hasGun = true;
    private float moveSpeed = 2f;
    private bool movingToPointB = true;

    private GameController gameController;

    private void Start()
    {
        gameController = FindObjectOfType<GameController>();

        cowboyWithGun.SetActive(true);
        cowboyWithoutGun.SetActive(false);
        UpdateHitCounterText();
    }

    private void Update()
    {
        Move();

        if (Input.GetKeyDown(actionKey))
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
        GameObject bullet = Instantiate(bulletPrefab, shootPoint.position, Quaternion.identity);
        Bullet bulletScript = bullet.GetComponent<Bullet>();
        bulletScript.Initialize(this);

        cowboyWithGun.SetActive(false);
        cowboyWithoutGun.SetActive(true);
        hasGun = false;

        gameController.RegisterShot(isLeftCowboy);
    }

    private void ChangeDirection()
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
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        RegisterHit();
    }
    private void UpdateHitCounterText()
    {
        hitCounterText.text = hitCounter.ToString();
    }
}
