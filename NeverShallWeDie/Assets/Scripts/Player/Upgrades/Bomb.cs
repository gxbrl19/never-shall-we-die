using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class Bomb : MonoBehaviour
{
    [HideInInspector] public float dir = 1;
    Rigidbody2D body;

    [Header("FMOD Events")]
    [SerializeField] EventReference explode;

    private void Awake()
    {
        body = GetComponentInParent<Rigidbody2D>();
    }

    private void Start()
    {
        body.AddForce(new Vector2(25f * dir, 13f), ForceMode2D.Impulse);
    }

    public void PlaySound() //chamado na animação
    {
        RuntimeManager.PlayOneShot(explode);
    }
}
