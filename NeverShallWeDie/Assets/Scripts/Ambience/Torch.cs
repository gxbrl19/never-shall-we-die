using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class Torch : MonoBehaviour
{
    [SerializeField] GameObject particleHit;
    [SerializeField] GameObject torchLight;
    SpriteRenderer spriteRenderer;

    [Header("FMOD Events")]
    [SerializeField] EventReference breackSound;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("SwordAtk"))
        {
            spriteRenderer.enabled = false;
            torchLight.SetActive(false);
            RuntimeManager.PlayOneShot(breackSound);
            GameObject hit = Instantiate(particleHit, transform.position, particleHit.transform.rotation);
            Destroy(hit, 2f);
        }
    }
}
