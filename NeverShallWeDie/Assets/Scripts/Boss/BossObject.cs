using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[CreateAssetMenu(fileName = "bossSettings", menuName = "Boss")]
public class BossObject : ScriptableObject
{
    public Sprite sprite;

    [Header("Settings")]
    public int maxHealth;
    public int dropRate;
    public AudioClip deadSound;
    public float volume;
    public Color damageColor;
}
