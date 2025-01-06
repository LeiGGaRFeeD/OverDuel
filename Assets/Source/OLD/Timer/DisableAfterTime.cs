using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableAfterTime : MonoBehaviour
{
    public float timeToDisable = 5f; // ����� � ��������

    void Start()
    {
        // ��������� �������� ��� ���������� �������
        StartCoroutine(DisableObjectAfterTime());
    }

    private IEnumerator DisableObjectAfterTime()
    {
        yield return new WaitForSeconds(timeToDisable);
        gameObject.SetActive(false); // ��������� GameObject
    }
}
