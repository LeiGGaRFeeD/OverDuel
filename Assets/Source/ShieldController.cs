using UnityEngine;
using UnityEngine.UI;

public class ShieldController : MonoBehaviour
{
    public GameObject shield; // ������ ����
    public Scrollbar shieldBar; // UI-������� ScrollBar ��� ����������� ������� ����
    public float maxEnergy = 100f; // ������������ ������� ����
    public float energyDepletionRate = 20f; // �������� ������� ������� ��� ��������� (������ � �������)
    public float energyRechargeRate = 10f; // �������� �������������� ������� (������ � �������)
    public float rechargeDelay = 3f; // �������� ����� ������� �������������� ������� ����� ������� �������

    public Button shieldUIButton; // ������ � UI ��� ��������� ����

    private float currentEnergy; // ������� ������� �������
    private bool isShieldActive = false; // ���� ���������� ����
    private bool isRecharging = false; // ����, ��� �� �������������� �������
    private float rechargeTimer = 0f; // ������ ��� ������������ �������� ��������������

    private bool isUIButtonPressed = false; // ���� ��� ������������ ��������� ������ UI
    private PlayerCowboy playerShooting; // ������ �� ��������� �������� ������

    void Start()
    {
        currentEnergy = maxEnergy; // ������������� ��������� ������� �� ��������
        shieldBar.size = currentEnergy / maxEnergy; // ��������� ��������� �������
        shield.SetActive(false); // ������������ ��� �� ���������

        playerShooting = GetComponent<PlayerCowboy>(); // �������� ��������� �������� ������, ���� �� ����

        // ������������� �� ������� UI-������
        if (shieldUIButton != null)
        {
            shieldUIButton.onClick.AddListener(() => { isUIButtonPressed = true; });
        }
    }

    void Update()
    {
        // �������� ��������� ������� E ��� ������ UI
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

        UpdateShieldBar(); // ��������� UI-��� �������
    }

    private void ActivateShield()
    {
        if (!isShieldActive)
        {
            shield.SetActive(true); // ���������� ���
            isShieldActive = true;

            if (playerShooting != null)
            {
                playerShooting.enabled = false; // ��������� ��������
            }
        }

        currentEnergy -= energyDepletionRate * Time.deltaTime; // ������ �������
        currentEnergy = Mathf.Clamp(currentEnergy, 0, maxEnergy);

        if (currentEnergy <= 0)
        {
            DeactivateShield(); // ��������� ���, ���� ������� �����������
        }

        isRecharging = false; // ���������� ���� �������������� �������
        rechargeTimer = 0f; // ���������� ������ ��������������
    }

    private void DeactivateShield()
    {
        if (isShieldActive)
        {
            shield.SetActive(false); // ������������ ���
            isShieldActive = false;

            if (playerShooting != null)
            {
                playerShooting.enabled = true; // �������� ��������
            }
        }

        isUIButtonPressed = false; // ���������� ���� UI-������
    }

    private void RechargeEnergy()
    {
        currentEnergy += energyRechargeRate * Time.deltaTime; // ��������������� �������
        currentEnergy = Mathf.Clamp(currentEnergy, 0, maxEnergy);
    }

    private void UpdateShieldBar()
    {
        shieldBar.size = currentEnergy / maxEnergy; // ��������� ��������� �������
    }
}
