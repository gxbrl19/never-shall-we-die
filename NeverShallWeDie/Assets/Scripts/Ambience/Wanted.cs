using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wanted : MonoBehaviour
{
    [SerializeField] int _bossID;
    Animator _animation;

    void Awake()
    {
        _animation = GetComponent<Animator>();
    }

    void Start()
    {
        if (GameManager.instance._bosses[_bossID] == 1)
        {
            _animation.SetBool("Finish", true);
        }
    }
}
