using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Press : MonoBehaviour
{
    public float _interval1;
    public float _interval2;

    private Animator _animation;

    private void Awake()
    {
        _animation = GetComponent<Animator>();
        _animation.enabled = false;
    }

    IEnumerator Start()
    {
        yield return new WaitForSeconds(Random.Range(_interval1, _interval2));
        _animation.enabled = true;
    }
}
