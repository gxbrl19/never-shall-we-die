using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ShipUpgrade
{
    Submarine, Propulsion, Cannon
}

public class ShipUpgrades : MonoBehaviour
{
    public static ShipUpgrades instance;

    public List<ShipUpgrade> shipUgrade;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        for (int i = 0; i < GameManager.instance._shipUpgrades.Count; i++)
        {
            shipUgrade.Add(GameManager.instance._shipUpgrades[i]);
        }
    }
}
