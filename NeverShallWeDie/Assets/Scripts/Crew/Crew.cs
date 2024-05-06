using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CrewMembers
{
    Helmsman, Navigator, Witch, Blacksmith
}

public class Crew : MonoBehaviour
{
    public static Crew instance;
    public List<CrewMembers> crewMembers;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        for (int i = 0; i < GameManager.instance._crewMembers.Count; i++)
        {
            crewMembers.Add(GameManager.instance._crewMembers[i]);
        }
    }
}
