using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombBlock_Piece : MonoBehaviour
{
    public Sprite[] sprites;
    Rigidbody2D body;
    SpriteRenderer spriteRenderer;

    private void Awake()
    {
        body = GetComponentInParent<Rigidbody2D>();
        spriteRenderer = GetComponentInParent<SpriteRenderer>();
    }

    private void Start()
    {
        Sprite sprite = sprites[(int)Random.Range(0f, 4f)];
        spriteRenderer.sprite = sprite;
        float x = Random.Range(-7f, 7f);
        float y = Random.Range(22f, 25f);
        body.AddForce(new Vector2(x, y), ForceMode2D.Impulse);
    }
}
