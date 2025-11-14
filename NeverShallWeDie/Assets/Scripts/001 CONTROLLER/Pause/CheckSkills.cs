using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
