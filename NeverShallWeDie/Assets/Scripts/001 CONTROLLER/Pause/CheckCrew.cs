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
    bool _enabled = false;

    private void Start()
    {
        _name = _crewObject.name;
    }

    private void Update()
    {
        switch (_name)
        {
            case "Marvin":
                _enabled = GameManager.instance._navigator == "CREW";
                gameObject.SetActive(_enabled);
                break;
            case "Selena":
                _enabled = GameManager.instance._shipwright == "CREW";
                gameObject.SetActive(_enabled);
                break;
            case "Lyra":
                _enabled = GameManager.instance._witch == "CREW";
                gameObject.SetActive(_enabled);
                break;
            case "Gribbit":
                _enabled = GameManager.instance._blacksmith == "CREW";
                gameObject.SetActive(_enabled);
                break;
        }
    }

    public void Check()
    {
        UIManager.instance.OpenCrewDescription(_crewObject.name, _crewObject.draw);

        //localization
        var currentLocale = LocalizationSettings.SelectedLocale;
        if (currentLocale.Identifier.Code == "pt-BR") { UIManager.instance._txtDescriptionCrew.text = _crewObject.ptDescription; }
        else if (currentLocale.Identifier.Code == "en") { UIManager.instance._txtDescriptionCrew.text = _crewObject.engDescription; }
        //localization
    }
}
