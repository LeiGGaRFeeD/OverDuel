using UnityEngine;

[CreateAssetMenu(fileName = "CowboySettings", menuName = "Game/CowboySettings")]
public class CowboySettings : ScriptableObject
{
    public int level; // Уровень ковбоя
    public float speed = 2f; // Скорость движения ковбоя
}
