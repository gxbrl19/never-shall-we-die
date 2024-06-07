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
using UnityEngine.Localization.Settings;
using UnityEngine.Audio;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;
    private Scene _currentScene;
    public int _sceneID;

    [BoxGroup("HUD")][SerializeField] private Image _healthBar;
    [BoxGroup("HUD")][SerializeField] private Image _healingBar;
    [BoxGroup("HUD")][SerializeField] private Text _txtGold;
    [BoxGroup("HUD")][SerializeField] private Image _fire;
    [BoxGroup("HUD")][SerializeField] private Image _air;
    [BoxGroup("HUD")][SerializeField] private Image _water;
    [BoxGroup("HUD")][SerializeField] private TextMeshProUGUI _txtInteract;
    [BoxGroup("HUD")][SerializeField] private GameObject _interact;
    [BoxGroup("HUD")][SerializeField] private GameObject _skullSave;
    [BoxGroup("HUD")][SerializeField] private GameObject _pnlBoss;
    [BoxGroup("HUD")] public Image _healthBoss;
    [BoxGroup("HUD")] public Text _txtBossName;

    [BoxGroup("Pause")] public bool _isPaused;
    [BoxGroup("Pause")] public RectTransform _menuPause;
    [BoxGroup("Pause")] public RectTransform[] _panels;
    private int _panelIndex;

    [Header("Equipments")]
    [BoxGroup("Pause Switch")] public Image[] _btnSwitch;
    [BoxGroup("Pause Switch")] public Sprite _spriteSwitch1;
    [BoxGroup("Pause Switch")] public Sprite _spriteSwitch;
    [BoxGroup("Pause Switch")] public GameObject[] _buttons;
    [BoxGroup("Pause Switch")] public GameObject _descriptions;
    [BoxGroup("Pause Switch")] public Image _btnKeyboard;
    [BoxGroup("Pause Switch")] public Image _btnGamepad;
    [BoxGroup("Pause Switch")] public Text _txtDescription;

    [Header("Skills")]
    [BoxGroup("Pause Switch")] public GameObject _pnlSkills;
    [BoxGroup("Pause Switch")] public Text _nameSkill;
    [BoxGroup("Pause Switch")] public Image _imgParchment;
    [BoxGroup("Pause Switch")] public Image _gamepadSkill;
    [BoxGroup("Pause Switch")] public Image _keyboardSkill;

    [Header("Crew")]
    [BoxGroup("Pause Switch")] public GameObject _pnlCrew;
    [BoxGroup("Pause Switch")] public Text _txtNameCrew;
    [BoxGroup("Pause Switch")] public Image _spriteMemberCrew;
    [BoxGroup("Pause Switch")] public Text _txtDescriptionCrew;

    [BoxGroup("Map")] public bool _inMap;
    [BoxGroup("Map")] public RectTransform _menuMap;
    [BoxGroup("Map")] public RectTransform[] _mapPanels;
    [BoxGroup("Map")] public Image[] _localizations;
    private int _mapID;
    private int _mapPanelIndex;

    [BoxGroup("Dialogue")] public GameObject _pnlDialogue;
    [BoxGroup("Dialogue")] public TextMeshProUGUI _txtName;
    [BoxGroup("Dialogue")] public TextMeshProUGUI _txtTalk;
    private bool _inDialogue = false;

    [BoxGroup("Crew")][Header("Helmsman")] public GameObject _pnlNavigate;
    [BoxGroup("Crew")] public GameObject _buttonYesNavigate;

    [BoxGroup("Fade")] public Image _pnlFade;

    [BoxGroup("Config")] public GameObject _pnlConfig;
    [BoxGroup("Config")] public GameObject[] _configInfo;
    [BoxGroup("Config")] public AudioMixer _mixer;
    [BoxGroup("Config")] public Dropdown _resolutionDropdown;
    Resolution[] _resolutions;

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

        Resolutions();
    }

    void Update()
    {
        HealthControl();
        HealingControl();
        GoldControl();
        SkillControl();
        DialogueControl();
    }

    #region HUD
    void HealthControl()
    {
        _healthBar.fillAmount = _health._currentHealth / _health._maxHealth;
    }

    public void HealingControl()
    {
        _healingBar.fillAmount = _health._currentMana / _health._maxMana;
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

    public void InteractPanel(bool show, string text)
    {
        _interact.SetActive(show);
        _txtInteract.text = text;
    }

    public void SaveEnabled()
    {
        _skullSave.SetActive(true);
    }

    public void SaveDisabled()
    {
        _skullSave.SetActive(false);
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
        _descriptions.SetActive(false);
        _pnlSkills.SetActive(false);
        _pnlCrew.SetActive(false);
        _pnlConfig.SetActive(false);

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

            if (_panelIndex == 4) //config
            {
                _pnlConfig.SetActive(true);
            }
        }
    }
    #endregion

    #region Map
    void InMap()
    {
        Time.timeScale = 0f;
        _menuMap.gameObject.SetActive(true);
        _inMap = true;
        _input.horizontal = 0f;

        SetLocalization();
        _mapPanelIndex = _mapID;
        CurrentMap();
    }

    void CancelMap()
    {
        Time.timeScale = 1f;
        _menuMap.gameObject.SetActive(false);
        _inMap = false;
        _player.EnabledControls();
    }

    void ChangeMap(string side)
    {
        int limit = _mapPanels.Length - 1;

        if (_inMap)
        {
            if (side == "Right" && _mapPanelIndex < limit)
            {
                _mapPanelIndex++;
            }
            else if (side == "Left" && _mapPanelIndex > 0)
            {
                _mapPanelIndex--;
            }

            MoveMap();
        }
    }

    void MoveMap()
    {
        for (int i = 0; i < _mapPanels.Length; i++)
        {
            if (i == _mapPanelIndex)
            {
                _mapPanels[i].DOAnchorPos(new Vector2(0f, 0f), .25f).SetUpdate(true).SetEase(Ease.Linear).SetUpdate(true).SetUpdate(UpdateType.Normal, true);
            }
            else if (i < _mapPanelIndex)
            {
                _mapPanels[i].DOAnchorPos(new Vector2(-384f, 0f), .25f).SetUpdate(true).SetEase(Ease.Linear).SetUpdate(true).SetUpdate(UpdateType.Normal, true);
            }
            else if (i > _mapPanelIndex)
            {
                _mapPanels[i].DOAnchorPos(new Vector2(384f, 0f), .25f).SetUpdate(true).SetEase(Ease.Linear).SetUpdate(true).SetUpdate(UpdateType.Normal, true);
            }
        }
    }

    public void CurrentMap()
    {
        for (int i = 0; i < _mapPanels.Length; i++)
        {
            if (i == _mapPanelIndex)
            {
                _mapPanels[i].DOAnchorPos(new Vector2(0f, 0f), 0f).SetUpdate(true).SetEase(Ease.Linear).SetUpdate(true).SetUpdate(UpdateType.Normal, true);
            }
            else if (i < _mapPanelIndex)
            {
                _mapPanels[i].DOAnchorPos(new Vector2(-384f, 0f), 0f).SetUpdate(true).SetEase(Ease.Linear).SetUpdate(true).SetUpdate(UpdateType.Normal, true);
            }
            else if (i > _mapPanelIndex)
            {
                _mapPanels[i].DOAnchorPos(new Vector2(384f, 0f), 0f).SetUpdate(true).SetEase(Ease.Linear).SetUpdate(true).SetUpdate(UpdateType.Normal, true);
            }
        }
    }

    public void SetLocalization() //verifica em qual ilha o player está para ativar a localização no mapa
    {
        if (_sceneID >= 4 && _sceneID <= 5) { _mapID = 0; }
        else if (_sceneID >= 6 && _sceneID <= 59) { _mapID = 1; }
        else if (_sceneID >= 60 && _sceneID <= 119) { _mapID = 2; }
        else if (_sceneID >= 120 && _sceneID <= 180) { _mapID = 3; }
        else if (_sceneID >= 181 && _sceneID <= 242) { _mapID = 4; }

        for (int i = 0; i < _localizations.Length; i++)
        {
            _localizations[i].enabled = false;
        }

        _localizations[_mapID].enabled = true;
    }
    #endregion

    #region Dialogue
    void DialogueControl()
    {
        _pnlDialogue.SetActive(_inDialogue);
    }

    public void SetName(string name)
    {
        _txtName.text = name;
    }

    public void EnableDialogue()
    {
        _inDialogue = true;
    }

    public void DisableDialogue()
    {
        _inDialogue = false;
        _txtName.text = "";
        _txtTalk.text = "";
    }
    #endregion

    #region Boss
    public void BossEnabled()
    {
        _pnlBoss.SetActive(true);
    }

    public void BossDisabled()
    {
        _pnlBoss.SetActive(false);
    }
    #endregion

    #region CrewFunctions

    public void ActivePanelNavigate()
    {
        //Time.timeScale = 0f;
        _player.DisableControls();
        _pnlNavigate.SetActive(true);
        EventSystem.current.SetSelectedGameObject(_buttonYesNavigate);
    }

    public void Navigate() //chamdo no botão Yes do pnl_navigate (UI Manager)
    {
        //Time.timeScale = 1f;
        _player.EnabledControls();
        PlayerPrefs.SetInt("Scene", 2); //cena do OpenWorld (configurar o index de acordo com o BuildSettings)
        SceneManager.LoadScene("Scenes/Load");
    }

    public void RecuseNavigate() //chamdo no botão No do pnl_navigate (UI Manager)
    {
        //Time.timeScale = 1f;
        _player.EnabledControls();
        _pnlNavigate.SetActive(false);
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
            if (_inMap) { return; }

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
            if (_isPaused) { return; }

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
            if (_isPaused) { SwitchPanel("Right"); }
            else if (_inMap) { ChangeMap("Right"); }
        }
    }

    public void SwitchLeft(InputAction.CallbackContext callback)
    {
        if (callback.started)
        {
            if (_isPaused) { SwitchPanel("Left"); }
            else if (_inMap) { ChangeMap("Left"); }
        }
    }
    #endregion

    #region Config
    public void SelectInfo(int panelID)
    {
        for (int i = 0; i < _configInfo.Length; i++)
        {
            _configInfo[i].SetActive(false);
        }

        _configInfo[panelID].SetActive(true);
    }
    #endregion

    #region Language
    public void ChangeLocal(int localeID)
    {
        StartCoroutine(SetLocale(localeID));
    }

    private IEnumerator SetLocale(int localeID)
    {
        yield return LocalizationSettings.InitializationOperation;
        LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[localeID];
        PlayerPrefs.SetInt("LocalKey", localeID);
    }
    #endregion

    #region Sound
    public void ClickSound(string type) //chamado na ação dos botões
    {
        AudioHUD.instance.SoundClick(type);
    }

    public void SetMasterVol(float volume)
    {
        _mixer.SetFloat("MasterVol", GetVol(volume));
    }

    public void SetMusicVol(float volume)
    {

        _mixer.SetFloat("MusicVol", GetVol(volume));
    }

    public void SetSFXVol(float volume)
    {
        _mixer.SetFloat("SFXVol", GetVol(volume));
    }

    public void SelectTest()
    {
        SceneManager.LoadScene("Scenes/Test/01");
    }

    float GetVol(float volume)
    {
        float _newVol = 0;
        _newVol = 20 * Mathf.Log10(volume);

        if (volume <= 0)
        {
            _newVol = -80;
        }

        return _newVol;
    }
    #endregion

    #region Resolution
    void Resolutions()
    {
        //initial Resolutions
        _resolutions = Screen.resolutions;
        _resolutionDropdown.ClearOptions();

        List<string> _options = new List<string>();

        int _currentResolutionIndex = 0;
        for (int i = 0; i < _resolutions.Length; i++)
        {
            string _option = _resolutions[i].width + " x " + _resolutions[i].height;
            _options.Add(_option);

            if (_resolutions[i].width == Screen.currentResolution.width && _resolutions[i].height == Screen.currentResolution.height)
            {
                _currentResolutionIndex = i;
            }
        }

        _resolutionDropdown.AddOptions(_options);
        _resolutionDropdown.value = _currentResolutionIndex;
        _resolutionDropdown.RefreshShownValue();
        //finish Resolutions
    }
    public void SetResolution(int resolutionIndex) //chamado no Dropdown
    {
        Resolution _resolution = _resolutions[resolutionIndex];
        Screen.SetResolution(_resolution.width, _resolution.height, Screen.fullScreen);
    }
    #endregion
}
