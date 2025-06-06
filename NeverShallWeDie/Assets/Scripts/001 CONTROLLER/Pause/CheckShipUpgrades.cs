using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CheckShipUpgrades : MonoBehaviour
{
    public ShipUpgrade _upgrade;
    Image _image;

    void Awake()
    {
        _image = GetComponent<Image>();
    }

    private void Update()
    {
        switch (_upgrade)
        {
            case ShipUpgrade.Submarine:
                _image.enabled = ShipUpgrades.instance.shipUgrade.Contains(ShipUpgrade.Submarine);
                break;
            case ShipUpgrade.Cannon:
                _image.enabled = ShipUpgrades.instance.shipUgrade.Contains(ShipUpgrade.Cannon);
                break;
            case ShipUpgrade.Propulsion:
                _image.enabled = ShipUpgrades.instance.shipUgrade.Contains(ShipUpgrade.Propulsion);
                break;
        }
    }
}
