using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using Sirenix.OdinInspector;
using Cinemachine;
using DG.Tweening;
using TMPro;
using System;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;
    private Scene _currentScene;
    public int _sceneID;

    [BoxGroup("HUD")][SerializeField] private Text _txtHealth;
    [BoxGroup("HUD")][SerializeField] private Text _txtHealing;
    [BoxGroup("HUD")][SerializeField] private Text _txtGold;
    [BoxGroup("HUD")][SerializeField] private Image _fire;
    [BoxGroup("HUD")][SerializeField] private Image _air;
    [BoxGroup("HUD")][SerializeField] private Image _water;
    [BoxGroup("HUD")][SerializeField] private GameObject _interact;
    [BoxGroup("HUD")][SerializeField] private Text _txtInteract;

    [BoxGroup("Pause")] public bool _isPaused;
    [BoxGroup("Pause")] public RectTransform _menuPause;
    [BoxGroup("Pause")] public RectTransform[] _panels;
    private int _panelIndex;

    [BoxGroup("Pause Switch")] public Image[] _btnSwitch;
    [BoxGroup("Pause Switch")] public Sprite _spriteSwitch1;
    [BoxGroup("Pause Switch")] public Sprite _spriteSwitch;
    [BoxGroup("Pause Switch")] public GameObject[] _buttons;
    [BoxGroup("Pause Switch")] public GameObject _descriptions;

    [Header("Skills")]
    [BoxGroup("Pause Switch")] public GameObject _pnlSkills;
    [BoxGroup("Pause Switch")] public Text _nameSkill;
    [BoxGroup("Pause Switch")] public GameObject _fireSkill;
    [BoxGroup("Pause Switch")] public GameObject _airSkill;
    [BoxGroup("Pause Switch")] public GameObject _waterSkill;

    [BoxGroup("Map")] public bool _inMap;
    [BoxGroup("Map")] public RectTransform _menuMap;

    [Header("Game Over")]
    public bool _isGameOver;
    public GameObject _pnlGameOver;
    public GameObject _btnRestart;
    public GameObject _btnQuit;

    [Header("Fade")]
    public Image _pnlFade;

    Player _player;
    PlayerInputs _input;
    PlayerHealth _health;

    private void Awake()
    {
        instance = this;

        _player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        _input = _player.GetComponent<PlayerInputs>();
        _health = _player.GetComponent<PlayerHealth>();

        Time.timeScale = 1f;
        //_player.EnabledControls();
    }

    void Start()
    {
        _currentScene = SceneManager.GetActiveScene();
        _sceneID = _currentScene.buildIndex;
    }

    void Update()
    {
        HealthControl();
        HealingControl();
        GoldControl();
        SkillControl();
    }

    #region HUD
    void HealthControl()
    {
        _txtHealth.text = _health._currentHealth.ToString();

        //Vector3 _healthBarScale = _healthBar.rectTransform.localScale;
        //_healthBarScale.x = (float)_health._currentHealth / _health._maxHealth;
        //_healthBar.rectTransform.localScale = _healthBarScale;
    }

    public void HealingControl()
    {
        _txtHealing.text = _health._currenteMana.ToString();
    }

    void GoldControl()
    {
        _txtGold.text = GameManager.instance._gold.ToString();
    }

    void SkillControl()
    {
        _fire.fillAmount = _player._timeAirCut / _player._timeForSkills;
        _air.fillAmount = _player._timeTornado / _player._timeForSkills;
        _water.fillAmount = _player._timeWaterSpin / _player._timeForSkills;
    }

    public void InteractPanel(bool show, string inform)
    {        
        _txtInteract.text = inform;
        _interact.SetActive(show);
    }
    #endregion

    #region Pause

    public void ReturnToMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Scenes/MainMenu");
    }

    void InPause()
    {
        Time.timeScale = 0f;
        _menuPause.gameObject.SetActive(true);
        _isPaused = true;
        _input.horizontal = 0f;

        _panelIndex = 0;
        MovePanel();
    }

    void CancelPause()
    {
        Time.timeScale = 1f;
        _menuPause.gameObject.SetActive(false);
        _isPaused = false;
        _player.EnabledControls();
    }

    void SwitchPanel(string side)
    {
        int limit = _panels.Length - 1;

        if (_isPaused)
        {
            if (side == "Right" && _panelIndex < limit)
            {
                _panelIndex++;
            }
            else if (side == "Left" && _panelIndex > 0)
            {
                _panelIndex--;
            }

            MovePanel();
        }
    }

    void MovePanel()
    {
        for (int i = 0; i < _panels.Length; i++)
        {
            if (i == _panelIndex)
            {
                _panels[i].DOAnchorPos(new Vector2(0f, 0f), .25f).SetUpdate(true).SetEase(Ease.Linear).SetUpdate(true).SetUpdate(UpdateType.Normal, true);
                _btnSwitch[i].sprite = _spriteSwitch1;
                EventSystem.current.SetSelectedGameObject(_buttons[i]);
            }
            else if (i < _panelIndex)
            {
                _panels[i].DOAnchorPos(new Vector2(-200f, 0f), .25f).SetUpdate(true).SetEase(Ease.Linear).SetUpdate(true).SetUpdate(UpdateType.Normal, true);
                _btnSwitch[i].sprite = _spriteSwitch;
            }
            else if (i > _panelIndex)
            {
                _panels[i].DOAnchorPos(new Vector2(200f, 0f), .25f).SetUpdate(true).SetEase(Ease.Linear).SetUpdate(true).SetUpdate(UpdateType.Normal, true);
                _btnSwitch[i].sprite = _spriteSwitch;
            }
        }
    }

    public void ChangeSkillDescription()
    {

    }
    #endregion

    #region Map

    void InMap()
    {
        Time.timeScale = 0f;
        _menuMap.gameObject.SetActive(true);
        _inMap = true;
        _input.horizontal = 0f;
    }

    void CancelMap()
    {
        Time.timeScale = 1f;
        _menuMap.gameObject.SetActive(false);
        _inMap = false;
        _player.EnabledControls();
    }

    #endregion

    #region Fade
    public void FadeIn()
    {
        _pnlFade.DOFade(1f, .3f);
    }
    #endregion

    #region Inputs
    public void InputPause(InputAction.CallbackContext callback)
    {
        if (callback.started)
        {
            if (!_isPaused)
            {
                InPause();
            }
            else
            {
                CancelPause();
            }

            AudioHUD.instance.SoundClick("Pause");
        }
    }

    public void InputSelect(InputAction.CallbackContext callback)
    {
        if (callback.started)
        {
            if (!_inMap)
            {
                InMap();
            }
            else
            {
                CancelMap();
            }

            AudioHUD.instance.SoundClick("Pause");
        }
    }

    public void SwitchRight(InputAction.CallbackContext callback)
    {
        if (callback.started)
        {
            SwitchPanel("Right");
        }
    }

    public void SwitchLeft(InputAction.CallbackContext callback)
    {
        if (callback.started)
        {
            SwitchPanel("Left");
        }
    }
    #endregion
}
