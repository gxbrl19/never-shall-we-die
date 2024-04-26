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

    private void Start()
    {
        if (PlayerSkills.instance.skills.Contains(_skill.skill))
        {
            _imageButton.enabled = true;
            if (_textButton != null) { _textButton.text = _skill.name; }
        }
        else
        {
            _imageButton.enabled = false;
            if (_textButton != null) { _textButton.text = "???"; }
        }
    }

    public void Check()
    {
        if (PlayerSkills.instance.skills.Contains(_skill.skill))
        {
            UIManager.instance._pnlSkills.SetActive(true);

            switch (_skill.name)
            {
                case "Fire Insignia":
                    UIManager.instance._fireSkill.SetActive(true);
                    UIManager.instance._waterSkill.SetActive(false);
                    UIManager.instance._airSkill.SetActive(false);
                    UIManager.instance._nameSkill.text = "Faikatto";
                    break;
                case "Water Insignia":
                    UIManager.instance._fireSkill.SetActive(false);
                    UIManager.instance._waterSkill.SetActive(true);
                    UIManager.instance._airSkill.SetActive(false);
                    UIManager.instance._nameSkill.text = "Mizu no Kaiten";
                    break;
                case "Air Insignia":
                    UIManager.instance._fireSkill.SetActive(false);
                    UIManager.instance._waterSkill.SetActive(false);
                    UIManager.instance._airSkill.SetActive(true);
                    UIManager.instance._nameSkill.text = "Tatsumaki";
                    break;
            }
        }
        else
        {
            UIManager.instance._pnlSkills.SetActive(false);
        }
    }
}
