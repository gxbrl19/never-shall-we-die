using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    private void Awake()
    {
        typeText = FindObjectOfType<TypeTextAnimation>();
        typeText.TypeFinished = OnTypeFinishe;
        
        _player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
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

        UIManager.instance.SetName(dialogueData.talkScript[currentText].name);

        typeText.fullText = dialogueData.talkScript[currentText++].text;

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
        if (Input.GetButtonDown("Jump"))
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
            }
        }
    }

    void Typing()
    {
        if (Input.GetButtonDown("Jump"))
        {
            typeText.Skip();
            state = STATE.WAITING;
        }
    }
}
