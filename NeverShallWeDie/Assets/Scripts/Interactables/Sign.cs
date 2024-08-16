using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class Sign : MonoBehaviour
{
    [SerializeField] string _txtPortuguese;
    [SerializeField] string _txtEnglish;
    [SerializeField] GameObject _canvas;
    [SerializeField] TextMeshProUGUI _text;

    private void Update()
    {
        var currentLocale = LocalizationSettings.SelectedLocale;
        if (currentLocale.Identifier.Code == "pt-BR") { _text.text = _txtPortuguese; }
        else if (currentLocale.Identifier.Code == "en") { _text.text = _txtEnglish; }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            _canvas.gameObject.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            _canvas.gameObject.SetActive(false);
        }
    }
}
