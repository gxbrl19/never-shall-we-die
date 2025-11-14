using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;

public class CheckSkills : MonoBehaviour
{
    [SerializeField] int _buttonID;
    [SerializeField] SkillObject _skill;
    Image _imageButton;

    private void Awake()
    {
        _imageButton = transform.Find("image_skill").GetComponent<Image>();
    }

    private void Update()
    {
        _imageButton.enabled = PlayerSkills.instance.skills.Contains(_skill.skill);
    }

    public void Check() //chamado no evento do botão
    {
        UIManager.instance._descriptions.SetActive(false);
        UIManager.instance._btnKeyboard.gameObject.SetActive(false);
        UIManager.instance._btnGamepad.gameObject.SetActive(false);

        if (!PlayerSkills.instance.skills.Contains(_skill.skill))
            return;

        UIManager.instance._descriptions.SetActive(true);
        //localization
        var currentLocale = LocalizationSettings.SelectedLocale;
        if (currentLocale.Identifier.Code == "pt-BR") { UIManager.instance._txtDescription.text = _skill.ptDescription; }
        else if (currentLocale.Identifier.Code == "en") { UIManager.instance._txtDescription.text = _skill.engDescription; }
        //localization

        if (_skill.gamepadButton == null || _skill.keyboardButton == null)
            return;

        UIManager.instance._btnKeyboard.gameObject.SetActive(true);
        UIManager.instance._btnGamepad.gameObject.SetActive(true);
        UIManager.instance._btnKeyboard.sprite = _skill.keyboardButton;
        UIManager.instance._btnGamepad.sprite = _skill.gamepadButton;
    }
}
