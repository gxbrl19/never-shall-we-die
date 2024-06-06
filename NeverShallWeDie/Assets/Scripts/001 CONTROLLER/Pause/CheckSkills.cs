using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CheckSkills : MonoBehaviour
{
    [SerializeField] SkillObject _skill;
    [SerializeField] Image _imageButton;
    Text _textButton;

    private void Awake()
    {
        _textButton = GetComponentInChildren<Text>();
    }

    private void Update()
    {
        if (PlayerSkills.instance.skills.Contains(_skill.skill))
        {
            _imageButton.enabled = true;
            if (_textButton != null) { _textButton.enabled = true; }
        }
        else
        {
            _imageButton.enabled = false;
            if (_textButton != null) { _textButton.enabled = false; }
        }
    }

    public void Check()
    {
        if (PlayerSkills.instance.skills.Contains(_skill.skill))
        {
            UIManager.instance._pnlSkills.SetActive(true);
            UIManager.instance._nameSkill.text = _skill.nameSkill;
            UIManager.instance._imgParchment.sprite = _skill.parchment;
            UIManager.instance._keyboardSkill.sprite = _skill.keyboardButton;
            UIManager.instance._gamepadSkill.sprite = _skill.gamepadButton;
        }
        else
        {
            UIManager.instance._pnlSkills.SetActive(false);
        }
    }
}
