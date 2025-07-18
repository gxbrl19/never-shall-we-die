using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[CreateAssetMenu(fileName = "bossSettings", menuName = "Boss")]
public class BossObject : ScriptableObject
{
    public Sprite sprite;
    public int bossID;
    public float maxHealth;
}
