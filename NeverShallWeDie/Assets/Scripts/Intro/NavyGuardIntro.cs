using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NavyGuardIntro : MonoBehaviour
{
    Animator _animation;

    void Awake()
    {
        _animation = GetComponent<Animator>();
    }

    void Update()
    {
        _animation.SetInteger("Intro", GameManager.instance._intro);
    }
}
