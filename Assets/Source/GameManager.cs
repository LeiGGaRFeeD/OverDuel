using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public GameObject regularComputerPrefab; // Префаб обычного ковбоя
    public GameObject bossComputerPrefab; // Префаб босса

    public Transform spawnPoint; // Точка спавна для компьютера
    public Transform[] computerWaypoints; // Массив точек для движения компьютера
    public static GameManager Instance;

    public PlayerCowboy player;
    public ComputerCowboy computer;
    public float reloadTime = 2f;

    public CowboySettings currentLevelSettings; // Настройки текущего уровня

    public Scrollbar progressBar; // ScrollBar для прогресса
    private int currentLevel = 1; // Текущий уровень
    private const int levelsInCycle = 5; // Количество уровней в цикле (4 обычных и 1 босс)

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

        // Загружаем текущий уровень из PlayerPrefs
        currentLevel = PlayerPrefs.GetInt("CurrentLevel", 1);
        UpdateProgressBar();
    }

    public void CheckReloadState()
    {
        // Если оба выстрелили, начинаем перезарядку
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
        // Уничтожаем старого компьютера перед спавном нового
        if (computer != null)
        {
            Destroy(computer.gameObject);
        }

        // Выбираем префаб в зависимости от текущего уровня
        GameObject selectedPrefab = (currentLevel % levelsInCycle == 0) ? bossComputerPrefab : regularComputerPrefab;

        // Спавним нового компьютера
        GameObject newComputer = Instantiate(selectedPrefab, spawnPoint.position, Quaternion.identity);
        computer = newComputer.GetComponent<ComputerCowboy>();

        if (computer != null)
        {
      //      computer.settings = currentLevelSettings; // Установить настройки уровня
        }
    }

    // Метод для перехода на следующий уровень
    public void NextLevel()
    {
        currentLevel++;

        // Если уровень делится на 5 (то есть, это босс)
        if (currentLevel % levelsInCycle == 0)
        {
            // Устанавливаем настройки босса
            currentLevelSettings = GetBossSettings();
        }
        else
        {
            // Обычные уровни
            currentLevelSettings = GetRegularLevelSettings();
        }

        // Сохраняем текущий уровень в PlayerPrefs
        PlayerPrefs.SetInt("CurrentLevel", currentLevel);
        PlayerPrefs.Save();

        // Обновляем прогресс бар
        UpdateProgressBar();

        // Спавним нового компьютера
        SpawnNewComputer();
    }

    // Обновляем прогресс бар в зависимости от уровня
    private void UpdateProgressBar()
    {
        // Каждый 5-й уровень увеличивает деления прогресса
        int divisions = (currentLevel - 1) / levelsInCycle + 1;
        float progress = (float)((currentLevel - 1) % levelsInCycle) / (levelsInCycle - 1);

        // Заполняем ScrollBar в зависимости от делений
        progressBar.size = progress / divisions;
    }

    // Получение настроек для обычного уровня
    private CowboySettings GetRegularLevelSettings()
    {
        // Логика для обычных уровней (можно добавить различные параметры)
        return new CowboySettings(); // Вернуть стандартные настройки для обычных уровней
    }

    // Получение настроек для уровня с боссом
    private CowboySettings GetBossSettings()
    {
        // Логика для босса (можно изменить параметры для босса)
        return new CowboySettings(); // Вернуть настройки для босса
    }

    public Transform[] GetComputerWaypoints()
    {
        return computerWaypoints;
    }
}
