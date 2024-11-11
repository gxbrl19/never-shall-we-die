using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class Katana : MonoBehaviour
{
    [SerializeField] private EquipmentObject _equipmentObject;
    private Damager _damager;
    private int _level;

    private void Awake()
    {
        _damager = GetComponent<Damager>();
    }

    private void Update()
    {
        _level = GameManager.instance._katanaLevel;

        switch (_level)
        {
            default:
            case 0:
                _damager._power = _equipmentObject.damager1;
                break;
            case 1:
                _damager._power = _equipmentObject.damager2;
                break;
            case 2:
                _damager._power = _equipmentObject.damager3;
                break;
            case 3:
                _damager._power = _equipmentObject.damager4;
                break;
        }
    }
}
