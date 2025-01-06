using UnityEngine;

[CreateAssetMenu(fileName = "CowboySettings", menuName = "Game/CowboySettings")]
public class CowboySettings : ScriptableObject
{
    public int level; // ������� ������
    public float speed = 2f; // �������� �������� ������
}
