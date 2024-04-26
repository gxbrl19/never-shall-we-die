using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Katana : MonoBehaviour {
    [SerializeField] private EquipmentObject _equipmentObject;
    private Damager _damager;

    private void Awake() {
        _damager = GetComponent<Damager>();
    }

    private void Update() {
        //TODO: definir o dano de acordo com o level da arma
        _damager._power = _equipmentObject.damager1;
    }
}
