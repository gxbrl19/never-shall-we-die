using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;

public class Sign : MonoBehaviour
{
    [SerializeField] string _txtPortuguese;
    [SerializeField] string _txtEnglish;
    [SerializeField] Sprite _btnSprite;
    [SerializeField] Sprite _keySprite;
    [SerializeField] GameObject _canvas;
    [SerializeField] Image _btnImage;
    [SerializeField] TextMeshProUGUI _text;

    private void Update()
    {
        var currentLocale = LocalizationSettings.SelectedLocale;
        if (currentLocale.Identifier.Code == "pt-BR") { _text.text = _txtPortuguese; }
        else if (currentLocale.Identifier.Code == "en") { _text.text = _txtEnglish; }

        if (GameManager.instance._inputType == "Gamepad") { _btnImage.sprite = _btnSprite; }
        else if (GameManager.instance._inputType == "Keyboard") { _btnImage.sprite = _keySprite; }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player") || other.gameObject.layer == LayerMask.NameToLayer("Invencible"))
        {
            _canvas.gameObject.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player") || other.gameObject.layer == LayerMask.NameToLayer("Invencible"))
        {
            _canvas.gameObject.SetActive(false);
        }
    }
}
