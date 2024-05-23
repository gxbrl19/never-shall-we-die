using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;

public enum STATE
{
    DISABLED,
    WAITING,
    TYPING
}

public class DialogueSystem : MonoBehaviour
{
    public DialogueData dialogueData;

    int currentText = 0;
    bool finished = false;

    TypeTextAnimation typeText;
    STATE state;
    Player _player;
    NPCController _npcController;

    private void Awake()
    {
        typeText = FindObjectOfType<TypeTextAnimation>();
        typeText.TypeFinished = OnTypeFinishe;

        _player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        _npcController = FindObjectOfType<NPCController>();
    }

    private void Start()
    {
        state = STATE.DISABLED;
    }

    private void Update()
    {
        if (state == STATE.DISABLED) return;

        switch (state)
        {
            case STATE.WAITING:
                Waiting();
                break;
            case STATE.TYPING:
                Typing();
                break;
        }
    }

    public void Next()
    {
        if (currentText == 0)
        {
            UIManager.instance.EnableDialogue();
        }

        //localization
        var currentLocale = LocalizationSettings.SelectedLocale;

        if (currentLocale.Identifier.Code == "pt-BR")
        {
            UIManager.instance.SetName(dialogueData.talkScript[currentText].portugueseName);
            typeText.fullText = dialogueData.talkScript[currentText++].portugueseText;
        }
        else if (currentLocale.Identifier.Code == "en")
        {
            UIManager.instance.SetName(dialogueData.talkScript[currentText].englishName);
            typeText.fullText = dialogueData.talkScript[currentText++].englishText;
        }
        //localization

        if (currentText == dialogueData.talkScript.Count) finished = true;

        typeText.StartTyping();
        state = STATE.TYPING;
    }

    void OnTypeFinishe()
    {
        state = STATE.WAITING;
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
                UIManager.instance.DisableDialogue();
                state = STATE.DISABLED;
                currentText = 0;
                finished = false;
                _player.EnabledControls();
                _npcController.NextState();
            }
        }
    }

    void Typing()
    {
        if (Input.GetButtonDown("Submit"))
        {
            typeText.Skip();
            state = STATE.WAITING;
        }
    }
}
