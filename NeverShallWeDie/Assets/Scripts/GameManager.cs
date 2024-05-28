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
    public int levelIndex;
    public List<Skills> skills;
    public int gold;
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

    [BoxGroup("Sound")] public float _masterVol;
    [BoxGroup("Sound")] public float _musicVol;
    [BoxGroup("Sound")] public float _sfxVol;

    [BoxGroup("Input")] public int _prefsInput;
    [BoxGroup("Input")] public string _inputType;

    [BoxGroup("ItemsInLevel")] public int[] _flags; //Flags já liberadas
    [BoxGroup("ItemsInLevel")] public int[] _airCutblock; //Air Cut Blocks já destruídos

    [BoxGroup("Checkpoint")] public int _checkpointScene; //a cena atual que será o checkpoint
    [BoxGroup("Checkpoint")] public int _direction;

    [BoxGroup("Crew")] public string _ship;
    [BoxGroup("Crew")] public string _helmsman;

    [BoxGroup("Maps")] public Vector3 _shipInitialPosition = new Vector3(-44f, 0f, 0f);
    [BoxGroup("Maps")] public string _currentPier;

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
        LoadBasic();
    }

    void Start()
    {
        //bloqueia o cursor do mouse
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        _currentPier = "06/H0"; //TODO: na cena do mapa, ao selecionar um Pier passar o valor para essa variavel (usar PlayerPrefs)
        _helmsman = "MEET";
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
        _path = Application.persistentDataPath + "/playerSave" + _indexSave + ".sav";
        BinaryFormatter _binaryFormatter = new BinaryFormatter();
        FileStream _file = File.Create(_path); //cria o arquivo no caminho
        PlayerData _data = new PlayerData(); //instanciando objeto da classe PlayerData

        //atribui os valores do jogo ao objeto
        _data.skills = _skills;
        _data.gold = _gold;

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
            _skills = _data.skills;
            _gold = _data.gold;
        }
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
