using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine.InputSystem;
using Sirenix.OdinInspector;

[Serializable]
public class PlayerData
{
    public List<Equipments> equipments;
    public List<Skills> skills;
    public float hpMax;
    public float mpMax;
    public int gold;
    public int katanaLevel;
    public int qtdPotentium;
    public int qtdOrb;
    public int[] potentiuns;
    public int[] orbs;
    public int[] flags;
    public int[] barrels;
    public int[] chests;
    public int[] airCutblock;
    public int[] barriersLever;
    public List<ShipUpgrade> shipUpgrades;
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
    public int[] bestiary;
    public int[] bosses;
}

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [BoxGroup("IndexSave")] public int _indexSave = 0;
    [BoxGroup("IndexSave")] public bool _save1 = false;
    [BoxGroup("IndexSave")] public bool _save2 = false;
    [BoxGroup("IndexSave")] public bool _save3 = false;

    [BoxGroup("PlayerStats")] public List<Equipments> _equipments;
    [BoxGroup("PlayerStats")] public List<Skills> _skills;
    [BoxGroup("PlayerStats")] public float _hpMax = 25f;
    [BoxGroup("PlayerStats")] public float _mpMax = 15f;
    [BoxGroup("PlayerStats")] public int _katanaLevel;
    [BoxGroup("PlayerStats")] public int _gold; //total de gold coletado
    [BoxGroup("PlayerStats")] public int _qtdPotentium; //total de Potentiuns coletado
    [BoxGroup("PlayerStats")] public int _qtdOrb; //total de Orbs points coletado

    [BoxGroup("ItemsInLevel")] public int[] _flags; //Flags já liberadas
    [BoxGroup("ItemsInLevel")] public int[] _barrels; //Barrels já destruídos
    [BoxGroup("ItemsInLevel")] public int[] _chests; //Chests já liberadas
    [BoxGroup("ItemsInLevel")] public int[] _airCutblock; //Air Cut Blocks já destruídos
    [BoxGroup("ItemsInLevel")] public int[] _barriersLever; //Barriers Lever já acionados
    [BoxGroup("ItemsInLevel")] public int _gateMechanism; //se já pegou o gate mechanism
    [BoxGroup("ItemsInLevel")] public int _gateBoss; //Portão do Boss do mapa 04 (1 = consertado | 2 = aberto)

    [BoxGroup("OpenWorld")] public List<ShipUpgrade> _shipUpgrades;
    [BoxGroup("OpenWorld")] public int[] _rocks; //Rocks já destruídos

    [BoxGroup("Checkpoint")] public int _checkpointScene; //a cena atual que será o checkpoint
    [BoxGroup("Checkpoint")] public int _direction;

    [BoxGroup("Crew")] public string _navigator;
    [BoxGroup("Crew")] public string _shipwright;
    [BoxGroup("Crew")] public string _witch;
    [BoxGroup("Crew")] public string _blacksmith;

    [Header("Functions")]
    [BoxGroup("Crew")] public int[] _maps;

    [Header("CrewItens")]
    [BoxGroup("Crew")] public int _hammer; //0 para false e 1 para true
    [BoxGroup("Crew")] public int _grimoire;
    [BoxGroup("Crew")] public int _submarine;
    [BoxGroup("Crew")] public int _propulsion;
    [BoxGroup("Crew")] public int _artillery;

    //Enemy
    [BoxGroup("Enemy")] public int[] _bestiary;
    [BoxGroup("Enemy")] public int[] _bosses;

    //Prefabs
    [BoxGroup("Sound")] public float _masterVol;
    [BoxGroup("Sound")] public float _musicVol;
    [BoxGroup("Sound")] public float _sfxVol;

    [BoxGroup("Door")] public int[] _keys; //mostra as chaves já encontradas
    [BoxGroup("Door")] public int[] _doors; //mostra as portas já abertas

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

        _flags = new int[13];
        _airCutblock = new int[3]; //TODO: colocar o numero de AirCut Blocks que estarão no game
        _barrels = new int[50]; //TODO: colocar o numero de Barrels que estarão no game
        _chests = new int[50]; //TODO: colocar o numero de Chests que estarão no game
        _barriersLever = new int[50]; //TODO: colocar o numero de Barriers lever que estarão no game
        _keys = new int[6];
        _doors = new int[6];
        _rocks = new int[2];

        //Crew
        _maps = new int[6];
        _maps[0] = 1; //DEMO (o mapa do navio será atribuido quando liberar o Navegador no jogo final)

        //Enemies
        _bosses = new int[6];

        LoadBasic();
    }

    void Start()
    {
        //bloqueia o cursor do mouse
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        _navigator = "MEET"; //TODO: - passar para o MEET ao terminar a demo (OTHER para a demo)
        _shipwright = "COMPLETE"; //DEMO - passar para o COMPLETE ao terminar a demo
        _witch = "MEET"; //TODO: - passar para o MEET ao terminar a demo
        _blacksmith = "MEET"; //TODO: - passar para o MEET ao terminar a demo
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
        _data.equipments = _equipments;
        _data.skills = _skills;
        _data.hpMax = _hpMax;
        _data.mpMax = _mpMax;
        _data.gold = _gold;
        _data.katanaLevel = _katanaLevel;
        _data.qtdPotentium = _qtdPotentium;
        _data.qtdOrb = _qtdOrb;

        _data.flags = _flags;
        _data.barrels = _barrels;
        _data.chests = _chests;
        _data.airCutblock = _airCutblock;
        _data.barriersLever = _barriersLever;

        _data.shipUpgrades = _shipUpgrades;
        _data.rocks = _rocks;

        _data.checkpointScene = _checkpointScene;
        _data.direction = _direction;

        _data.navigator = _navigator;
        _data.shipwright = _shipwright;
        _data.witch = _witch;
        _data.blacksmith = _blacksmith;

        _data.maps = _maps;

        _data.hammer = _hammer;
        _data.grimoire = _grimoire;

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
            _equipments = _data.equipments;
            _skills = _data.skills;
            _hpMax = _data.hpMax;
            _mpMax = _data.mpMax;
            _gold = _data.gold;
            _katanaLevel = _data.katanaLevel;
            _qtdPotentium = _data.qtdPotentium;
            _qtdOrb = _data.qtdOrb;

            _flags = _data.flags;
            _barrels = _data.barrels;
            _chests = _data.chests;
            _airCutblock = _data.airCutblock;
            _barriersLever = _data.barriersLever;

            _shipUpgrades = _data.shipUpgrades;
            _rocks = _data.rocks;

            _checkpointScene = _data.checkpointScene;
            _direction = _data.direction;

            _navigator = _data.navigator;
            _shipwright = _data.shipwright;
            _witch = _data.witch;
            _blacksmith = _data.blacksmith;

            _maps = _data.maps;

            _hammer = _data.hammer;
            _grimoire = _data.grimoire;

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
