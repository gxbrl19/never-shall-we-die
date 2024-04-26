using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Skills
{
    AirCut, Tornado, WaterSpin
}

public class PlayerSkills : MonoBehaviour
{
    public static PlayerSkills instance;

    public List<Skills> skills;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        for (int i = 0; i < GameManager.instance._skills.Count; i++)
        {
            skills.Add(GameManager.instance._skills[i]);
        }
    }
}
