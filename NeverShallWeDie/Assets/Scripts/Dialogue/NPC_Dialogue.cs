using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC_Dialogue : MonoBehaviour
{
    bool playerTriggered;
    DialogueSystem dialogueSystem;
    SpriteRenderer spriteRenderer;
    Player player;
    PlayerInputs playerInput;

    void Awake()
    {
        dialogueSystem = GetComponentInChildren<DialogueSystem>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        playerInput = player.GetComponent<PlayerInputs>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        FlipTowardsPlayer();

        if (playerTriggered && playerInput.pressInteract)
        {
            dialogueSystem.Next();
            player.DisableControls();
            playerInput.pressInteract = false;
            playerTriggered = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player") || other.gameObject.layer == LayerMask.NameToLayer("Invencible"))
        {
            playerTriggered = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player") || other.gameObject.layer == LayerMask.NameToLayer("Invencible"))
        {
            playerTriggered = false;
        }
    }

    private void FlipTowardsPlayer()
    {
        if (player == null) return;

        float dir = player.transform.position.x - transform.position.x;
        spriteRenderer.flipX = dir > 0 ? false : true;
    }
}
