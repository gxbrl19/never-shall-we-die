using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoidcallerSpell : MonoBehaviour
{
    private float speed = 25f;
    [HideInInspector] public Vector2 direction;
    Animator anim;
    bool getplayer;

    void Awake()
    {
        anim = GetComponent<Animator>();
    }

    void Start()
    {
        transform.localScale = new Vector2(direction.x, 1f);
    }

    private void Update()
    {
        if (getplayer) return;
        transform.position += (Vector3)(direction * speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //if (collision.gameObject.layer == LayerMask.NameToLayer("Player") || collision.gameObject.layer == LayerMask.NameToLayer("Door"))
        //GetPlayer();
    }

    void GetPlayer()
    {
        getplayer = true;
        anim.SetTrigger("Destroy");
    }

    public void DestroyObject() //chamado na animação
    {
        Destroy(gameObject);
    }
}
