using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class MapController : MonoBehaviour
{
    [SerializeField] int _sceneID;
    Button _button;

    private void Awake() {
        _button = GetComponent<Button>();
    }
    void Start()
    {
        if (UIManager.instance._sceneID == _sceneID) { EventSystem.current.SetSelectedGameObject(_button.gameObject); }    
    }
}
