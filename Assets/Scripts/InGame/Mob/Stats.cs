using UnityEngine;

[System.Serializable]
public class Stats
{
    [Min(1)] public float maxHP = 100f;
    [Min(0)] public float maxStamina = 50f;
    [Min(0)] public float attackPower = 10f;
    [Min(0)] public float defense = 0f;
    [Min(0)] public float moveSpeed = 5f;
    [Min(0)] public float attackSpeed = 1f;
    [Min(1)] public float xpValue = 1f;
}



