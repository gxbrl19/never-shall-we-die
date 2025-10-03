using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    [HideInInspector] public float dir = 1;
    Rigidbody2D body;

    private void Awake()
    {
        body = GetComponentInParent<Rigidbody2D>();
    }

    private void Start()
    {
        body.AddForce(new Vector2(25f * dir, 13f), ForceMode2D.Impulse);
    }

    void StopMovement()
    {
        body.velocity = Vector2.zero;
    }
}
