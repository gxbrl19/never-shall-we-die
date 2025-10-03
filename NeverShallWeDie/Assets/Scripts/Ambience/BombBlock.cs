using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class BombBlock : MonoBehaviour
{
    public int id;
    [SerializeField] EventReference destroySound;
    Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == 28) //Bomb
        {
            animator.SetBool("Destroy", true);
            GameManager.instance._bombBlock[id] = 1;
            RuntimeManager.PlayOneShot(destroySound);
        }
    }
}
