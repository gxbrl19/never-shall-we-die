﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    public static MenuController instance;

    public AudioMixer _mixer;

    [Header("Save Select")]
    public Text _txtSave1;
    public Text _txtSave2;
    public Text _txtSave3;

    [Header("Audio")]
    public Slider _masterVolSlider;
    public Slider _musicVolSlider;
    public Slider _sfxVolSlider;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        SetVolumes();
    }

    private void Update()
    {
        UnlockSaveButtons();
    }

    void SetVolumes()
    {
        _masterVolSlider.value = GameManager.instance._masterVol;
        _musicVolSlider.value = GameManager.instance._musicVol;
        _sfxVolSlider.value = GameManager.instance._sfxVol;
    }

    void UnlockSaveButtons() //verifica os arquivos de save para atribuir aos botões
    {
        //Button Save 1
        if (GameManager.instance._save1)
        {
            _txtSave1.text = "CONTINUE";
        }
        else
        {
            _txtSave1.text = "NEW GAME";
        }

        //Button Save 2
        if (GameManager.instance._save2)
        {
            _txtSave2.text = "CONTINUE";
        }
        else
        {
            _txtSave2.text = "NEW GAME";
        }

        //Button Save 3
        if (GameManager.instance._save3)
        {
            _txtSave3.text = "CONTINUE";
        }
        else
        {
            _txtSave3.text = "NEW GAME";
        }

    }

    public void SelectSave(int saveIndex)
    {
        GameManager.instance._indexSave = saveIndex;

        Invoke("LoadScene", 1f);  //invoca o load com delay, para dar tempo de fazer o fade
    }

    private void LoadScene()
    {
        GameManager.instance.LoadGame();

        //TODO: verificar se é um novo save para carregar a primeira fase

        Time.timeScale = 1f;
        PlayerPrefs.SetInt("Scene", 3); //chama a cena do Mapa (configurar o index de acordo com o build settings)
        SceneManager.LoadScene("Scenes/Load");
        GameManager.instance.SavePlayerPrefs(_masterVolSlider.value, _musicVolSlider.value, _sfxVolSlider.value);
    }

    public void QuitGame()
    {
        Application.Quit();
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

    public void SetInputType(int index)
    {
        if (index == 0)
        {
            GameManager.instance._inputType = "Keyboard";
        }
        else if (index == 1)
        {
            GameManager.instance._inputType = "Gamepad";
        }

        GameManager.instance.SaveInput(index);
    }
}