using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class Katana : MonoBehaviour
{
    [SerializeField] private EquipmentObject equipmentObject;
    private Damager damager;

    private void Awake()
    {
        damager = GetComponent<Damager>();
    }

    private void Update()
    {
        damager.attackPower = GameManager.instance._katanaLevel;
    }
}
