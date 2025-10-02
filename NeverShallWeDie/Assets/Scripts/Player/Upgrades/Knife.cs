using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knife : MonoBehaviour
{
    [HideInInspector] public float dir = 1;
    private Vector2 force = new Vector2(50f, 0f);

    void FixedUpdate()
    {
        transform.Translate(force * dir * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == 8 || other.gameObject.layer == 12)
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.layer == 8 || other.gameObject.layer == 12)
        {
            Destroy(gameObject);
        }
    }
}
