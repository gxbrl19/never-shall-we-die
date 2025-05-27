using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "Item")]
public class ItemObject : ScriptableObject
{
    public Sprite sprite;
    public int itemID;
    public Items item;
    public string ptName;
    public string engName;
}
