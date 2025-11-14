using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;

public class CheckEquipments : MonoBehaviour
{
    [SerializeField] int _buttonID;
    [SerializeField] EquipmentObject _equipment;
    Image _imageButton;

    private void Awake()
    {
        _imageButton = transform.Find("image_equip").GetComponent<Image>();
    }

    private void Update()
    {
        _imageButton.enabled = PlayerEquipment.instance.equipments.Contains(_equipment.equipment);
    }

    public void Check() //chamado no evento do botão
    {
        UIManager.instance._descriptions.SetActive(false);
        UIManager.instance._btnKeyboard.gameObject.SetActive(false);
        UIManager.instance._btnGamepad.gameObject.SetActive(false);

        if (!PlayerEquipment.instance.equipments.Contains(_equipment.equipment))
            return;

        UIManager.instance._descriptions.SetActive(true);
        //localization
        var currentLocale = LocalizationSettings.SelectedLocale;
        if (currentLocale.Identifier.Code == "pt-BR") { UIManager.instance._txtDescription.text = _equipment.ptDescription; }
        else if (currentLocale.Identifier.Code == "en") { UIManager.instance._txtDescription.text = _equipment.engDescription; }
        //localization

        if (_equipment.gamepadButton == null || _equipment.keyboardButton == null)
            return;

        UIManager.instance._btnKeyboard.gameObject.SetActive(true);
        UIManager.instance._btnGamepad.gameObject.SetActive(true);
        UIManager.instance._btnKeyboard.sprite = _equipment.keyboardButton;
        UIManager.instance._btnGamepad.sprite = _equipment.gamepadButton;
    }
}
