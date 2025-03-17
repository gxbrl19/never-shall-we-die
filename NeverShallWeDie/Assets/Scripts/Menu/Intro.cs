using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using UnityEngine.SceneManagement;

public enum STATE_INTRO
{
    DISABLED,
    WAITING,
    TYPING
}

public class Intro : MonoBehaviour
{
    public DialogueData dialogueData;

    int currentText = 0;
    bool finished = false;

    TypeTextAnimation typeText;
    STATE_INTRO state_intro;

    private void Awake()
    {
        typeText = FindObjectOfType<TypeTextAnimation>();
        typeText.TypeFinished = OnTypeFinishe;
    }

    private void Start()
    {
        state_intro = STATE_INTRO.DISABLED;

        Next();
    }

    private void Update()
    {
        if (state_intro == STATE_INTRO.DISABLED) return;

        switch (state_intro)
        {
            case STATE_INTRO.WAITING:
                Waiting();
                break;
            case STATE_INTRO.TYPING:
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
        state_intro = STATE_INTRO.TYPING;
    }

    void OnTypeFinishe()
    {
        state_intro = STATE_INTRO.WAITING;
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
                state_intro = STATE_INTRO.DISABLED;
                currentText = 0;
                finished = false;
                //SceneManager.LoadScene("Scenes/OpenWorld"); //TODO: DEMO | remover após a demo
                SceneManager.LoadScene("Scenes/06/00");
            }
        }
    }

    void Typing()
    {
        if (Input.GetButtonDown("Submit"))
        {
            typeText.Skip();
            state_intro = STATE_INTRO.WAITING;
        }
    }
}
