using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Items
{
    Hammer, Grimoire, Cannon, Submarine, Propulsion, Secret, Crank, Key0, Key1, Key2, Key3, Key4, Key5, WaterGem, FireGem, AirGem
}

public class InventorySystem : MonoBehaviour
{
    public static InventorySystem instance;

    public List<Items> items;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        for (int i = 0; i < GameManager.instance._inventory.Count; i++)
        {
            items.Add(GameManager.instance._inventory[i]);
        }
    }
}
