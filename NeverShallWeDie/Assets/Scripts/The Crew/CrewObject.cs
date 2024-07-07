using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[CreateAssetMenu(fileName = "crewSettings", menuName = "Crew")]
public class CrewObject : ScriptableObject
{
    public Sprite sprite;
    public Sprite draw;
    public string ptCrewFunction;
    public string engCrewFunction;
    [TextArea(5, 10)] public string ptDescription;
    [TextArea(5, 10)] public string engDescription;
}
