using System;
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

public enum Character
{
    Navigator, Shipwright, Witch, Blacksmith, NavyGuard
}

public class DialogueSystem : MonoBehaviour
{
    public DialogueData dialogueData;
    public Character _character;

    int currentText = 0;
    bool finished = false;

    TypeTextAnimation typeText;
    STATE state;
    Player _player;
    PlayerInputs _input;
    Navigator _navigator;
    Blacksmith _blacksmith;
    Witch _witch;
    Shipwright _shipwright;

    private void Awake()
    {
        typeText = FindObjectOfType<TypeTextAnimation>();
        typeText.TypeFinished = OnTypeFinishe;

        _player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        _input = _player.GetComponent<PlayerInputs>();

        if (_character == Character.Navigator) { _navigator = FindObjectOfType<Navigator>(); }
        if (_character == Character.Blacksmith) { _blacksmith = FindObjectOfType<Blacksmith>(); }
        if (_character == Character.Witch) { _witch = FindObjectOfType<Witch>(); }
        if (_character == Character.Shipwright) { _shipwright = FindObjectOfType<Shipwright>(); }

        //_npcController = FindObjectOfType<NPCController>();
    }

    private void Start()
    {
        if (_character == Character.NavyGuard) { Invoke("Next", 5f); }
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
                _input.interact = false;
                _player.EnabledControls();
                AudioHUD.instance.PlayTexting();
                if (_navigator != null) { _navigator.NextState(); }
                if (_blacksmith != null) { _blacksmith.NextState(); }
                if (_witch != null) { _witch.NextState(); }
                if (_shipwright != null) { _shipwright.NextState(); }
            }
        }
    }

    void Typing()
    {
        if (Input.GetButtonDown("Submit"))
        {
            typeText.Skip();
            state = STATE.WAITING;
            AudioHUD.instance.PlayTexting();
        }
    }
}
