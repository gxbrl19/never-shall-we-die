using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Piece : MonoBehaviour
{
    Rigidbody2D _body;

    private void Awake()
    {
        _body = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        float x = Random.Range(-2f, 2f);
        float y = Random.Range(2f, 5f);
        _body.AddForce(new Vector2(x, y), ForceMode2D.Impulse);
        //Invoke("StopMovement", 0.3f);
    }

    void StopMovement()
    {
        _body.velocity = Vector2.zero;
    }
}
