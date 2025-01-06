using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject computerPrefab; // Префаб ковбоя-компьютера
    public Transform spawnPoint; // Точка спавна для компьютера
    public Transform[] computerWaypoints; // Массив точек для движения компьютера
    public static GameManager Instance;

    public PlayerCowboy player;
    public ComputerCowboy computer;
    public float reloadTime = 2f;

    public CowboySettings currentLevelSettings; // Настройки текущего уровня

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
        GameObject newComputer = Instantiate(computerPrefab, spawnPoint.position, Quaternion.identity);
        ComputerCowboy cowboy = newComputer.GetComponent<ComputerCowboy>();

        if (cowboy != null)
        {
            cowboy.settings = currentLevelSettings; // Установить настройки уровня
        }
    }

    public Transform[] GetComputerWaypoints()
    {
        return computerWaypoints;
    }
}
