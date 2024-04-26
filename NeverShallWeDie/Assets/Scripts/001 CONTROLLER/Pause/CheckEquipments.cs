using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CheckEquipments : MonoBehaviour
{
    [SerializeField] EquipmentObject _equipment;
    [SerializeField] Image _imageButton;
    Text _textButton;

    private void Awake()
    {
        _textButton = GetComponentInChildren<Text>();
    }

    private void Start()
    {
        if (PlayerEquipment.instance.equipments.Contains(_equipment.equipment))
        {
            _imageButton.enabled = true;
            _textButton.text = _equipment.name;
        }
        else
        {
            _imageButton.enabled = false;
            _textButton.text = "???";
        }
    }

    public void Check()
    {

    }
}
