using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IcePlatform : MonoBehaviour
{
    float _breakDelay = 0.3f;
    int _life = 2;

    Animator _animation;

    private void Awake()
    {
        _animation = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("PlayerFoot"))
        {
            _life -= 1;
            if (_life == 1) { _animation.SetBool("Collision", true); } else if (_life <= 0) { StartCoroutine(Break()); }
        }
    }

    private IEnumerator Break()
    {
        yield return new WaitForSeconds(_breakDelay);
        _animation.SetBool("Break", true);
    }
}
