using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Navigator : MonoBehaviour
{
    public CrewObject _crewObject;
    public GameObject _meet;
    public GameObject _quest;
    public GameObject _complete;
    public GameObject _crew;
    public GameObject _other;


    void Update()
    {
        if (_meet != null)
        {
            if (GameManager.instance._navigator == "MEET") { _meet.SetActive(true); } else { _meet.SetActive(false); }
        }

        if (_quest != null)
        {
            if (GameManager.instance._navigator == "QUEST") { _quest.SetActive(true); } else { _quest.SetActive(false); }
        }

        if (_complete != null)
        {
            if (GameManager.instance._navigator == "COMPLETE") { _complete.SetActive(true); } else { _complete.SetActive(false); }
        }

        if (_crew != null)
        {
            if (GameManager.instance._navigator == "CREW") { _crew.SetActive(true); } else { _crew.SetActive(false); }
        }

        if (_other != null)
        {
            if (GameManager.instance._navigator == "OTHER") { _other.SetActive(true); } else { _other.SetActive(false); }
        }
    }

    public void NextState()
    {
        //atribui o mapa do navio
        if (GameManager.instance._navigator == "OTHER") { GameManager.instance._maps[0] = 1; }

        //animação de member joined
        if (GameManager.instance._navigator == "COMPLETE") { UIManager.instance.MemberJoined(_crewObject.name, _crewObject.ptCrewFunction, _crewObject.engCrewFunction, _crewObject.draw); }

        //trocando o STATE do Navegador
        if (GameManager.instance._navigator == "MEET") { GameManager.instance._navigator = "QUEST"; }
        else if (GameManager.instance._navigator == "COMPLETE") { GameManager.instance._navigator = "OTHER"; }
        else if (GameManager.instance._navigator == "OTHER") { GameManager.instance._navigator = "CREW"; }
    }
}
