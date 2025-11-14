using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;

public class CheckGems : MonoBehaviour
{
    [SerializeField] ItemObject _itemObject;

    Items _item;
    Image _imageButton;

    private void Awake()
    {
        _imageButton = transform.Find("image_gems").GetComponent<Image>();
    }

    void Start()
    {
        _item = _itemObject.item;
    }

    private void Update()
    {
        _imageButton.enabled = InventorySystem.instance.items.Contains(_itemObject.item);
    }

    public void Check() //chamado no evento do botão
    {
        UIManager.instance._descriptions.SetActive(false);
        UIManager.instance._btnKeyboard.gameObject.SetActive(false);
        UIManager.instance._btnGamepad.gameObject.SetActive(false);

        if (!InventorySystem.instance.items.Contains(_itemObject.item))
            return;

        UIManager.instance._descriptions.SetActive(true);
        //localization
        var currentLocale = LocalizationSettings.SelectedLocale;
        if (currentLocale.Identifier.Code == "pt-BR") { UIManager.instance._txtDescription.text = _itemObject.ptDescription; }
        else if (currentLocale.Identifier.Code == "en") { UIManager.instance._txtDescription.text = _itemObject.engDescription; }
        //localization
    }
}
