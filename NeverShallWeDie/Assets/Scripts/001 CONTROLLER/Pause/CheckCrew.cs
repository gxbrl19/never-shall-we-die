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
    public Sprite spriteMember;
    public string nameMember;
    [TextArea(5, 10)] public string ptDescription;
    [TextArea(5, 10)] public string engDescription;

    Text _textButton;

    private void Awake()
    {
        _textButton = GetComponentInChildren<Text>();
    }

    private void Update()
    {
        switch (nameMember)
        {
            case "Caleb":
                _textButton.enabled = GameManager.instance._navigator == "CREW";
                break;
            case "Selena":
                _textButton.enabled = GameManager.instance._blacksmith == "CREW";
                break;
            case "Eleonore":
                _textButton.enabled = GameManager.instance._witch == "CREW";
                break;
            case "Gribbit":
                _textButton.enabled = GameManager.instance._shipwright == "CREW";
                break;
        }
    }

    public void Check()
    {
        if (_textButton.enabled)
        {
            UIManager.instance._pnlCrew.SetActive(true);
            UIManager.instance._txtNameCrew.text = nameMember;
            UIManager.instance._spriteMemberCrew.sprite = spriteMember;

            //localization
            var currentLocale = LocalizationSettings.SelectedLocale;
            if (currentLocale.Identifier.Code == "pt-BR") { UIManager.instance._txtDescriptionCrew.text = ptDescription; }
            else if (currentLocale.Identifier.Code == "en") { UIManager.instance._txtDescriptionCrew.text = engDescription; }
            //localization
        }
        else
        {
            UIManager.instance._pnlCrew.SetActive(false);
        }
    }
}
