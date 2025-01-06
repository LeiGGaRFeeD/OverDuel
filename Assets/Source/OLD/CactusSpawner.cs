using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CactusSpawner : MonoBehaviour
{
    public GameObject cactusPrefab;
    public Transform spawnArea;
    public float spawnInterval = 5f; // »нтервал спавна
    public int maxCacti = 3; // ћаксимальное количество кактусов в зоне

    private List<GameObject> spawnedCacti = new List<GameObject>();

    private void Start()
    {
        StartCoroutine(SpawnCacti());
    }

    private IEnumerator SpawnCacti() 
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnInterval);

            // ѕроверка на превышение максимального количества кактусов
            if (spawnedCacti.Count >= maxCacti)
            {
                Destroy(spawnedCacti[0]);
                spawnedCacti.RemoveAt(0); // ”даление самого старого кактуса
            }

            // —оздание нового кактуса в пределах зоны
            Vector2 spawnPosition = new Vector2(
                Random.Range(spawnArea.position.x - spawnArea.localScale.x / 2, spawnArea.position.x + spawnArea.localScale.x / 2),
                Random.Range(spawnArea.position.y - spawnArea.localScale.y / 2, spawnArea.position.y + spawnArea.localScale.y / 2)
            );

            GameObject newCactus = Instantiate(cactusPrefab, spawnPosition, Quaternion.identity);
            spawnedCacti.Add(newCactus);
        }
    }
}
