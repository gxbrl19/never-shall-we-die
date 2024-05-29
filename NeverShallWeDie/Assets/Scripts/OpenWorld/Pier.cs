using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;

public class Pier : MonoBehaviour
{
    [SerializeField] string _sceneName;
    [SerializeField] string _ptIslandName;
    [SerializeField] string _engIslandName;
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
        if (_playerTriggered && _input.submit)
        {
            _playerTriggered = false;
            var currentLocale = LocalizationSettings.SelectedLocale;
            _islandName = (currentLocale.Identifier.Code == "pt-BR") ? _ptIslandName : _engIslandName;
            //if (currentLocale.Identifier.Code == "pt-BR") { _islandName = _ptIslandName; } else if (currentLocale.Identifier.Code == "en") { _islandName = _engIslandName; }
            
            _ship._canMove = false;
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
