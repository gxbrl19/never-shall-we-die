using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;
using Unity.VisualScripting;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;

public class CheckCrew : MonoBehaviour
{
    public CrewObject _crewObject;
    string _name;
    Text _textButton;

    private void Awake()
    {
        _textButton = GetComponentInChildren<Text>();
    }

    private void Start() {
        _name = _crewObject.name;
    }

    private void Update()
    {
        switch (_name)
        {
            case "Marvin":
                _textButton.enabled = GameManager.instance._navigator == "CREW";
                break;
            case "Selena":
                _textButton.enabled = GameManager.instance._shipwright == "CREW";
                break;
            case "Lyra":
                _textButton.enabled = GameManager.instance._witch == "CREW";
                break;
            case "Gribbit":
                _textButton.enabled = GameManager.instance._blacksmith == "CREW";
                break;
        }
    }

    public void Check()
    {
        if (_textButton.enabled)
        {
            UIManager.instance._pnlCrew.SetActive(true);
            UIManager.instance._txtNameCrew.text = _crewObject.name;
            UIManager.instance._spriteMemberCrew.sprite = _crewObject.draw;

            //localization
            var currentLocale = LocalizationSettings.SelectedLocale;
            if (currentLocale.Identifier.Code == "pt-BR") { UIManager.instance._txtDescriptionCrew.text = _crewObject.ptDescription; }
            else if (currentLocale.Identifier.Code == "en") { UIManager.instance._txtDescriptionCrew.text = _crewObject.engDescription; }
            //localization
        }
        else
        {
            UIManager.instance._pnlCrew.SetActive(false);
        }
    }
}
