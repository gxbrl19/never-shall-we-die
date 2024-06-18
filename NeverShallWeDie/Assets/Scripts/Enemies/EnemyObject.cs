using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[CreateAssetMenu(fileName = "enemySettings", menuName = "Enemy")]
public class EnemyObject : ScriptableObject
{
    public Sprite sprite;

    [Header("Settings")]
    public int maxHealth;
    public int dropRate;
    public Color damageColor;
}
