using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CrewMembers
{
    Helmsman, Navigator, Witch, Blacksmith
}

public class NPCController : MonoBehaviour
{
    public CrewMembers _function;
    public GameObject _knowing;
    public GameObject _quest;
    public GameObject _complete;
    public GameObject _crew;


    void Update()
    {
        if (_knowing != null)
        {
            if (GameManager.instance._helmsman == "KNOWING") { _knowing.SetActive(true); } else { _knowing.SetActive(false); }
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
    }

    public void NextState()
    {
        if (_function == CrewMembers.Helmsman)
        {
            if (GameManager.instance._helmsman == "KNOWING") { GameManager.instance._helmsman = "QUEST"; }
            else if (GameManager.instance._helmsman == "COMPLETE") { GameManager.instance._helmsman = "CREW"; }
        }
    }
}
