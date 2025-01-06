using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableAfterTime : MonoBehaviour
{
    public float timeToDisable = 5f; // Время в секундах

    void Start()
    {
        // Запускаем корутину для отключения объекта
        StartCoroutine(DisableObjectAfterTime());
    }

    private IEnumerator DisableObjectAfterTime()
    {
        yield return new WaitForSeconds(timeToDisable);
        gameObject.SetActive(false); // Отключаем GameObject
    }
}
