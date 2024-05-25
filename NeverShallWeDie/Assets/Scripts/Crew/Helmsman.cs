using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Helmsman : MonoBehaviour
{
    public GameObject _meet;
    public GameObject _quest;
    public GameObject _complete;
    public GameObject _crew;
    public GameObject _other;


    void Update()
    {
        if (_meet != null)
        {
            if (GameManager.instance._helmsman == "MEET") { _meet.SetActive(true); } else { _meet.SetActive(false); }
        }

        if (_quest != null)
        {
            if (GameManager.instance._helmsman == "QUEST") { _quest.SetActive(true); } else { _quest.SetActive(false); }
        }

        if (_complete != null)
        {
            if (GameManager.instance._helmsman == "COMPLETE") { _complete.SetActive(true); } else { _complete.SetActive(false); }
        }

        if (_crew != null)
        {
            if (GameManager.instance._helmsman == "CREW") { _crew.SetActive(true); } else { _crew.SetActive(false); }
        }

        if (_other != null)
        {
            if (GameManager.instance._helmsman == "OTHER") { _other.SetActive(true); } else { _other.SetActive(false); }
        }
    }

    public void NextState()
    {
        if (GameManager.instance._helmsman == "MEET") { GameManager.instance._helmsman = "QUEST"; }
        else if (GameManager.instance._helmsman == "COMPLETE") { GameManager.instance._helmsman = "OTHER"; }
        else if (GameManager.instance._helmsman == "OTHER") { GameManager.instance._helmsman = "CREW"; }
    }
}
