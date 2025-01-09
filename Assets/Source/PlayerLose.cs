using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerLose : MonoBehaviour
{
    public int maxHealth; // ������������ ���������� ��������
    public Image[] hearts; // ������ Image ��� ����������� ��������
    public float beatScale = 1.2f; // ������������ ���������� Scale �� ����� ��������
    public float beatDuration = 0.5f; // ������������ ������ ����� ��������
    public string resultSceneName = "ResultScene"; // ��� ����� ��� ���������

    private int currentHealth; // ������� ��������
    private Coroutine beatCoroutine; // ������ �� �������� ��������

    void Start()
    {
        currentHealth = maxHealth; // ������������� �������� �� ��������
        UpdateHeartsUI(); // ��������� UI
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<Bullet>() != null)
        {
            TakeDamage(); // �������� ����
        }
    }

    private void TakeDamage()
    {
        if (currentHealth <= 0) return;

        currentHealth--; // ��������� ��������
        UpdateHeartsUI(); // ��������� UI

        if (currentHealth <= 0)
        {
            Debug.Log("Game ended!");
            SceneManager.LoadScene(resultSceneName); // ��������� ����� ����������
        }
    }

    private void UpdateHeartsUI()
    {
        for (int i = 0; i < hearts.Length; i++)
        {
            if (i < currentHealth)
            {
                hearts[i].enabled = true; // ���������� �������� ��������
                if (i == currentHealth - 1)
                {
                    StartBeatAnimation(hearts[i]); // �������� ��� �������� ��������� ������
                }
            }
            else
            {
                hearts[i].enabled = false; // �������� ������ ��������
            }
        }
    }

    private void StartBeatAnimation(Image heart)
    {
        if (beatCoroutine != null)
        {
            StopCoroutine(beatCoroutine); // ������������� ���������� ��������
        }

        beatCoroutine = StartCoroutine(BeatAnimation(heart));
    }

    private IEnumerator BeatAnimation(Image heart)
    {
        Vector3 originalScale = heart.transform.localScale;
        Vector3 targetScale = originalScale * beatScale;
        float timer = 0f;

        while (currentHealth > 0)
        {
            // ����������
            while (timer < beatDuration / 2f)
            {
                timer += Time.deltaTime;
                heart.transform.localScale = Vector3.Lerp(originalScale, targetScale, timer / (beatDuration / 2f));
                yield return null;
            }

            // ����������
            timer = 0f;
            while (timer < beatDuration / 2f)
            {
                timer += Time.deltaTime;
                heart.transform.localScale = Vector3.Lerp(targetScale, originalScale, timer / (beatDuration / 2f));
                yield return null;
            }

            timer = 0f; // ���������� ������ ��� ���������� �����
        }

        // ���������� ����������� Scale, ���� �������� ������������
        heart.transform.localScale = originalScale;
    }
}
