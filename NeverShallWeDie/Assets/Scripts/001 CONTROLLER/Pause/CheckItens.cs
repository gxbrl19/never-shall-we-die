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

    void Start()
    {

    }

    void Update()
    {
        switch (_item)
        {
            case "Gold":
                _imageButton.enabled = true;
                break;
            case "Hammer":
                _imageButton.enabled = GameManager.instance._hammer == 1 ? true : false;
                break;
            case "Grimoire":
                _imageButton.enabled = GameManager.instance._grimoire == 1 ? true : false;
                break;
            case "Orb":
                _imageButton.enabled = true;
                break;
            case "Potentium":
                _imageButton.enabled = true;
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
        switch (_item)
        {
            case "Hammer":
                CheckHammer();
                break;
        }
    }

    void CheckHammer()
    {
        Debug.Log("descrição");
    }
}
