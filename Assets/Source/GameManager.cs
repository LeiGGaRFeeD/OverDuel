using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public GameObject regularComputerPrefab;
    public GameObject bossComputerPrefab;

    public Transform spawnPoint;
    public Transform[] computerWaypoints;
    public static GameManager Instance;

    public PlayerCowboy player;
    public ComputerCowboy computer;
    public float reloadTime = 2f;

    public CowboySettings currentLevelSettings;

    public Scrollbar progressBar;
    private int currentLevel = 1;
    private const int levelsInCycle = 5;

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

        currentLevel = PlayerPrefs.GetInt("CurrentLevel", 1);
        UpdateProgressBar();
    }

    public void CheckReloadState()
    {
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
        if (computer != null)
        {
            Destroy(computer.gameObject);
        }

        GameObject selectedPrefab = (currentLevel % levelsInCycle == 0) ? bossComputerPrefab : regularComputerPrefab;

        GameObject newComputer = Instantiate(selectedPrefab, spawnPoint.position, Quaternion.identity);
        computer = newComputer.GetComponent<ComputerCowboy>();
    }

    public void NextLevel()
    {
        currentLevel++;

        if (currentLevel % levelsInCycle == 0)
        {
            currentLevelSettings = GetBossSettings();
        }
        else
        {
            currentLevelSettings = GetRegularLevelSettings();
        }

        PlayerPrefs.SetInt("CurrentLevel", currentLevel);
        PlayerPrefs.Save();

        UpdateProgressBar();
        SpawnNewComputer();
    }

    private void UpdateProgressBar()
    {
        int positionInCycle = (currentLevel - 1) % levelsInCycle + 1;

        switch (positionInCycle)
        {
            case 1:
                progressBar.size = 0.2f; // 20%
                break;
            case 2:
                progressBar.size = 0.4f; // 40%
                break;
            case 3:
                progressBar.size = 0.6f; // 60%
                break;
            case 4:
                progressBar.size = 0.8f; // 80%
                break;
            case 5:
                progressBar.size = 1.0f; // 100%
                break;
        }
    }

    private CowboySettings GetRegularLevelSettings()
    {
        return new CowboySettings();
    }

    private CowboySettings GetBossSettings()
    {
        return new CowboySettings();
    }

    public Transform[] GetComputerWaypoints()
    {
        return computerWaypoints;
    }
}
