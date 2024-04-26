using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingPlatform : MonoBehaviour
{
    private float _fallDelay = 0.009f;
    private Animator _animation;

    private void Start()
    {
        _animation = GetComponent<Animator>();
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            StartCoroutine(Fall());
        }
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            StartCoroutine(Return());
        }
    }

    private IEnumerator Fall()
    {
        yield return new WaitForSeconds(_fallDelay);
        _animation.SetBool("Collision", true);
    }

    private IEnumerator Return()
    {
        yield return new WaitForSeconds(_fallDelay);
        _animation.SetBool("Collision", false);
    }
}
