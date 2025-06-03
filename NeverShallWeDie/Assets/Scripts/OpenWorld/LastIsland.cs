using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;

public class LastIsland : MonoBehaviour
{
    [SerializeField] string _sceneName;
    [SerializeField] string _ptIslandName;
    [SerializeField] string _engIslandName;
    string _ptMessage = "Não é possível entrar nessa rota sem a bússola";
    string _engMessage = "You cannot enter this route without the compass";
    string _islandName;
    bool _playerTriggered;
    ShipOpenWorld _ship;
    ShipInput _input;

    private void Awake()
    {
        _ship = FindAnyObjectByType<ShipOpenWorld>();
        _input = _ship.gameObject.GetComponent<ShipInput>();
    }

    private void Update()
    {
        var currentLocale = LocalizationSettings.SelectedLocale;

        if (_playerTriggered && _input.interact)
        {
            if (!GameManager.instance._equipments.Contains(Equipments.Compass))
            {
                string _message = (currentLocale.Identifier.Code == "pt-BR") ? _ptMessage : _engMessage;
                Debug.Log(_message); //TODO: feedback para avisar que não pode passar;
                return;
            }

            _playerTriggered = false;
            _islandName = (currentLocale.Identifier.Code == "pt-BR") ? _ptIslandName : _engIslandName;
            //_ship.StopMove();
            _ship._canMove = false;
            AudioHUD.instance.PlaySelectButton();
            CanvasOpenWorld.instance.OpenLevelSelect(_islandName);
            CanvasOpenWorld.instance._sceneName = _sceneName;
        }
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player")) { _playerTriggered = true; }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player")) { _playerTriggered = false; }
    }
}
