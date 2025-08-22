using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceStake : MonoBehaviour
{
    Animator animator;
    Rigidbody2D rb;

    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnTriggerEnter2D(Collider2D _other)
    {
        if (_other.gameObject.layer == 8) //Ground
        {
            rb.velocity = Vector3.zero;
            rb.bodyType = RigidbodyType2D.Static;
            animator.SetTrigger("Break");
        }
    }
}
