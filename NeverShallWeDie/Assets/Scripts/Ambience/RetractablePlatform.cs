using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RetractablePlatform : MonoBehaviour
{
    [SerializeField] float _interval;
    Animator _animation;

    void Awake()
    {
        _animation = GetComponent<Animator>();
        _animation.enabled = false;
    }

    IEnumerator Start()
    {
        yield return new WaitForSeconds(_interval);
        _animation.enabled = true;
    }
}
