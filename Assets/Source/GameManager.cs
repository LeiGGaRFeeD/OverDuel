using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject computerPrefab; // ������ ������-����������
    public Transform spawnPoint; // ����� ������ ��� ����������
    public Transform[] computerWaypoints; // ������ ����� ��� �������� ����������
    public static GameManager Instance;

    public PlayerCowboy player;
    public ComputerCowboy computer;
    public float reloadTime = 2f;

    public CowboySettings currentLevelSettings; // ��������� �������� ������

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
        GameObject newComputer = Instantiate(computerPrefab, spawnPoint.position, Quaternion.identity);
        ComputerCowboy cowboy = newComputer.GetComponent<ComputerCowboy>();

        if (cowboy != null)
        {
            cowboy.settings = currentLevelSettings; // ���������� ��������� ������
        }
    }

    public Transform[] GetComputerWaypoints()
    {
        return computerWaypoints;
    }
}
