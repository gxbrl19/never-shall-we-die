using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Skill", menuName = "Skill")]
public class SkillObject : ScriptableObject
{
    [BoxGroup("Info")]
    [PreviewField(75)]
    [HideLabel]
    public Sprite sprite;

    [BoxGroup("Info")]
    [VerticalGroup("Info/Stats")]
    [LabelWidth(100)]
    public int skillID;

    [BoxGroup("Info")]
    [VerticalGroup("Info/Stats")]
    [LabelWidth(100)]
    public Skills skill;

    [BoxGroup("Info")]
    [VerticalGroup("Info/Stats")]
    [LabelWidth(100)]
    public string nameSkill;

    [BoxGroup("Info")]
    [PreviewField(75)]
    [HideLabel]
    public Sprite parchment;

    [BoxGroup("Info")]
    [VerticalGroup("Info/Stats")]
    [LabelWidth(100)] public Sprite keyboardButton;

    [BoxGroup("Info")]
    [VerticalGroup("Info/Stats")]
    [LabelWidth(100)] public Sprite gamepadButton;



    //
    public int level1;
    public int level2;
    public int level3;
    public int level4;
}
