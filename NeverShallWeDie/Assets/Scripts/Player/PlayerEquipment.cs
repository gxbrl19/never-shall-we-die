using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Equipments 
{
    Katana, Compass, Parachute, Boots, Lantern
}

public class PlayerEquipment : MonoBehaviour
{
    public static PlayerEquipment instance;

    public List<Equipments> equipments;

    private void Awake() {
        instance = this;
    }

    private void Start() {
        for (int i = 0; i < GameManager.instance._equipments.Count; i++) {
            equipments.Add(GameManager.instance._equipments[i]);
        }
    }
}
