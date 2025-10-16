using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class BombBlock : MonoBehaviour
{
    public int id;
    [SerializeField] private GameObject bombBlockPieces;
    [SerializeField] EventReference destroySound;
    Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    void Start()
    {
        bool disable = GameManager.instance._bombBlock[id] == 1;
        animator.SetBool("Disable", disable);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == 28) //Bomb
        {
            animator.SetBool("Disable", true);
            GameManager.instance._bombBlock[id] = 1;
            RuntimeManager.PlayOneShot(destroySound);
            DropPieces();
        }
    }

    private void DropPieces()
    {
        if (bombBlockPieces == null) { return; } //retorna se não tiver nenhum item

        for (int i = 0; i < 5; i++)
        {
            float pos = Random.Range(-1.5f, 1.5f);
            Instantiate(bombBlockPieces, new Vector2(transform.position.x + pos, transform.position.y + pos), Quaternion.identity);
        }
    }
}
