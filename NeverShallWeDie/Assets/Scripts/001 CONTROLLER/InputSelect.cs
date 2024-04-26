using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class InputSelect : MonoBehaviour
{
    Image _image;
    [SerializeField] Sprite _keyboardSprite;
    [SerializeField] Sprite _gamepadSprite;

    void Start()
    {
        _image = GetComponent<Image>();
    }

    private void Update()
    {
        if(GameManager.instance._inputType == "Gamepad") { _image.sprite = _gamepadSprite; } else { _image.sprite = _keyboardSprite;}
    }
}
