using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GeneralInput : MonoBehaviour
{
    public Sprite _imgKeyboard;
    public Sprite _imgGamepad;

    Image _image;

    void Update()
    {
        _image = GetComponent<Image>();

        switch (GameManager.instance._inputType)
        {
            case "Keyboard":
                _image.sprite = _imgKeyboard;
                break;
            case "Gamepad":
                _image.sprite = _imgGamepad;
                break;
        }
        
    }
}
