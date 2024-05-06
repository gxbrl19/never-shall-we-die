using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Crew", menuName = "Crew")]
public class CrewObject : ScriptableObject
{
    [BoxGroup("Info")]
    [PreviewField(75)]
    [HideLabel]
    public Sprite sprite;

    [BoxGroup("Info")]
    [VerticalGroup("Info/Stats")]
    [LabelWidth(100)]
    public string hability;

    [BoxGroup("Info")]
    [VerticalGroup("Info/Stats")]
    [LabelWidth(100)]
    public CrewMembers crewMember;
}
