using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class AudioHUD : MonoBehaviour
{
    public static AudioHUD instance;

    [SerializeField] EventReference _confirmBtn;
    [SerializeField] EventReference _backBtn;
    [SerializeField] EventReference _navigationBtn;
    [SerializeField] EventReference _selectBtn;
    [SerializeField] EventReference _sliderIncrease;
    [SerializeField] EventReference _sliderDecrease;
    [SerializeField] EventReference _openMap;
    [SerializeField] EventReference _closeMap;
    [SerializeField] EventReference _upgradeKatana;
    [SerializeField] EventReference _upgradeMP;
    [SerializeField] EventReference _upgradeShip;
    [SerializeField] EventReference _texting;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(instance);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlayConfirmButton()
    {
        RuntimeManager.PlayOneShot(_confirmBtn);
    }

    public void PlayBackButton()
    {
        RuntimeManager.PlayOneShot(_backBtn);
    }

    public void PlayNavigationButton()
    {
        RuntimeManager.PlayOneShot(_navigationBtn);
    }

    public void PlaySelectButton()
    {
        RuntimeManager.PlayOneShot(_selectBtn);
    }

    public void PlaySliderIncrease()
    {
        RuntimeManager.PlayOneShot(_sliderIncrease);
    }

    public void PlaySliderDecrease()
    {
        RuntimeManager.PlayOneShot(_sliderDecrease);
    }

    public void PlayOpenMap()
    {
        RuntimeManager.PlayOneShot(_openMap);
    }

    public void PlayCloseMap()
    {
        RuntimeManager.PlayOneShot(_closeMap);
    }

    public void PlayUpgradeKatana()
    {
        RuntimeManager.PlayOneShot(_upgradeKatana);
    }

    public void PlayUpgradeHP()
    {
        RuntimeManager.PlayOneShot(_upgradeMP);
    }

    public void PlayUpgradeShip()
    {
        RuntimeManager.PlayOneShot(_upgradeShip);
    }

    public void PlayTexting()
    {
        RuntimeManager.PlayOneShot(_texting);
    }
}
