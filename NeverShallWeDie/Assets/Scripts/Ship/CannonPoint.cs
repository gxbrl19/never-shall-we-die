using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonPoint : MonoBehaviour
{
    public int _id;
    Animator _animation;

    private void Awake()
    {
        _animation = GetComponent<Animator>();
    }

    private void Start()
    {
        if (GameManager.instance._rocks[_id] == 1)
        {
            _animation.SetBool("Disable", true);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == 28) //AirCut (usar o aircut para não precisar criar outra layer)
        {
            _animation.SetBool("Destroy", true);
            GameManager.instance._rocks[_id] = 1;
        }
    }
}
