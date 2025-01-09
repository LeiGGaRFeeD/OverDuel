using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerLose : MonoBehaviour
{
    public int maxHealth; // Максимальное количество здоровья
    public Image[] hearts; // Массив Image для отображения здоровья
    public float beatScale = 1.2f; // Максимальное увеличение Scale во время анимации
    public float beatDuration = 0.5f; // Длительность одного цикла анимации
    public string resultSceneName = "ResultScene"; // Имя сцены при проигрыше

    private int currentHealth; // Текущее здоровье
    private Coroutine beatCoroutine; // Ссылка на корутину анимации

    void Start()
    {
        currentHealth = maxHealth; // Устанавливаем здоровье на максимум
        UpdateHeartsUI(); // Обновляем UI
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<Bullet>() != null)
        {
            TakeDamage(); // Получаем урон
        }
    }

    private void TakeDamage()
    {
        if (currentHealth <= 0) return;

        currentHealth--; // Уменьшаем здоровье
        UpdateHeartsUI(); // Обновляем UI

        if (currentHealth <= 0)
        {
            Debug.Log("Game ended!");
            SceneManager.LoadScene(resultSceneName); // Загружаем сцену результата
        }
    }

    private void UpdateHeartsUI()
    {
        for (int i = 0; i < hearts.Length; i++)
        {
            if (i < currentHealth)
            {
                hearts[i].enabled = true; // Показываем активные сердечки
                if (i == currentHealth - 1)
                {
                    StartBeatAnimation(hearts[i]); // Анимация для текущего активного сердца
                }
            }
            else
            {
                hearts[i].enabled = false; // Скрываем пустые сердечки
            }
        }
    }

    private void StartBeatAnimation(Image heart)
    {
        if (beatCoroutine != null)
        {
            StopCoroutine(beatCoroutine); // Останавливаем предыдущую анимацию
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
            // Увеличение
            while (timer < beatDuration / 2f)
            {
                timer += Time.deltaTime;
                heart.transform.localScale = Vector3.Lerp(originalScale, targetScale, timer / (beatDuration / 2f));
                yield return null;
            }

            // Уменьшение
            timer = 0f;
            while (timer < beatDuration / 2f)
            {
                timer += Time.deltaTime;
                heart.transform.localScale = Vector3.Lerp(targetScale, originalScale, timer / (beatDuration / 2f));
                yield return null;
            }

            timer = 0f; // Сбрасываем таймер для следующего цикла
        }

        // Возвращаем изначальный Scale, если анимация прекращается
        heart.transform.localScale = originalScale;
    }
}
