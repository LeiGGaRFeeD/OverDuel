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

    public float currentEnergy; // ������� ������� �������
    private bool isShieldActive = false; // ���� ���������� ����
    private bool isRecharging = false; // ����, ��� �� �������������� �������
    private float rechargeTimer = 0f; // ������ ��� ������������ �������� ��������������

    private bool isUIButtonToggled = false; // ���� ������������ ��������� ����� ������ UI
    private PlayerCowboy playerShooting; // ������ �� ��������� �������� ������

    public AudioClip shieldActivationSound; // ���� ��������� ����
    private AudioSource audioSource; // ��������� ��� ��������������� �����

    void Start()
    {
        currentEnergy = maxEnergy; // ������������� ��������� ������� �� ��������
        shieldBar.size = currentEnergy / maxEnergy; // ��������� ��������� �������
        shield.SetActive(false); // ������������ ��� �� ���������

        playerShooting = GetComponent<PlayerCowboy>(); // �������� ��������� �������� ������, ���� �� ����

        // ����������� ������� ��� UI-������
        if (shieldUIButton != null)
        {
            shieldUIButton.onClick.AddListener(ToggleShieldUI);
        }

        // ������������� AudioSource
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = shieldActivationSound;
        audioSource.playOnAwake = false;
    }

    void Update()
    {
        // ������ ��� ������� E
        if (Input.GetKey(KeyCode.E) && currentEnergy > 0)
        {
            ActivateShield();
        }
        else if (isUIButtonToggled && currentEnergy > 0) // ������ ��� ����������� ���� ����� UI
        {
            ActivateShield();
        }
        else
        {
            DeactivateShield();
            RechargeEnergy();
        }

        // �������������� ������� ��� ����������� ����
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

    public void ToggleShieldUI()
    {
        // ������������ ��������� ���� ����� ������ UI
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
            shield.SetActive(true); // ���������� ���
            isShieldActive = true;

            if (playerShooting != null)
            {
                playerShooting.enabled = false; // ��������� ��������
            }

            PlayShieldSound(); // ����������� ���� ��������� ����
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

        isRecharging = true; // �������� �������������� �������
        rechargeTimer = 0f; // ���������� ������ ��������������
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

    private void PlayShieldSound()
    {
        if (audioSource != null && shieldActivationSound != null)
        {
            audioSource.Play(); // ����������� ����
        }
    }
}
