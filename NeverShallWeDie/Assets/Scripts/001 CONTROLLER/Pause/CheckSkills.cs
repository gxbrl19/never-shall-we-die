using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CheckSkills : MonoBehaviour
{
    [SerializeField] int _buttonID;
    [SerializeField] SkillObject _skill;
    Image _imageButton;
    Text _textButton;

    private void Awake()
    {
        _imageButton = transform.Find("image_skill").GetComponent<Image>();
        _textButton = transform.Find("txt_skill").GetComponent<Text>();
    }

    private void Update()
    {
        _imageButton.enabled = PlayerSkills.instance.skills.Contains(_skill.skill);
        if (_textButton != null) { _textButton.enabled = PlayerSkills.instance.skills.Contains(_skill.skill); }
    }

    public void Check()
    {
        if (PlayerSkills.instance.skills.Contains(_skill.skill))
        {
            UIManager.instance.OpenSkillDescription();
            UIManager.instance._buttonSkillID = _buttonID;
            UIManager.instance._pnlSkills.SetActive(true);
            UIManager.instance._nameSkill.text = _skill.nameSkill;
            UIManager.instance._keyboardSkill.sprite = _skill.keyboardButton;
            UIManager.instance._gamepadSkill.sprite = _skill.gamepadButton;
        }
        else
        {
            UIManager.instance._pnlSkills.SetActive(false);
        }
    }
}
