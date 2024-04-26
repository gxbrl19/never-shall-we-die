using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using Cinemachine;
using DG.Tweening;

public class CanvasTopDown : MonoBehaviour
{
    public static CanvasTopDown instance;    

    [Header("Interact")]
    public GameObject _pnlInteract;

    [Header("Level Select")]
    public GameObject _pnlLevelSelect;
    public GameObject _btnYes;
    public GameObject _btnNo;   
    public Text _levelName;    

    [Header("Maps")]
    public Image _imgMap;
    public Text _countGold;
    public Text _textSkullIsland;

    [HideInInspector] public int _index;
    [HideInInspector] public bool _playerInTrigger;
    [HideInInspector] public float _horizontal;
    [HideInInspector] public float _vertical;
    [HideInInspector] public bool _submit;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        _countGold.text = GameManager.instance._gold.ToString();
    }

    void Update()
    {
        _pnlInteract.SetActive(_playerInTrigger);

        if (_playerInTrigger)
        {
            if (_submit)
            {
                _pnlLevelSelect.SetActive(true);
                PlayerTopDown.instance._canMove = false;                
            }
        }
    }

    public void SelectLevel()
    {
        Time.timeScale = 1f;
        PlayerPrefs.SetInt("Scene", _index);   
        SceneManager.LoadScene("Scenes/Load");
        PlayerTopDown.instance.SavePos();
    }

    public void CancelLevelSelect() //Botao CANCEL
    {
        _pnlLevelSelect.SetActive(false);
        PlayerTopDown.instance._canMove = true;
        _submit = false;
    }

    void ReturnMenu()
    {
        SceneManager.LoadScene("Scenes/MainMenu");
    }

    #region Inputs
    public void Horizontal(InputAction.CallbackContext callback)
    {
        _horizontal = callback.ReadValue<float>();
    }

    public void Vertical(InputAction.CallbackContext callback)
    {
        _vertical = callback.ReadValue<float>();
    }

    public void Submit(InputAction.CallbackContext callback)
    {
        if (callback.started)
        {
            _submit = true;
        }

        if (callback.canceled)
        {
            _submit = false;
        }
    }
    
    public void Cancel(InputAction.CallbackContext callback)
    {
        if (callback.started)
        {
            ReturnMenu();
        }
    }

    #endregion
}
