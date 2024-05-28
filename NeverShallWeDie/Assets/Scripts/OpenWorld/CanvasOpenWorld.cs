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

    public void EnterLevel()
    {

    }

    public void CancelLevel()
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
