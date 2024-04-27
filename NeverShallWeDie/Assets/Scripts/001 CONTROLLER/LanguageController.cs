using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization.Settings;

public class LanguageController : MonoBehaviour
{
    private bool _active = false;

    void Start()
    {
        int ID = PlayerPrefs.GetInt("LocalKey", 0);
        ChangeLocal(ID);
    }

    public void ChangeLocal(int localeID)
    {
        if (_active) { return; }
        StartCoroutine(SetLocale(localeID));
    }

    private IEnumerator SetLocale(int localeID) {
        _active = true;
        yield return LocalizationSettings.InitializationOperation;
        LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[localeID];
        PlayerPrefs.SetInt("LocalKey", localeID);
        _active = false;
    }
}
