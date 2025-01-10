using UnityEngine;
using UnityEngine.UI;

public class ShieldController : MonoBehaviour
{
    public GameObject shield; // Объект щита
    public Scrollbar shieldBar; // UI-элемент ScrollBar для отображения энергии щита
    public float maxEnergy = 100f; // Максимальная энергия щита
    public float energyDepletionRate = 20f; // Скорость расхода энергии при удержании (единиц в секунду)
    public float energyRechargeRate = 10f; // Скорость восстановления энергии (единиц в секунду)
    public float rechargeDelay = 3f; // Задержка перед началом восстановления энергии после полного расхода

    public Button shieldUIButton; // Кнопка в UI для активации щита

    public float currentEnergy; // Текущий уровень энергии
    private bool isShieldActive = false; // Флаг активности щита
    private bool isRecharging = false; // Флаг, идёт ли восстановление энергии
    private float rechargeTimer = 0f; // Таймер для отслеживания задержки восстановления

    private bool isUIButtonToggled = false; // Флаг переключения состояния через кнопку UI
    private PlayerCowboy playerShooting; // Ссылка на компонент стрельбы игрока

    public AudioClip shieldActivationSound; // Звук активации щита
    private AudioSource audioSource; // Компонент для воспроизведения звука

    void Start()
    {
        currentEnergy = maxEnergy; // Устанавливаем начальную энергию на максимум
        shieldBar.size = currentEnergy / maxEnergy; // Обновляем индикатор энергии
        shield.SetActive(false); // Деактивируем щит по умолчанию

        playerShooting = GetComponent<PlayerCowboy>(); // Получаем компонент стрельбы игрока, если он есть

        // Настраиваем событие для UI-кнопки
        if (shieldUIButton != null)
        {
            shieldUIButton.onClick.AddListener(ToggleShieldUI);
        }

        // Инициализация AudioSource
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = shieldActivationSound;
        audioSource.playOnAwake = false;
    }

    void Update()
    {
        // Логика для клавиши E
        if (Input.GetKey(KeyCode.E) && currentEnergy > 0)
        {
            ActivateShield();
        }
        else if (isUIButtonToggled && currentEnergy > 0) // Логика для включенного щита через UI
        {
            ActivateShield();
        }
        else
        {
            DeactivateShield();
            RechargeEnergy();
        }

        // Восстановление энергии при выключенном щите
        if (!isShieldActive && currentEnergy < maxEnergy)
        {
            if (isRecharging)
            {
                rechargeTimer += Time.deltaTime;
                if (rechargeTimer >= rechargeDelay)
                {
                    RechargeEnergy();
                }
            }
            else
            {
                isRecharging = true;
                rechargeTimer = 0f;
            }
        }

        UpdateShieldBar(); // Обновляем UI-бар энергии
    }

    public void ToggleShieldUI()
    {
        // Переключение состояния щита через кнопку UI
        isUIButtonToggled = !isUIButtonToggled;
        if (isUIButtonToggled && currentEnergy > 0)
        {
            ActivateShield();
        }
        else
        {
            DeactivateShield();
            RechargeEnergy();
        }
    }

    private void ActivateShield()
    {
        if (!isShieldActive)
        {
            shield.SetActive(true); // Активируем щит
            isShieldActive = true;

            if (playerShooting != null)
            {
                playerShooting.enabled = false; // Отключаем стрельбу
            }

            PlayShieldSound(); // Проигрываем звук активации щита
        }

        currentEnergy -= energyDepletionRate * Time.deltaTime; // Тратим энергию
        currentEnergy = Mathf.Clamp(currentEnergy, 0, maxEnergy);

        if (currentEnergy <= 0)
        {
            DeactivateShield(); // Отключаем щит, если энергия закончилась
        }

        isRecharging = false; // Сбрасываем флаг восстановления энергии
        rechargeTimer = 0f; // Сбрасываем таймер восстановления
    }

    private void DeactivateShield()
    {
        if (isShieldActive)
        {
            shield.SetActive(false); // Деактивируем щит
            isShieldActive = false;

            if (playerShooting != null)
            {
                playerShooting.enabled = true; // Включаем стрельбу
            }
        }

        isRecharging = true; // Включаем восстановление энергии
        rechargeTimer = 0f; // Сбрасываем таймер восстановления
    }

    private void RechargeEnergy()
    {
        currentEnergy += energyRechargeRate * Time.deltaTime; // Восстанавливаем энергию
        currentEnergy = Mathf.Clamp(currentEnergy, 0, maxEnergy);
    }

    private void UpdateShieldBar()
    {
        shieldBar.size = currentEnergy / maxEnergy; // Обновляем индикатор энергии
    }

    private void PlayShieldSound()
    {
        if (audioSource != null && shieldActivationSound != null)
        {
            audioSource.Play(); // Проигрываем звук
        }
    }
}
