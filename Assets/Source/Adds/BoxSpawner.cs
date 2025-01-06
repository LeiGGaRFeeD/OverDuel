using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxSpawner : MonoBehaviour
{
    public GameObject boxPrefab;                // ������ �������
    public int activeLevel = 1;                 // �������, �� ������� ������� �������
    public float spawnInterval = 2f;            // �������� ������ �������
    public int maxBoxes = 10;                   // ������������ ���������� ������� �� ������
    public Transform spawnArea;                 // ��������� ������� ������
    public LayerMask boxLayer;                  // ����, ��������������� ��������
    public float boxRadius = 0.5f;              // ������ �������� ��������� ��� �������

    private bool isSpawning = false;            // ������� �� �����
    private Coroutine spawnCoroutine;           // ��� �������� ������ �� �������� ������
    private List<GameObject> spawnedBoxes = new List<GameObject>(); // ������ ���� ������������ �������

    private void Start()
    {
        CheckSpawnerStatus();
    }

    private void Update()
    {
        // �������� ��������� �������� � ����������� �� �������� ������
        CheckSpawnerStatus();
    }

    private void CheckSpawnerStatus()
    {
        // �������� ������� ������� (��������, �� ������� �������)
        int currentLevel = GetCurrentLevel(); // �������� ����� ������ ��������� ������

        if (currentLevel >= activeLevel && !isSpawning)
        {
            StartSpawning();
        }
        else if (currentLevel < activeLevel && isSpawning)
        {
            StopSpawning();
        }
    }

    private void StartSpawning()
    {
        isSpawning = true;
        spawnCoroutine = StartCoroutine(SpawnBoxes());
    }

    private void StopSpawning()
    {
        isSpawning = false;
        if (spawnCoroutine != null)
        {
            StopCoroutine(spawnCoroutine);
            spawnCoroutine = null;
        }

        // ������� ������ � ����������� ���� ������������ �������
        foreach (var box in spawnedBoxes)
        {
            if (box != null)
            {
                Destroy(box);
            }
        }
        spawnedBoxes.Clear();
    }

    private IEnumerator SpawnBoxes()
    {
        while (isSpawning)
        {
            // �������� �� ������������ ���������� �������
            if (spawnedBoxes.Count < maxBoxes)
            {
                Vector2 spawnPosition = GetRandomPositionInSpawnArea();

                // �������� �� ��������� � ������� ���������
                if (!Physics2D.OverlapCircle(spawnPosition, boxRadius, boxLayer))
                {
                    GameObject newBox = Instantiate(boxPrefab, spawnPosition, Quaternion.identity);
                    spawnedBoxes.Add(newBox);
                }
            }

            yield return new WaitForSeconds(spawnInterval);
        }
    }

    private Vector2 GetRandomPositionInSpawnArea()
    {
        // �������� ������� ������� ������
        Vector2 areaSize = spawnArea.localScale / 2f;

        // ��������� ������� ������ �������
        float x = Random.Range(spawnArea.position.x - areaSize.x, spawnArea.position.x + areaSize.x);
        float y = Random.Range(spawnArea.position.y - areaSize.y, spawnArea.position.y + areaSize.y);

        return new Vector2(x, y);
    }

    private int GetCurrentLevel()
    {
        // ����� ����� ������� ������ ��������� �������� ������
        return PlayerPrefs.GetInt("CurrentLevel", 1); // �������� ��� �������������
    }
}
