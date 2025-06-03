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
using Unity.VisualScripting;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;
    private Scene _currentScene;
    public int _sceneID;

    [BoxGroup("HUD")][SerializeField] private Image _healthBar;
    [BoxGroup("HUD")][SerializeField] private Image _healingBar;
    [BoxGroup("HUD")][SerializeField] private Text _healthTxt;
    [BoxGroup("HUD")][SerializeField] private Text _healingTxt;
    [BoxGroup("HUD")][SerializeField] private Text _txtGold;
    [BoxGroup("HUD")][SerializeField] private Text _txtGoldInPause;
    [BoxGroup("HUD")][SerializeField] private Text _txtStoneInPause;
    [BoxGroup("HUD")][SerializeField] private Text _txtGoldBuy;
    [BoxGroup("HUD")][SerializeField] private Animator _goldBuyAnimator;
    [BoxGroup("HUD")][SerializeField] private Image _imgFeedbackItem;
    [BoxGroup("HUD")][SerializeField] private Animator _feedbackItemAnimator;
    [BoxGroup("HUD")][SerializeField] private Image _fire;
    [BoxGroup("HUD")][SerializeField] private Image _air;
    [BoxGroup("HUD")][SerializeField] private Image _water;
    [BoxGroup("HUD")][SerializeField] private GameObject _skullSave;
    [BoxGroup("HUD")] public GameObject _pnlBoss;
    [BoxGroup("HUD")] public Image _healthBoss;
    [BoxGroup("HUD")] public Text _txtBossName;

    [BoxGroup("Pause")] public bool _isPaused;
    [BoxGroup("Pause")] public RectTransform _menuPause;
    [BoxGroup("Pause")] public RectTransform[] _panels;
    private int _panelIndex;
    [BoxGroup("Pause Switch")] public GameObject[] _buttons;

    [Header("Equipments")]
    [BoxGroup("Pause Switch")] public GameObject[] _buttonsEquipment;
    [HideInInspector] public int _buttonEquipID;
    [BoxGroup("Pause Switch")] public GameObject _descriptions;
    [BoxGroup("Pause Switch")] public Image _btnKeyboard;
    [BoxGroup("Pause Switch")] public Image _btnGamepad;
    [BoxGroup("Pause Switch")] public Text _txtDescription;
    [BoxGroup("Pause Switch")] public GameObject _buttonBackEquipDescription;

    [Header("Skills")]
    [BoxGroup("Pause Switch")] public GameObject[] _buttonsSkill;
    [HideInInspector] public int _buttonSkillID;
    [BoxGroup("Pause Switch")] public GameObject _pnlSkills;
    [BoxGroup("Pause Switch")] public Text _nameSkill;
    [BoxGroup("Pause Switch")] public Image _imgParchment;
    [BoxGroup("Pause Switch")] public Image _gamepadSkill;
    [BoxGroup("Pause Switch")] public Image _keyboardSkill;
    [BoxGroup("Pause Switch")] public GameObject _buttonBackSkillDescription;

    [Header("Items")]
    [BoxGroup("Pause Switch")] public GameObject _pnlItems;
    [BoxGroup("Pause Switch")] public Text _descItems;

    [Header("Crew")]
    [BoxGroup("Pause Switch")] public GameObject _pnlCrew;
    [BoxGroup("Pause Switch")] public Text _txtNameCrew;
    [BoxGroup("Pause Switch")] public Image _spriteMemberCrew;
    [BoxGroup("Pause Switch")] public Text _txtDescriptionCrew;
    [BoxGroup("Pause Switch")] public GameObject _buttonBackCrewDescription;

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

    [BoxGroup("Crew")][Header("Helm")] public GameObject _pnlNavigate;
    [BoxGroup("Crew")] public GameObject _buttonYesNavigate;
    [BoxGroup("Crew")][Header("Navigator")] public GameObject _pnlBuyMap;
    [BoxGroup("Crew")] public GameObject _buttonYesBuyMap;
    [BoxGroup("Crew")] public Text _txtMapPrice;
    [HideInInspector] public int _mapPrice;
    [HideInInspector] public int _mapBuyId;
    [BoxGroup("Crew")][Header("Blacksmith")] public GameObject _pnlUpKatana;
    [BoxGroup("Crew")] public GameObject _buttonYesUpKatana;
    [BoxGroup("Crew")] public Text _txtKatanaPrice;
    [BoxGroup("Crew")] public Text _txtCurrPotentium;
    [HideInInspector] public int _katanaPrice;
    [BoxGroup("Crew")][Header("Witch")] public GameObject _pnlUpHpMp;
    [BoxGroup("Crew")] public GameObject _buttonYesUpHpMp;
    [BoxGroup("Crew")] public Text _txtUpHpMpPrice;
    [BoxGroup("Crew")] public Text _txtCurrOrbs;
    [HideInInspector] public int _UpHpMpPrice;
    [HideInInspector] public int _xpStones;
    [BoxGroup("Crew")][Header("Shipwright")] public GameObject _pnlUPShip;
    [BoxGroup("Crew")] public GameObject _firstButtonShip;
    [BoxGroup("Crew")] public Button[] _buttonsUpgradeShip;
    [BoxGroup("Crew")] public Text[] _txtsUpgradeShip;
    [BoxGroup("Crew")] public Text _txtUpShipPrice;
    [HideInInspector] public int _upShipPrice;
    [BoxGroup("Crew")] public Animator _buyFeedback;

    [BoxGroup("New Member")] public bool _inNewMember;
    [BoxGroup("New Member")] public GameObject _pnlNewMember;
    [BoxGroup("New Member")] public Image _imgNewMember;
    [BoxGroup("New Member")] public Text _nameNewMember;

    [BoxGroup("Secret")] public GameObject _firstButtonKey;
    [BoxGroup("Secret")] public GameObject _pnlDrawbridge;
    [BoxGroup("Secret")] public GameObject[] _slotsDrawbridge;
    [BoxGroup("Secret")] public GameObject _pnlSecret;
    [BoxGroup("Secret")] public Image[] _secretSequence;

    [BoxGroup("Fade")] public Image _pnlFade;

    [BoxGroup("Config")] public GameObject _pnlConfig;
    [BoxGroup("Config")] public GameObject[] _configInfo;
    [BoxGroup("Config")] public Dropdown _resolutionDropdown;
    Resolution[] _resolutions;

    bool _inUIScreen;

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
        StatsController();
        SkillControl();
        DialogueControl();
    }

    #region HUD
    void StatsController()
    {
        _healthBar.fillAmount = _health._currentHealth / _health._maxHealth;
        _healthTxt.text = _health._maxHealth.ToString();
        _healingBar.fillAmount = _health._currentMana / _health._maxMana;
        _healingTxt.text = _health._maxMana.ToString();
        _txtGold.text = GameManager.instance._gold.ToString();
        _txtGoldInPause.text = GameManager.instance._gold.ToString();
        _txtStoneInPause.text = GameManager.instance._xp.ToString();
    }

    void SkillControl()
    {
        _fire.fillAmount = _player._timeAirCut / _player._timeForSkills;
        _air.fillAmount = _player._timeTornado / _player._timeForSkills;
        _water.fillAmount = _player._timeWaterSpin / _player._timeForSkills;
    }

    public void SaveEnabled()
    {
        _skullSave.SetActive(true);
        Invoke("SaveDisabled", 2f);
    }

    public void SaveDisabled() //chamado na função SaveEnabled()
    {
        _skullSave.SetActive(false);
    }

    public void FeedbackItem(Sprite image)
    {
        _imgFeedbackItem.sprite = image;
        _feedbackItemAnimator.SetTrigger("Start");
    }
    #endregion

    #region Pause

    public void ReturnToMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Scenes/MainMenu");
        GameManager.instance.LoadBasic();
        BackgroundMusic.instance.MusicControl(10); //musica do menu
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
        _pnlItems.SetActive(false);
        _pnlSecret.SetActive(false);
        AudioHUD.instance.PlayNavigationButton();

        for (int i = 0; i < _panels.Length; i++)
        {
            if (i == _panelIndex)
            {
                _panels[i].DOAnchorPos(new Vector2(0f, 0f), .25f).SetUpdate(true).SetEase(Ease.Linear).SetUpdate(true).SetUpdate(UpdateType.Normal, true);
                EventSystem.current.SetSelectedGameObject(_buttons[i]);
            }
            else if (i < _panelIndex)
            {
                _panels[i].DOAnchorPos(new Vector2(-500f, 0f), .25f).SetUpdate(true).SetEase(Ease.Linear).SetUpdate(true).SetUpdate(UpdateType.Normal, true);
            }
            else if (i > _panelIndex)
            {
                _panels[i].DOAnchorPos(new Vector2(500f, 0f), .25f).SetUpdate(true).SetEase(Ease.Linear).SetUpdate(true).SetUpdate(UpdateType.Normal, true);
            }

            if (_panelIndex == 5) //config
            {
                _pnlConfig.SetActive(true);
            }
        }
    }

    public void OpenEquipDescription()
    {
        _descriptions.SetActive(true);
        EventSystem.current.SetSelectedGameObject(_buttonBackEquipDescription);
    }

    public void ReturnToEquipDescription()
    {
        _descriptions.SetActive(false);
        EventSystem.current.SetSelectedGameObject(_buttonsEquipment[_buttonEquipID]);
    }

    public void OpenSkillDescription()
    {
        _pnlSkills.SetActive(true);
        EventSystem.current.SetSelectedGameObject(_buttonBackSkillDescription);
    }

    public void ReturnToSkillDescription()
    {
        _pnlSkills.SetActive(false);
        EventSystem.current.SetSelectedGameObject(_buttonsSkill[_buttonSkillID]);
    }

    public void OpenCrewDescription(string name, Sprite draw)
    {
        _pnlCrew.SetActive(true);
        _txtNameCrew.text = name;
        _spriteMemberCrew.sprite = draw;
        EventSystem.current.SetSelectedGameObject(_buttonBackCrewDescription);
    }

    public void ReturnToCrewScreen() //chamado no clique do botão de voltar no pnl_crews
    {
        _pnlCrew.SetActive(false);
        EventSystem.current.SetSelectedGameObject(_buttons[2]);
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

        CheckMap();
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
                AudioHUD.instance.PlayNavigationButton();
            }
            else if (i > _mapPanelIndex)
            {
                _mapPanels[i].DOAnchorPos(new Vector2(384f, 0f), .25f).SetUpdate(true).SetEase(Ease.Linear).SetUpdate(true).SetUpdate(UpdateType.Normal, true);
                AudioHUD.instance.PlayNavigationButton();
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

    public void CheckMap() //verifica se o mapa já foi comprado
    {
        for (int i = 0; i < _mapPanels.Length; i++)
        {
            if (GameManager.instance._maps[i] == 0) { _mapPanels[i].gameObject.SetActive(false); } else { _mapPanels[_mapPanelIndex].gameObject.SetActive(true); }
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

    #region Crew Joined
    public void MemberJoined(string name, Sprite image)
    {
        _inNewMember = true;
        _player.DisableControls();
        _pnlNewMember.SetActive(true);
        BackgroundMusic.instance.MusicControl(11);

        _imgNewMember.sprite = image;
        _nameNewMember.text = name;

        Invoke("FinishMemberJoined", 4f);
    }

    public void FinishMemberJoined() //chamado na função MemberJoined()
    {
        _inNewMember = false;
        _player.EnabledControls();
        _pnlNewMember.SetActive(false);
        BackgroundMusic.instance.BackToMapMusic();
    }
    #endregion

    #region Drawbridge

    public void ActivePanelDrawbridge()
    {
        _inUIScreen = true;
        _player.DisableControls();
        _pnlDrawbridge.SetActive(true);
        AudioHUD.instance.PlayBackButton();
        EventSystem.current.SetSelectedGameObject(_firstButtonKey);
    }

    public void RecuseDrawbridge() //chamado no botão back do pnl_drawbridge (UI Manager)
    {
        _inUIScreen = false;
        _player.EnabledControls();
        DrawbridgeMechanism.instance.CancelSelect();
        _pnlDrawbridge.SetActive(false);
        AudioHUD.instance.PlayBackButton();
    }

    public void SelectKey(ItemObject key) //chamado ao apertar um slot do pnl_drawbridge (UI Manager)
    {
        string _key = key.name.ToString();

        switch (_key)
        {
            case "Key0":
                if (InventorySystem.instance.items.Contains(Items.Key0)) { DrawbridgeMechanism.instance.AddSlot(key, 0); }
                break;
            case "Key1":
                if (InventorySystem.instance.items.Contains(Items.Key1)) { DrawbridgeMechanism.instance.AddSlot(key, 1); }
                break;
            case "Key2":
                if (InventorySystem.instance.items.Contains(Items.Key2)) { DrawbridgeMechanism.instance.AddSlot(key, 2); }
                break;
            case "Key3":
                if (InventorySystem.instance.items.Contains(Items.Key3)) { DrawbridgeMechanism.instance.AddSlot(key, 3); }
                break;
            case "Key4":
                if (InventorySystem.instance.items.Contains(Items.Key4)) { DrawbridgeMechanism.instance.AddSlot(key, 4); }
                break;
            case "Key5":
                if (InventorySystem.instance.items.Contains(Items.Key5)) { DrawbridgeMechanism.instance.AddSlot(key, 5); }
                break;
        }

        AudioHUD.instance.PlaySelectButton();
    }

    public void UpDrawbridge()
    {
        _inUIScreen = false;
        _player.EnabledControls();
        _pnlDrawbridge.SetActive(false);
    }

    public void SecretSequence()
    {
        for (int i = 0; i < GameManager.instance._secret.Length; i++)
        {
            int secret = GameManager.instance._secret[i];

            switch (secret)
            {
                case 0:
                    _secretSequence[i].color = Color.yellow;
                    break;
                case 1:
                    _secretSequence[i].color = Color.red;
                    break;
                case 2:
                    _secretSequence[i].color = Color.green;
                    break;
                case 3:
                    _secretSequence[i].color = Color.cyan;
                    break;
                case 4:
                    _secretSequence[i].color = Color.magenta;
                    break;
                case 5:
                    _secretSequence[i].color = Color.grey;
                    break;
            }
        }
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
        _inUIScreen = true;
        _player.DisableControls();
        _pnlNavigate.SetActive(true);
        EventSystem.current.SetSelectedGameObject(_buttonYesNavigate);
    }

    public void Navigate() //chamdo no botão Yes do pnl_navigate (UI Manager)
    {
        _inUIScreen = false;
        _player.EnabledControls();
        PlayerPrefs.SetInt("Scene", 2); //cena do OpenWorld (configurar o index de acordo com o BuildSettings)
        SceneManager.LoadScene("Scenes/Load");
    }

    public void RecuseNavigate() //chamdo no botão No do pnl_navigate (UI Manager)
    {
        _inUIScreen = false;
        _player.EnabledControls();
        _pnlNavigate.SetActive(false);
    }

    public void ActivePanelBuyMap()
    {
        _inUIScreen = true;
        _player.DisableControls();
        _txtMapPrice.text = _mapPrice.ToString();
        _pnlBuyMap.SetActive(true);
        EventSystem.current.SetSelectedGameObject(_buttonYesBuyMap);
        AudioHUD.instance.PlaySelectButton();
    }

    public void BuyMap() //chamado no botão Yes do pnl_buymap (UI Manager)
    {
        if (GameManager.instance._gold >= _mapPrice)
        {
            _inUIScreen = false;
            _player.EnabledControls();
            GameManager.instance._maps[_mapBuyId] = 1;
            _pnlBuyMap.SetActive(false);
            GameManager.instance._gold -= _mapPrice;
            AudioHUD.instance.PlayOpenMap();
            _txtGoldBuy.text = "-" + _mapPrice.ToString();
            _goldBuyAnimator.SetTrigger("Start");
        }
        else
        {
            _buyFeedback.SetTrigger("Start");
        }
    }

    public void RecuseBuyMap() //chamado no botão No do pnl_buymap (UI Manager)
    {
        _inUIScreen = false;
        _player.EnabledControls();
        _pnlBuyMap.SetActive(false);
    }

    public void ActivePanelUpKatana()
    {
        _inUIScreen = true;
        _player.DisableControls();
        _txtKatanaPrice.text = _katanaPrice.ToString();
        _txtCurrPotentium.text = _xpStones.ToString();
        _pnlUpKatana.SetActive(true);
        EventSystem.current.SetSelectedGameObject(_buttonYesUpKatana);
        AudioHUD.instance.PlaySelectButton();
    }

    public void UpgradeKatana() //chamado no botão Yes do pnl_upKatana (UI Manager)
    {
        if (GameManager.instance._gold >= _katanaPrice && _xpStones >= 4)
        {
            _inUIScreen = false;
            _player.EnabledControls();
            GameManager.instance._katanaLevel += 1;
            _pnlUpKatana.SetActive(false);
            GameManager.instance._gold -= _katanaPrice;
            GameManager.instance._xp -= 4;
            AudioHUD.instance.PlayUpgradeKatana();
            _txtGoldBuy.text = "-" + _katanaPrice.ToString();
            _goldBuyAnimator.SetTrigger("Start");

            UpgradeSword npc = FindObjectOfType<UpgradeSword>();
            npc.Action();
        }
        else
        {
            _buyFeedback.SetTrigger("Start");
        }
    }

    public void RecuseUpgradeKatana() //chamado no botão No do pnl_upKatana (UI Manager)
    {
        _inUIScreen = false;
        _player.EnabledControls();
        _pnlUpKatana.SetActive(false);
    }

    public void ActivePanelHpMp()
    {
        _inUIScreen = true;
        _player.DisableControls();
        _txtUpHpMpPrice.text = _UpHpMpPrice.ToString();
        _txtCurrOrbs.text = _xpStones.ToString();
        _pnlUpHpMp.SetActive(true);
        EventSystem.current.SetSelectedGameObject(_buttonYesUpHpMp);
        AudioHUD.instance.PlaySelectButton();
    }

    public void UpgradeHP() //chamado no botão HP do pnl_upHpMp (UI Manager)
    {
        if (GameManager.instance._gold >= _UpHpMpPrice && _xpStones >= 4)
        {
            _inUIScreen = false;
            _player.EnabledControls();
            _health._maxHealth += 5f;
            GameManager.instance._hpMax = _health._maxHealth;
            _pnlUpHpMp.SetActive(false);
            GameManager.instance._gold -= _UpHpMpPrice;
            GameManager.instance._xp -= 4;
            AudioHUD.instance.PlayUpgradeHP();
            _txtGoldBuy.text = "-" + _UpHpMpPrice.ToString();
            _goldBuyAnimator.SetTrigger("Start");

            UpgradePoints npc = FindObjectOfType<UpgradePoints>();
            npc.Action();
        }
        else
        {
            _buyFeedback.SetTrigger("Start");
        }
    }

    public void UpgradeMP() //chamado no botão MP do pnl_upHpMp (UI Manager)
    {
        if (GameManager.instance._gold >= _UpHpMpPrice && _xpStones >= 4)
        {
            _inUIScreen = false;
            _player.EnabledControls();
            _health._maxMana += 5f;
            GameManager.instance._mpMax = _health._maxMana;
            _pnlUpHpMp.SetActive(false);
            GameManager.instance._gold -= _UpHpMpPrice;
            GameManager.instance._xp -= 4;
            AudioHUD.instance.PlayUpgradeHP();
            _txtGoldBuy.text = "-" + _UpHpMpPrice.ToString();
            _goldBuyAnimator.SetTrigger("Start");

            UpgradePoints npc = FindObjectOfType<UpgradePoints>();
            npc.Action();
        }
        else
        {
            _buyFeedback.SetTrigger("Start");
        }
    }

    public void RecuseUpHpMp() //chamado no botão Cancel do pnl_upHpMp (UI Manager)
    {
        _inUIScreen = false;
        _player.EnabledControls();
        _pnlUpHpMp.SetActive(false);
    }

    public void ActivePanelShip()
    {
        _inUIScreen = true;
        _player.DisableControls();
        _pnlUPShip.SetActive(true);
        EventSystem.current.SetSelectedGameObject(_firstButtonShip);
        AudioHUD.instance.PlaySelectButton();
    }

    public void SelectedButtonShip(int id)
    {
        if (_txtsUpgradeShip[id].enabled == false) { return; }

        if (id == 0) { _upShipPrice = 100; } else if (id == 1) { _upShipPrice = 150; } else if (id == 2) { _upShipPrice = 200; }
        _txtUpShipPrice.text = _upShipPrice.ToString();
    }

    public void UpgradeShip(int id) //chamado no botão de upgrades do pnl_Ship (UI Manager)
    {
        if (_txtsUpgradeShip[id].enabled == false) { return; }

        if (id == 0 && ShipUpgrades.instance.shipUgrade.Contains(ShipUpgrade.Submarine)) { return; }
        else if (id == 1 && ShipUpgrades.instance.shipUgrade.Contains(ShipUpgrade.Propulsion)) { return; }
        else if (id == 2 && ShipUpgrades.instance.shipUgrade.Contains(ShipUpgrade.Cannon)) { return; }

        if (GameManager.instance._gold >= _upShipPrice)
        {
            _inUIScreen = false;
            _player.EnabledControls();

            if (id == 0)
            {
                GameManager.instance._shipUpgrades.Add(ShipUpgrade.Submarine);
                ShipUpgrades.instance.shipUgrade.Add(ShipUpgrade.Submarine);
            }
            else if (id == 1)
            {
                GameManager.instance._shipUpgrades.Add(ShipUpgrade.Propulsion);
                ShipUpgrades.instance.shipUgrade.Add(ShipUpgrade.Propulsion);
            }
            else if (id == 2)
            {
                GameManager.instance._shipUpgrades.Add(ShipUpgrade.Cannon);
                ShipUpgrades.instance.shipUgrade.Add(ShipUpgrade.Cannon);
            }

            _pnlUPShip.SetActive(false);
            GameManager.instance._gold -= _upShipPrice;
            //AudioHUD.instance.PlayUpgradeShip();
            _txtGoldBuy.text = "-" + _upShipPrice.ToString();
            _goldBuyAnimator.SetTrigger("Start");

            UpgradeShip npc = FindObjectOfType<UpgradeShip>();
            npc.Action();
        }
        else
        {
            _buyFeedback.SetTrigger("Start");
        }
    }

    public void RecuseUpShip() //chamado no botão Cancel do pnl_Ship (UI Manager)
    {
        _inUIScreen = false;
        _player.EnabledControls();
        _pnlUPShip.SetActive(false);
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
            if (_inMap || _inNewMember || _inUIScreen) { return; }

            if (!_isPaused)
            {
                InPause();
            }
            else
            {
                CancelPause();
            }

            AudioHUD.instance.PlaySelectButton();
        }
    }

    public void InputSelect(InputAction.CallbackContext callback)
    {
        if (callback.started)
        {
            if (_isPaused || _inNewMember || _inUIScreen) { return; }

            if (!_inMap && GameManager.instance._maps[1] == 1) //só habilita quando tiver o mapa da ilha 1
            {
                InMap();
                AudioHUD.instance.PlayOpenMap();
            }
            else if (_inMap)
            {
                CancelMap();
                AudioHUD.instance.PlayCloseMap();
            }
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
    public void ClickSound() //chamado na ação dos botões
    {
        AudioHUD.instance.PlaySelectButton();
    }

    public void SwitchButton() //chamado na ação dos botões
    {
        AudioHUD.instance.PlayNavigationButton();
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
