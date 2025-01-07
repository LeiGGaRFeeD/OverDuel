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

    private float currentEnergy; // Текущий уровень энергии
    private bool isShieldActive = false; // Флаг активности щита
    private bool isRecharging = false; // Флаг, идёт ли восстановление энергии
    private float rechargeTimer = 0f; // Таймер для отслеживания задержки восстановления

    private bool isUIButtonPressed = false; // Флаг для отслеживания удержания кнопки UI
    private PlayerCowboy playerShooting; // Ссылка на компонент стрельбы игрока

    void Start()
    {
        currentEnergy = maxEnergy; // Устанавливаем начальную энергию на максимум
        shieldBar.size = currentEnergy / maxEnergy; // Обновляем индикатор энергии
        shield.SetActive(false); // Деактивируем щит по умолчанию

        playerShooting = GetComponent<PlayerCowboy>(); // Получаем компонент стрельбы игрока, если он есть

        // Подписываемся на события UI-кнопки
        if (shieldUIButton != null)
        {
            shieldUIButton.onClick.AddListener(() => { isUIButtonPressed = true; });
        }
    }

    void Update()
    {
        // Проверка удержания клавиши E или кнопки UI
        if ((Input.GetKey(KeyCode.E) || isUIButtonPressed) && currentEnergy > 0)
        {
            ActivateShield();
        }
        else
        {
            DeactivateShield();
        }

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

        isUIButtonPressed = false; // Сбрасываем флаг UI-кнопки
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
}
