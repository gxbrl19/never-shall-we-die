using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Equipment", menuName = "Equipment")]
public class EquipmentObject : ScriptableObject
{
    [BoxGroup("Info")]
    [PreviewField(75)]
    [HideLabel]
    public Sprite sprite;

    [BoxGroup("Info")]
    [VerticalGroup("Info/Stats")]
    [LabelWidth(100)]
    public int equipmentID;

    [BoxGroup("Info")]
    [VerticalGroup("Info/Stats")]
    [LabelWidth(100)]
    public Equipments equipment;

    public int damager1;
    public int damager2;
    public int damager3;
    public int damager4;
}
