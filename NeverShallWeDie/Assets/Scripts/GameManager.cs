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
    public int gold;
    public int[] flags;
    public int[] barrels;
    public int[] airCutblock;
    public int[] barriersLever;
    public int checkpointScene;
    public int direction;
    public string navigator;
    public string shipwright;
    public string witch;
    public string blacksmith;
    public int[] maps;
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
    [BoxGroup("PlayerStats")] public int _gold; //total de gold coletado

    [BoxGroup("ItemsInLevel")] public int[] _flags; //Flags já liberadas
    [BoxGroup("ItemsInLevel")] public int[] _barrels; //Barrels já destruídos
    [BoxGroup("ItemsInLevel")] public int[] _chests; //Chests já liberadas
    [BoxGroup("ItemsInLevel")] public int[] _airCutblock; //Air Cut Blocks já destruídos
    [BoxGroup("ItemsInLevel")] public int[] _barriersLever; //Barriers Lever já acionados

    [BoxGroup("Checkpoint")] public int _checkpointScene; //a cena atual que será o checkpoint
    [BoxGroup("Checkpoint")] public int _direction;

    [BoxGroup("Crew")] public string _navigator;
    [BoxGroup("Crew")] public string _shipwright;
    [BoxGroup("Crew")] public string _witch;
    [BoxGroup("Crew")] public string _blacksmith;

    [Header("Functions")]
    [BoxGroup("Crew")] public int[] _maps;

    //Enemy
    [BoxGroup("Enemy")] public int[] _bestiary;
    [BoxGroup("Enemy")] public int[] _bosses;

    //Prefabs
    [BoxGroup("Sound")] public float _masterVol;
    [BoxGroup("Sound")] public float _musicVol;
    [BoxGroup("Sound")] public float _sfxVol;

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

        _flags = new int[11];
        _airCutblock = new int[3]; //TODO: colocar o numero de AirCut Blocks que estarão no game
        _barrels = new int[50]; //TODO: colocar o numero de Barrels que estarão no game
        _chests = new int[50]; //TODO: colocar o numero de Chests que estarão no game
        _barriersLever = new int[50]; //TODO: colocar o numero de Barriers lever que estarão no game

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

        _navigator = "CREW";
        _shipwright = "MEET";
        _witch = "MEET";
        _blacksmith = "MEET";
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
        _data.gold = _gold;

        _data.flags = _flags;
        _data.barrels = _barrels;
        _data.airCutblock = _airCutblock;
        _data.barriersLever = _barriersLever;

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

    void LoadBasic()
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
            _gold = _data.gold;

            _flags = _data.flags;
            _barrels = _data.barrels;
            _airCutblock = _data.airCutblock;
            _barriersLever = _data.barriersLever;

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
        //
        if (Input.GetAxis("Horizontal") != 0f || Input.GetAxis("Vertical") != 0f)
        {
            _inputType = "Gamepad";
        }
    }
    #endregion Inputs
}
