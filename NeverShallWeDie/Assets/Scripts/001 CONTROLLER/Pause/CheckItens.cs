using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;

public class CheckItens : MonoBehaviour
{
    [SerializeField] ItemObject _itemObject;
    Image _imageButton;
    Items _item;

    void Start()
    {
        _imageButton = transform.GetChild(0).gameObject.GetComponent<Image>();
    }

    void Update()
    {
        if (_itemObject == null) { return; }

        _item = _itemObject.item;

        switch (_item.ToString())
        {
            case "Hammer":
                _imageButton.sprite = InventorySystem.instance.items.Contains(Items.Hammer) ? _itemObject.sprite : _imageButton.sprite;
                break;
            case "Grimoire":
                _imageButton.sprite = InventorySystem.instance.items.Contains(Items.Grimoire) ? _itemObject.sprite : _imageButton.sprite;
                break;
            case "Cannon":
                _imageButton.sprite = InventorySystem.instance.items.Contains(Items.Cannon) ? _itemObject.sprite : _imageButton.sprite;
                break;
            case "Propulsion":
                _imageButton.sprite = InventorySystem.instance.items.Contains(Items.Propulsion) ? _itemObject.sprite : _imageButton.sprite;
                break;
            case "Submarine":
                _imageButton.sprite = InventorySystem.instance.items.Contains(Items.Submarine) ? _itemObject.sprite : _imageButton.sprite;
                break;
            case "Secret":
                _imageButton.sprite = InventorySystem.instance.items.Contains(Items.Secret) ? _itemObject.sprite : _imageButton.sprite;
                break;
            case "Crank":
                _imageButton.sprite = InventorySystem.instance.items.Contains(Items.Crank) ? _itemObject.sprite : _imageButton.sprite;
                break;
            case "Key0":
                _imageButton.sprite = InventorySystem.instance.items.Contains(Items.Key0) ? _itemObject.sprite : _imageButton.sprite;
                break;
            case "Key1":
                _imageButton.sprite = InventorySystem.instance.items.Contains(Items.Key1) ? _itemObject.sprite : _imageButton.sprite;
                break;
            case "Key2":
                _imageButton.sprite = InventorySystem.instance.items.Contains(Items.Key2) ? _itemObject.sprite : _imageButton.sprite;
                break;
            case "Key3":
                _imageButton.sprite = InventorySystem.instance.items.Contains(Items.Key3) ? _itemObject.sprite : _imageButton.sprite;
                break;
            case "Key4":
                _imageButton.sprite = InventorySystem.instance.items.Contains(Items.Key4) ? _itemObject.sprite : _imageButton.sprite;
                break;
            case "Key5":
                _imageButton.sprite = InventorySystem.instance.items.Contains(Items.Key5) ? _itemObject.sprite : _imageButton.sprite;
                break;
        }
    }

    public void Check() //verificar se tem o item e mostrar no painel de descrição
    {
        UIManager.instance._pnlItems.SetActive(false);
        UIManager.instance._pnlSecret.SetActive(false);

        switch (_item.ToString())
        {
            case "Hammer":
                UIManager.instance._pnlItems.SetActive(InventorySystem.instance.items.Contains(Items.Hammer) ? true : false);
                break;
            case "Grimoire":
                UIManager.instance._pnlItems.SetActive(InventorySystem.instance.items.Contains(Items.Grimoire) ? true : false);
                break;
            case "Cannon":
                UIManager.instance._pnlItems.SetActive(InventorySystem.instance.items.Contains(Items.Cannon) ? true : false);
                break;
            case "Propulsion":
                UIManager.instance._pnlItems.SetActive(InventorySystem.instance.items.Contains(Items.Propulsion) ? true : false);
                break;
            case "Submarine":
                UIManager.instance._pnlItems.SetActive(InventorySystem.instance.items.Contains(Items.Submarine) ? true : false);
                break;
            case "Secret":
                UIManager.instance._pnlSecret.SetActive(InventorySystem.instance.items.Contains(Items.Secret) ? true : false);
                UIManager.instance.SecretSequence();
                break;
            case "Crank":
                UIManager.instance._pnlItems.SetActive(InventorySystem.instance.items.Contains(Items.Crank) ? true : false);
                break;
            case "Key0":
                UIManager.instance._pnlItems.SetActive(InventorySystem.instance.items.Contains(Items.Key0) ? true : false);
                break;
            case "Key1":
                UIManager.instance._pnlItems.SetActive(InventorySystem.instance.items.Contains(Items.Key1) ? true : false);
                break;
            case "Key2":
                UIManager.instance._pnlItems.SetActive(InventorySystem.instance.items.Contains(Items.Key2) ? true : false);
                break;
            case "Key3":
                UIManager.instance._pnlItems.SetActive(InventorySystem.instance.items.Contains(Items.Key3) ? true : false);
                break;
            case "Key4":
                UIManager.instance._pnlItems.SetActive(InventorySystem.instance.items.Contains(Items.Key4) ? true : false);
                break;
            case "Key5":
                UIManager.instance._pnlItems.SetActive(InventorySystem.instance.items.Contains(Items.Key5) ? true : false);
                break;
        }

        //if (_item == "Secret") { return; }

        //localization
        //var currentLocale = LocalizationSettings.SelectedLocale;
        //if (currentLocale.Identifier.Code == "pt-BR") { UIManager.instance._descItems.text = _itemObject.ptDesc; }
        //else if (currentLocale.Identifier.Code == "en") { UIManager.instance._descItems.text = _itemObject.engDesc; }
        //localization
    }
}
