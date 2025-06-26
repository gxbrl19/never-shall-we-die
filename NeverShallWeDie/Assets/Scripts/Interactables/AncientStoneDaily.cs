using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;

public enum STATE_DAILY
{
    DISABLED,
    WAITING,
    TYPING
}

public class AncientStoneDaily : MonoBehaviour
{
    public DialogueData dialogueData;

    int currentText = 0;
    bool finished = false;

    TypeTextAnimation typeText;
    STATE_DAILY state_daily;

    private void Awake()
    {
        typeText = FindObjectOfType<TypeTextAnimation>();
        typeText.TypeFinished = OnTypeFinishe;
    }

    private void Start()
    {
        state_daily = STATE_DAILY.DISABLED;
    }

    private void Update()
    {
        if (state_daily == STATE_DAILY.DISABLED) return;

        switch (state_daily)
        {
            case STATE_DAILY.WAITING:
                Waiting();
                break;
            case STATE_DAILY.TYPING:
                Typing();
                break;
        }
    }

    public void Next()
    {
        //localization
        var currentLocale = LocalizationSettings.SelectedLocale;

        if (currentLocale.Identifier.Code == "pt-BR")
        {
            typeText.fullText = dialogueData.talkScript[currentText++].portugueseText;
        }
        else if (currentLocale.Identifier.Code == "en")
        {
            typeText.fullText = dialogueData.talkScript[currentText++].englishText;
        }
        //localization

        if (currentText == dialogueData.talkScript.Count) finished = true;

        typeText.StartTyping();
        state_daily = STATE_DAILY.TYPING;
    }

    void OnTypeFinishe()
    {
        state_daily = STATE_DAILY.WAITING;
    }

    void Waiting()
    {
        if (Input.GetButtonDown("Submit"))
        {
            if (!finished)
            {
                Next();
            }
            else
            {
                UIManager.instance.CloseAncientStone();
                state_daily = STATE_DAILY.DISABLED;
                currentText = 0;
                finished = false;
                AudioHUD.instance.PlayTexting();
            }
        }
    }

    void Typing()
    {
        if (Input.GetButtonDown("Submit"))
        {
            typeText.Skip();
            state_daily = STATE_DAILY.WAITING;
            AudioHUD.instance.PlayTexting();
        }
    }
}
