using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using Cinemachine;
using DG.Tweening;

public class CanvasOpenWorld : MonoBehaviour
{
    public static CanvasOpenWorld instance;

    [Header("Interact")]
    public GameObject _pnlInteract;

    [Header("Level Select")]
    public GameObject _pnlLevelSelect;
    public GameObject _btnYes;
    public GameObject _btnNo;
    public Text _islandName;

    //Pier
    [HideInInspector] public string _sceneName;

    ShipOpenWorld _ship;

    [Header("Progress")]
    public Text _countGold;

    private void Awake()
    {
        instance = this;        

        _ship = FindAnyObjectByType<ShipOpenWorld>();
    }

    void Start()
    {
        _countGold.text = GameManager.instance._gold.ToString();
    }

    void Update()
    {

    }

    public void EnterLevel() //chamado no botão YES do pnl_select
    {
        _ship.SavePos();

        _ship._canMove = true;
        PlayerPrefs.SetInt("Scene", 4); //cena do navio
        SceneManager.LoadScene("Scenes/Load");

        //passa o nome da cena para usar no ExitShip
        PlayerPrefs.SetString("Pier", _sceneName);
    }

    public void CancelLevel() //chamado no botão NO do pnl_select
    {
        _ship._canMove = true;
        _pnlLevelSelect.SetActive(false);
    }

    public void OpenLevelSelect(string name)
    {
        _islandName.text = name;
        _pnlLevelSelect.SetActive(true);
        EventSystem.current.SetSelectedGameObject(_btnYes);
    }
}
