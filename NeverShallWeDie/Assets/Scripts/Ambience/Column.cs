using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Column : MonoBehaviour
{
    Animator _animation;

    public UnityEvent ReturnGame;

    private void Awake()
    {
        _animation = GetComponent<Animator>();
    }

    public void OpenColumn() 
    {
        Invoke("SetTriggerColumn", 1.5f);
    }

    void SetTriggerColumn()
    {
        _animation.SetTrigger("Open");
        Invoke("ReturnToGame", 6f);
    }

    void ReturnToGame()
    {
        ReturnGame.Invoke();
    }
}
