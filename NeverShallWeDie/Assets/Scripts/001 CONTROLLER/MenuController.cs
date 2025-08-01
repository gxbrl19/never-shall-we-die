﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;

public class MenuController : MonoBehaviour
{
    public static MenuController instance;
    public Image _pnlFade;

    [Header("Save Select")]
    public Text _txtSave1;
    public Text _txtSave2;
    public Text _txtSave3;

    [Header("Delete Save")]
    int _idDelete;
    public GameObject _btn1;
    public GameObject _btn2;
    public GameObject _btn3;
    public GameObject _btnDelete1;
    public GameObject _btnDelete2;
    public GameObject _btnDelete3;
    public GameObject _btnDeleteFirst;
    public GameObject _panelDelete;

    BackgroundMusic _music;

    private void Awake()
    {
        instance = this;
        _music = FindAnyObjectByType<BackgroundMusic>();
    }

    private void Update()
    {
        UnlockSaveButtons();
    }

    void UnlockSaveButtons() //verifica os arquivos de save para atribuir aos botões
    {
        //Button Save 1
        if (GameManager.instance._save1)
        {
            _txtSave1.text = "CONTINUE";
            _btnDelete1.SetActive(true);
        }
        else
        {
            _txtSave1.text = "NEW GAME";
            _btnDelete1.SetActive(false);
        }

        //Button Save 2
        if (GameManager.instance._save2)
        {
            _txtSave2.text = "CONTINUE";
            _btnDelete2.SetActive(true);
        }
        else
        {
            _txtSave2.text = "NEW GAME";
            _btnDelete2.SetActive(false);
        }

        //Button Save 3
        if (GameManager.instance._save3)
        {
            _txtSave3.text = "CONTINUE";
            _btnDelete3.SetActive(true);
        }
        else
        {
            _txtSave3.text = "NEW GAME";
            _btnDelete3.SetActive(false);
        }

    }

    public void SelectSave(int saveIndex)
    {
        GameManager.instance._indexSave = saveIndex;
        _pnlFade.DOFade(1f, .3f);

        Invoke("LoadScene", 1f);  //invoca o load com delay, para dar tempo de fazer o fade
    }

    private void LoadScene() //chamado na função SelectSave
    {
        GameManager.instance.LoadGame();
        int scene = GameManager.instance._checkpointScene;

        if (scene == 0)
        {
            SceneManager.LoadScene("Scenes/Intro");
        }
        else
        {
            GameManager.instance._sceneForLoad = scene;
            SceneManager.LoadScene("Scenes/Load");
        }

        BackgroundMusic.instance.StartMusicInCheckpoint(scene);
    }

    public void PressDelete(int idDelete)
    {
        _idDelete = idDelete;
        _panelDelete.SetActive(true);
        EventSystem.current.SetSelectedGameObject(_btnDeleteFirst);
    }

    public void DeleteSave()
    {
        if (_idDelete == 1) { EventSystem.current.SetSelectedGameObject(_btn1); }
        else if (_idDelete == 2) { EventSystem.current.SetSelectedGameObject(_btn2); }
        else if (_idDelete == 3) { EventSystem.current.SetSelectedGameObject(_btn3); }

        GameManager.instance.DeleteSave(_idDelete);
        _panelDelete.SetActive(false);
    }

    public void CancelDelete()
    {
        if (_idDelete == 1) { EventSystem.current.SetSelectedGameObject(_btnDelete1); }
        else if (_idDelete == 2) { EventSystem.current.SetSelectedGameObject(_btnDelete2); }
        else if (_idDelete == 3) { EventSystem.current.SetSelectedGameObject(_btnDelete3); }

        _panelDelete.SetActive(false);
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

    public void SelectTest()
    {
        SceneManager.LoadScene("Scenes/Test/01");
    }
}
