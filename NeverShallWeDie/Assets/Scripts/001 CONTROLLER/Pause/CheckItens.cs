using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;

public class CheckItens : MonoBehaviour
{
    [SerializeField] string _item;
    [SerializeField] Image _imageButton;
    [SerializeField] string _ptDesc;
    [SerializeField] string _engDesc;

    void Update()
    {
        switch (_item)
        {
            case "Hammer":
                _imageButton.enabled = InventorySystem.instance.items.Contains(Items.Hammer) ? true : false;
                break;
            case "Grimoire":
                _imageButton.enabled = InventorySystem.instance.items.Contains(Items.Grimoire) ? true : false;
                break;
            case "Cannon":
                _imageButton.enabled = GameManager.instance._artillery == 1 ? true : false;
                break;
            case "Propulsion":
                _imageButton.enabled = GameManager.instance._propulsion == 1 ? true : false;
                break;
            case "Submarine":
                _imageButton.enabled = GameManager.instance._submarine == 1 ? true : false;
                break;
            case "Secret":
                _imageButton.enabled = GameManager.instance._secretDrawbridge == 1 ? true : false;
                break;
            case "Crank":
                _imageButton.enabled = GameManager.instance._gateMechanism == 1 ? true : false;
                break;
            case "Key0":
                _imageButton.enabled = GameManager.instance._keys[0] == 1 ? true : false;
                break;
            case "Key1":
                _imageButton.enabled = GameManager.instance._keys[1] == 1 ? true : false;
                break;
            case "Key2":
                _imageButton.enabled = GameManager.instance._keys[2] == 1 ? true : false;
                break;
            case "Key3":
                _imageButton.enabled = GameManager.instance._keys[3] == 1 ? true : false;
                break;
            case "Key4":
                _imageButton.enabled = GameManager.instance._keys[4] == 1 ? true : false;
                break;
            case "Key5":
                _imageButton.enabled = GameManager.instance._keys[5] == 1 ? true : false;
                break;
        }
    }

    public void Check() //verificar se tem o item e mostrar no painel de descrição
    {
        UIManager.instance._pnlItems.SetActive(false);
        UIManager.instance._pnlSecret.SetActive(false);

        switch (_item)
        {
            case "Hammer":
                UIManager.instance._pnlItems.SetActive(InventorySystem.instance.items.Contains(Items.Hammer) ? true : false);
                break;
            case "Grimoire":
                UIManager.instance._pnlItems.SetActive(InventorySystem.instance.items.Contains(Items.Grimoire) ? true : false);
                break;
            case "Cannon":
                UIManager.instance._pnlItems.SetActive(GameManager.instance._artillery == 1 ? true : false);
                break;
            case "Propulsion":
                UIManager.instance._pnlItems.SetActive(GameManager.instance._propulsion == 1 ? true : false);
                break;
            case "Submarine":
                UIManager.instance._pnlItems.SetActive(GameManager.instance._submarine == 1 ? true : false);
                break;
            case "Secret":
                UIManager.instance._pnlSecret.SetActive(GameManager.instance._secretDrawbridge == 1 ? true : false);
                UIManager.instance.SecretSequence();
                break;
            case "Crank":
                UIManager.instance._pnlItems.SetActive(GameManager.instance._gateMechanism == 1 ? true : false);
                break;
            case "Key0":
                UIManager.instance._pnlItems.SetActive(GameManager.instance._keys[0] == 1 ? true : false);
                break;
            case "Key1":
                UIManager.instance._pnlItems.SetActive(GameManager.instance._keys[1] == 1 ? true : false);
                break;
            case "Key2":
                UIManager.instance._pnlItems.SetActive(GameManager.instance._keys[2] == 1 ? true : false);
                break;
            case "Key3":
                UIManager.instance._pnlItems.SetActive(GameManager.instance._keys[3] == 1 ? true : false);
                break;
            case "Key4":
                UIManager.instance._pnlItems.SetActive(GameManager.instance._keys[4] == 1 ? true : false);
                break;
            case "Key5":
                UIManager.instance._pnlItems.SetActive(GameManager.instance._keys[5] == 1 ? true : false);
                break;
        }

        if (_item == "Secret") { return; }

        //localization
        var currentLocale = LocalizationSettings.SelectedLocale;
        if (currentLocale.Identifier.Code == "pt-BR") { UIManager.instance._descItems.text = _ptDesc; }
        else if (currentLocale.Identifier.Code == "en") { UIManager.instance._descItems.text = _engDesc; }
        //localization
    }
}
