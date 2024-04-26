using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using DG.Tweening;

public class Menu : MonoBehaviour
{
    private bool _intro;
    private bool _enabled;

    [Header("Initial")]
    public GameObject _txtStart;

    [Header("Logo")]
    public RectTransform _logo;
    public RectTransform _referenceLogo;
    public RectTransform _skull;
    public RectTransform _referenceSkull;

    [Header("MainMenu")]
    public GameObject _emptyButton;
    public GameObject _firstButton;
    public RectTransform _imgMenu;
    public RectTransform _referenceMenu;
    public RectTransform _initialPosMenu;

    [Header("SaveMenu")]
    public RectTransform _saveButtons;
    public RectTransform _referenceSave;
    public RectTransform _initialPosSave;

    [Header("ConfigMenu")]
    public RectTransform _configButtons;
    public RectTransform _referenceConfig;
    public RectTransform _initialPosConfig;

    private void Start()
    {
        EventSystem.current.SetSelectedGameObject(_emptyButton); //desabilita todos os botões até finalizar a intro

        _txtStart.SetActive(false);
        _intro = true;
        _enabled = false;
        Invoke("StartIntro", 0.3f);
    }

    private void Update()
    {
        if (_intro == false && _enabled == false)
        {
            //identifica qualquer botão do teclado ou gamepad
            if (Input.anyKeyDown)
            {
                EnabledMainMenu();
                AudioHUD.instance.SoundClick("Pause");
            }

            for (int i = 0; i < 20; i++) // 20 é um número arbitrário para os botões do gamepad
            {
                if (Input.GetKeyDown("joystick button " + i))
                {
                    EnabledMainMenu();
                    AudioHUD.instance.SoundClick("Pause");
                }
            }
        }
    }

    public void ClickSound(string type) //chamado na ação dos botões
    {
        AudioHUD.instance.SoundClick(type);
    }

    public void StartIntro() //no inicio da Cena
    {
        Invoke("FinishIntro", 3.1f);
    }

    void FinishIntro()
    {
        _intro = false;
        _txtStart.SetActive(true);
    }

    public void EnabledMainMenu()
    {
        _enabled = true;
        _txtStart.SetActive(false);

        EventSystem.current.SetSelectedGameObject(_firstButton); //habilita os botões

        //habilita os Panels
        _imgMenu.gameObject.SetActive(true);
        _saveButtons.gameObject.SetActive(true);
        _configButtons.gameObject.SetActive(true);

        _imgMenu.DOMoveY(_referenceMenu.position.y, 0.3f).SetEase(Ease.InOutSine);
        _saveButtons.DOMoveY(_initialPosSave.position.y, 0.3f).SetEase(Ease.InOutBack);
        _configButtons.DOMoveX(_initialPosConfig.position.x, 0.3f).SetEase(Ease.InOutBack);
        _skull.DOMove(new Vector3(_referenceSkull.position.x, _referenceSkull.position.y, _referenceSkull.position.z), 0.4f).SetEase(Ease.InOutSine);
        _logo.DOMove(new Vector3(_referenceLogo.position.x, _referenceLogo.position.y, _referenceLogo.position.z), 0.4f).SetEase(Ease.InOutSine);
    }

    public void EnabledSaveMenu()
    {
        _saveButtons.DOMoveY(_referenceSave.position.y, 0.3f).SetEase(Ease.InOutBack);
        _imgMenu.DOMoveY(_initialPosMenu.position.y, 0.3f).SetEase(Ease.InOutSine);
    }

    public void EnabledConfigMenu()
    {
        _configButtons.DOMoveX(_referenceConfig.position.x, 0.3f).SetEase(Ease.InOutBack);
        _imgMenu.DOMoveY(_initialPosMenu.position.y, 0.3f).SetEase(Ease.InOutSine);
    }
}
