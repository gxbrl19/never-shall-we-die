using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;

public class CheckEquipments : MonoBehaviour
{
    [SerializeField] EquipmentObject _equipment;
    [SerializeField] Image _imageButton;
    Text _textButton;

    private void Awake()
    {
        _textButton = GetComponentInChildren<Text>();
    }

    private void Update()
    {
        if (PlayerEquipment.instance.equipments.Contains(_equipment.equipment))
        {
            _imageButton.enabled = true;
            _textButton.enabled = true;
        }
        else
        {
            _imageButton.enabled = false;
            _textButton.enabled = false;
        }
    }

    public void Check()
    {
        if (PlayerEquipment.instance.equipments.Contains(_equipment.equipment))
        {
            UIManager.instance._descriptions.SetActive(true);

            if (_equipment.gamepadButton != null && _equipment.keyboardButton != null)
            {
                UIManager.instance._btnKeyboard.gameObject.SetActive(true);
                UIManager.instance._btnGamepad.gameObject.SetActive(true);
                UIManager.instance._btnKeyboard.sprite = _equipment.keyboardButton;
                UIManager.instance._btnGamepad.sprite = _equipment.gamepadButton;
            }
            else
            {
                UIManager.instance._btnKeyboard.gameObject.SetActive(false);
                UIManager.instance._btnGamepad.gameObject.SetActive(false);
            }

            //localization
            var currentLocale = LocalizationSettings.SelectedLocale;
            if (currentLocale.Identifier.Code == "pt-BR") { UIManager.instance._txtDescription.text = _equipment.ptDescription; }
            else if (currentLocale.Identifier.Code == "en") { UIManager.instance._txtDescription.text = _equipment.engDescription; }
            //localization
        }
    }
}
