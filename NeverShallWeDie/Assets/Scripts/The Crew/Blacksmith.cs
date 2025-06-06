using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blacksmith : MonoBehaviour
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
            if (GameManager.instance._blacksmith == "MEET") { _meet.SetActive(true); } else { _meet.SetActive(false); }
        }

        if (_quest != null)
        {
            if (GameManager.instance._blacksmith == "QUEST") { _quest.SetActive(true); } else { _quest.SetActive(false); }
        }

        if (_complete != null)
        {
            if (GameManager.instance._blacksmith == "COMPLETE") { _complete.SetActive(true); } else { _complete.SetActive(false); }
        }

        if (_crew != null)
        {
            if (GameManager.instance._blacksmith == "CREW") { _crew.SetActive(true); } else { _crew.SetActive(false); }
        }

        if (_other != null)
        {
            if (GameManager.instance._blacksmith == "OTHER") { _other.SetActive(true); } else { _other.SetActive(false); }
        }
    }

    public void NextState()
    {
        //animação de member joined
        if (GameManager.instance._blacksmith == "COMPLETE") { UIManager.instance.MemberJoined(_crewObject.name, _crewObject.draw); }

        //trocando o STATE do Ferreiro
        if (GameManager.instance._blacksmith == "MEET") { GameManager.instance._blacksmith = "QUEST"; }
        else if (InventorySystem.instance.items.Contains(Items.Hammer) && GameManager.instance._blacksmith == "QUEST") { GameManager.instance._blacksmith = "COMPLETE"; }
        else if (GameManager.instance._blacksmith == "COMPLETE") { GameManager.instance._blacksmith = "CREW"; }
    }
}
