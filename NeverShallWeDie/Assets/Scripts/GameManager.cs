using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine.InputSystem;

[Serializable]
public class PlayerData
{
    public int intro;
    public List<Equipments> equipments;
    public List<Skills> skills;
    public List<Items> items;
    public int hpMax;
    public int hlMax;
    public int hpMpLevel;
    public int gold;
    public int katanaLevel;
    public int[] flags;
    public int[] barrels;
    public int[] chests;
    public int[] ancientStones;
    public int[] airCutblock;
    public int[] barriersLever;
    public int[] secret;
    public int secretDrawbridge;
    public int drawbridge;
    public int gateMechanism;
    public int gateBoss;
    public List<ShipUpgrade> shipUpgrades;
    public float shipPosX;
    public float shipPosY;
    public string lastPier;
    public int[] rocks;
    public int checkpointScene;
    public int direction;
    public string navigator;
    public string shipwright;
    public string witch;
    public string blacksmith;
    public int[] maps;
    public int hammer;
    public int grimoire;
    public int submarine;
    public int propulsion;
    public int artillery;
    public int[] bestiary;
    public int[] bosses;
}

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [HideInInspector] public int _indexSave = 0;
    [HideInInspector] public bool _save1 = false;
    [HideInInspector] public bool _save2 = false;
    [HideInInspector] public bool _save3 = false;

    [HideInInspector] public int _intro = 1;
    [HideInInspector] public List<Equipments> _equipments;
    [HideInInspector] public List<Skills> _skills;
    [HideInInspector] public List<Items> _inventory;
    [HideInInspector] public int _currentHP; //usado para manter o HP na troca de cena
    [HideInInspector] public int _currentHL; //usado para manter o HL na troca de cena
    public int _hpMax;
    public int _hlMax;
    public int _hpHlLevel;
    public int _gold; //total de gold coletado
    [HideInInspector] public int _katanaLevel;

    [HideInInspector] public int[] _flags; //Flags já liberadas
    [HideInInspector] public int[] _barrels; //Barrels já destruídos
    [HideInInspector] public int[] _chests; //Chests já liberadas
    public int[] _ancientStones; //Daily já liberadas
    [HideInInspector] public int[] _airCutblock; //Air Cut Blocks já destruídos
    [HideInInspector] public int[] _barriersLever; //Barriers Lever já acionados
    [HideInInspector] public int[] _secret; //Segredo do mapa do mapa 04 (gerado aleatoriamente)
    [HideInInspector] public int _secretDrawbridge; //Item encontrado numa folha
    [HideInInspector] public int _drawbridge; //Ponte do mapa 04 (0 = fechada | 1 = aberta)
    [HideInInspector] public int _gateMechanism; //se já pegou o gate mechanism
    [HideInInspector] public int _gateBoss; //Portão do Boss do mapa 05 (1 = consertado | 2 = aberto)

    [HideInInspector] public List<ShipUpgrade> _shipUpgrades;
    [HideInInspector] public float _shipPosX; //salva a posição do navio para quando voltar para o OpenWorld
    [HideInInspector] public float _shipPosY; //salva a posição do navio para quando voltar para o OpenWorld
    public string _lastPier = ""; //salva o ultimo Pier
    public int _sceneForLoad; //salva o ID da cena pra passar pro Loading
    [HideInInspector] public int[] _rocks; //Rocks já destruídos

    [HideInInspector] public int _checkpointScene; //a cena atual que será o checkpoint
    [HideInInspector] public int _direction;

    public string _navigator;
    public string _shipwright;
    public string _witch;
    public string _blacksmith;

    [Header("Functions")]
    [HideInInspector] public int[] _maps;

    //Enemy
    [HideInInspector] public int[] _bestiary;
    public int[] _bosses;

    //Prefabs
    [HideInInspector] public float _masterVol;
    [HideInInspector] public float _musicVol;
    [HideInInspector] public float _sfxVol;

    [HideInInspector] public int[] _doors; //mostra as portas já abertas

    [HideInInspector] public string _inputType;

    private string _path; //caminho para salvar o arquivo

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }

        _flags = new int[16];
        _airCutblock = new int[3]; //TODO: colocar o numero de AirCut Blocks que estarão no game
        _barrels = new int[50]; //TODO: colocar o numero de Barrels que estarão no game
        _chests = new int[50]; //TODO: colocar o numero de Chests que estarão no game
        _ancientStones = new int[9];
        _barriersLever = new int[50]; //TODO: colocar o numero de Barriers lever que estarão no game
        _doors = new int[6];
        _rocks = new int[4];

        //Crew
        _maps = new int[6];
        _maps[0] = 1; //DEMO (o mapa do navio será atribuido quando liberar o Navegador no jogo final)

        //Enemies
        _bosses = new int[6];

        _hpMax = 4;
        _hlMax = 2;
        _hpHlLevel = 1;
        _katanaLevel = 1;

        LoadBasic();
    }

    void Start()
    {
        //bloqueia o cursor do mouse
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        _navigator = "MEET"; //TODO: - passar para o MEET ao terminar a demo (OTHER para a demo)
        _shipwright = "COMPLETE"; //DEMO - passar para o COMPLETE ao terminar a demo
        _witch = "CREW"; //TODO: - passar para o MEET ao terminar a demo
        _blacksmith = "CREW"; //TODO: - passar para o MEET ao terminar a demo
    }

    void Update()
    {
        VerifyInputType();
    }

    public void SavePlayerPrefs(float masterVolume, float musicVolume, float sfxVolume)
    {
        PlayerPrefs.SetFloat("masterVol", masterVolume);
        PlayerPrefs.SetFloat("musicVol", musicVolume);
        PlayerPrefs.SetFloat("sfxVol", sfxVolume);

        _masterVol = masterVolume;
        _musicVol = musicVolume;
        _sfxVol = sfxVolume;
    }

    public void SaveGame()
    {
        UIManager.instance.SaveEnabled();

        _path = Application.persistentDataPath + "/playerSave" + _indexSave + ".sav";
        BinaryFormatter _binaryFormatter = new BinaryFormatter();
        FileStream _file = File.Create(_path); //cria o arquivo no caminho
        PlayerData _data = new PlayerData(); //instanciando objeto da classe PlayerData

        //atribui os valores do jogo ao objeto
        _data.intro = _intro;
        _data.equipments = _equipments;
        _data.skills = _skills;
        _data.items = _inventory;
        _data.hpMax = _hpMax;
        _data.hlMax = _hlMax;
        _data.hpMpLevel = _hpHlLevel;
        _data.gold = _gold;
        _data.katanaLevel = _katanaLevel;

        _data.flags = _flags;
        _data.barrels = _barrels;
        _data.chests = _chests;
        _data.ancientStones = _ancientStones;
        _data.airCutblock = _airCutblock;
        _data.barriersLever = _barriersLever;

        _data.secret = _secret;
        _data.secretDrawbridge = _secretDrawbridge;

        _data.drawbridge = _drawbridge;
        _data.gateMechanism = _gateMechanism;
        _data.gateBoss = _gateBoss;

        _data.shipUpgrades = _shipUpgrades;
        _data.shipPosX = _shipPosX;
        _data.shipPosY = _shipPosY;
        _data.lastPier = _lastPier;
        _data.rocks = _rocks;

        _data.checkpointScene = _checkpointScene;
        _data.direction = _direction;

        _data.navigator = _navigator;
        _data.shipwright = _shipwright;
        _data.witch = _witch;
        _data.blacksmith = _blacksmith;

        _data.maps = _maps;

        _data.bestiary = _bestiary;
        _data.bosses = _bosses;


        //envia os dados do objeto pra dentro de um arquivo
        _binaryFormatter.Serialize(_file, _data);

        _file.Close();
    }

    public void LoadBasic()
    {
        //verifica se existem os saves para mostrar nos botões do menu
        if (File.Exists(Application.persistentDataPath + "/playerSave1.sav"))
        {
            _save1 = true;
        }

        if (File.Exists(Application.persistentDataPath + "/playerSave2.sav"))
        {
            _save2 = true;
        }

        if (File.Exists(Application.persistentDataPath + "/playerSave3.sav"))
        {
            _save3 = true;
        }

        _masterVol = PlayerPrefs.GetFloat("masterVol");
        _musicVol = PlayerPrefs.GetFloat("musicVol");
        _sfxVol = PlayerPrefs.GetFloat("sfxVol");

        if (_masterVol == 0 && _musicVol == 0 && _sfxVol == 0)
        {
            _masterVol = 1;
            _musicVol = 1;
            _sfxVol = 1;
        }
    }

    public void LoadGame()
    {
        _path = Application.persistentDataPath + "/playerSave" + _indexSave + ".sav";
        if (File.Exists(_path)) //verifica se existe o arquivo de save
        {
            BinaryFormatter _binaryFormatter = new BinaryFormatter();
            FileStream _file = File.Open(_path, FileMode.Open); //abre o arquivo

            //recebe os dados de um arquivo para o objeto
            PlayerData _data = (PlayerData)_binaryFormatter.Deserialize(_file);
            _file.Close();


            //atribui os valores do objeto ao jogo
            _intro = _data.intro;
            _equipments = _data.equipments;
            _skills = _data.skills;
            _inventory = _data.items;
            _hpMax = _data.hpMax;
            _hlMax = _data.hlMax;
            _hpHlLevel = _data.hpMpLevel;
            _gold = _data.gold;
            _katanaLevel = _data.katanaLevel;

            _flags = _data.flags;
            _barrels = _data.barrels;
            _chests = _data.chests;
            _ancientStones = _data.ancientStones;
            _airCutblock = _data.airCutblock;
            _barriersLever = _data.barriersLever;

            _secret = _data.secret;
            _secretDrawbridge = _data.secretDrawbridge;

            _drawbridge = _data.drawbridge;
            _gateMechanism = _data.gateMechanism;
            _gateBoss = _data.gateBoss;

            _shipUpgrades = _data.shipUpgrades;
            _shipPosX = _data.shipPosX;
            _shipPosY = _data.shipPosY;
            _lastPier = _data.lastPier;
            _rocks = _data.rocks;

            _checkpointScene = _data.checkpointScene;
            _direction = _data.direction;

            _navigator = _data.navigator;
            _shipwright = _data.shipwright;
            _witch = _data.witch;
            _blacksmith = _data.blacksmith;

            _maps = _data.maps;

            _bestiary = _data.bestiary;
            _bosses = _data.bosses;
        }
    }

    public void DeleteSave(int indexSave)
    {
        // Constrói o caminho do arquivo do save a ser deletado
        string pathToDelete = Application.persistentDataPath + "/playerSave" + indexSave + ".sav";

        // Verifica se o arquivo existe antes de tentar deletá-lo
        if (File.Exists(pathToDelete)) { File.Delete(pathToDelete); } // Deleta o arquivo

        if (indexSave == 1) { _save1 = false; }
        else if (indexSave == 2) { _save2 = false; }
        else if (indexSave == 3) { _save3 = false; }
    }

    #region Inputs
    void VerifyInputType()
    {
        if (Input.anyKeyDown)
        {
            _inputType = "Keyboard";
        }
        //
        for (int i = 0; i < 20; i++) // 20 é um número arbitrário para os botões do gamepad
        {
            if (Input.GetKeyDown("joystick button " + i))
            {
                _inputType = "Gamepad";
                break;
            }
        }

        //lendo o WASD e analógicos
        bool keyboard = Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D);

        float stickX = Input.GetAxisRaw("Horizontal");
        float stickY = Input.GetAxisRaw("Vertical");

        if (keyboard) { _inputType = "Keyboard"; }
        if (!keyboard && (Mathf.Abs(stickX) > 0.1f || Mathf.Abs(stickY) > 0.1f)) { _inputType = "Gamepad"; }
    }
    #endregion Inputs
}
