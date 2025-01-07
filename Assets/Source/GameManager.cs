using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public GameObject regularComputerPrefab; // ������ �������� ������
    public GameObject bossComputerPrefab; // ������ �����

    public Transform spawnPoint; // ����� ������ ��� ����������
    public Transform[] computerWaypoints; // ������ ����� ��� �������� ����������
    public static GameManager Instance;

    public PlayerCowboy player;
    public ComputerCowboy computer;
    public float reloadTime = 2f;

    public CowboySettings currentLevelSettings; // ��������� �������� ������

    public Scrollbar progressBar; // ScrollBar ��� ���������
    private int currentLevel = 1; // ������� �������
    private const int levelsInCycle = 5; // ���������� ������� � ����� (4 ������� � 1 ����)

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        // ��������� ������� ������� �� PlayerPrefs
        currentLevel = PlayerPrefs.GetInt("CurrentLevel", 1);
        UpdateProgressBar();
    }

    public void CheckReloadState()
    {
        // ���� ��� ����������, �������� �����������
        if (player.hasShot && computer.hasShot)
        {
            Invoke(nameof(Reload), reloadTime);
        }
    }

    void Reload()
    {
        player.hasShot = false;
        computer.hasShot = false;
    }

    public void SpawnNewComputer()
    {
        // ���������� ������� ���������� ����� ������� ������
        if (computer != null)
        {
            Destroy(computer.gameObject);
        }

        // �������� ������ � ����������� �� �������� ������
        GameObject selectedPrefab = (currentLevel % levelsInCycle == 0) ? bossComputerPrefab : regularComputerPrefab;

        // ������� ������ ����������
        GameObject newComputer = Instantiate(selectedPrefab, spawnPoint.position, Quaternion.identity);
        computer = newComputer.GetComponent<ComputerCowboy>();

        if (computer != null)
        {
      //      computer.settings = currentLevelSettings; // ���������� ��������� ������
        }
    }

    // ����� ��� �������� �� ��������� �������
    public void NextLevel()
    {
        currentLevel++;

        // ���� ������� ������� �� 5 (�� ����, ��� ����)
        if (currentLevel % levelsInCycle == 0)
        {
            // ������������� ��������� �����
            currentLevelSettings = GetBossSettings();
        }
        else
        {
            // ������� ������
            currentLevelSettings = GetRegularLevelSettings();
        }

        // ��������� ������� ������� � PlayerPrefs
        PlayerPrefs.SetInt("CurrentLevel", currentLevel);
        PlayerPrefs.Save();

        // ��������� �������� ���
        UpdateProgressBar();

        // ������� ������ ����������
        SpawnNewComputer();
    }

    // ��������� �������� ��� � ����������� �� ������
    private void UpdateProgressBar()
    {
        // ������ 5-� ������� ����������� ������� ���������
        int divisions = (currentLevel - 1) / levelsInCycle + 1;
        float progress = (float)((currentLevel - 1) % levelsInCycle) / (levelsInCycle - 1);

        // ��������� ScrollBar � ����������� �� �������
        progressBar.size = progress / divisions;
    }

    // ��������� �������� ��� �������� ������
    private CowboySettings GetRegularLevelSettings()
    {
        // ������ ��� ������� ������� (����� �������� ��������� ���������)
        return new CowboySettings(); // ������� ����������� ��������� ��� ������� �������
    }

    // ��������� �������� ��� ������ � ������
    private CowboySettings GetBossSettings()
    {
        // ������ ��� ����� (����� �������� ��������� ��� �����)
        return new CowboySettings(); // ������� ��������� ��� �����
    }

    public Transform[] GetComputerWaypoints()
    {
        return computerWaypoints;
    }
}
