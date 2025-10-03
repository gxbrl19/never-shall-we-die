using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimations : MonoBehaviour
{
    private Player player;
    private Animator animator;

    [HideInInspector] public float xVelocity;
    [HideInInspector] public float yVelocity;

    void Start()
    {
        player = GetComponent<Player>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        AnimatorController();
    }

    void AnimatorController()
    {
        //INTRO
        animator.SetBool("Intro", GameManager.instance._intro == 0);

        //Idle
        animator.SetBool("IsGrounded", player.isGrounded);

        //Fall
        if (!player.isImpulsing)
            animator.SetFloat("Fall", player.rb.velocity.y);

        //Walk
        animator.SetFloat("Speed", Mathf.Abs(xVelocity));

        //Climb
        animator.SetBool("IsClimbing", player.onClimbing);
        animator.SetFloat("Vertical", Mathf.Abs(yVelocity));

        //Parachute
        animator.SetBool("IsParachuting", player.playerInputs.pressParachute);

        //Ledge
        animator.SetBool("IsWalling", player.onLedge);

        //Slide
        animator.SetBool("IsWallSliding", player.isWallSliding);

        //Impulse
        animator.SetBool("IsImpulsing", player.isImpulsing);

        //Grid
        animator.SetBool("IsGriding", player.isGriding);
        animator.SetBool("GridMove", xVelocity != 0 || yVelocity != 0 && player.isGriding);

        //Swim
        if (player.onWater) { animator.SetBool("IsSwimming", true); } else { animator.SetBool("IsSwimming", false); }

        //Grab
        animator.SetBool("IsGrabing", player.isGrabing);

        if (player.playerMovement.playerDirection == 1 && player.playerInputs.horizontal > 0 && player.isGrabing)
        {
            animator.SetBool("IsPulling", false); //puxar
            animator.SetBool("IsPushing", true); //empurrar
        }
        else if (player.playerMovement.playerDirection == 1 && player.playerInputs.horizontal < 0 && player.isGrabing)
        {
            animator.SetBool("IsPulling", true); //puxar
            animator.SetBool("IsPushing", false); //empurrar
        }
        else if (player.playerMovement.playerDirection == -1 && player.playerInputs.horizontal > 0 && player.isGrabing)
        {
            animator.SetBool("IsPulling", true); //puxar
            animator.SetBool("IsPushing", false); //empurrar
        }
        else if (player.playerMovement.playerDirection == -1 && player.playerInputs.horizontal < 0 && player.isGrabing)
        {
            animator.SetBool("IsPulling", false); //puxar
            animator.SetBool("IsPushing", true); //empurrar
        }
        else
        {
            animator.SetBool("IsPulling", false); //puxar
            animator.SetBool("IsPushing", false); //empurrar
        }
    }

    public void AnimAttack()
    {
        animator.Play("Player_Katana");
    }

    public void AnimHealing()
    {
        animator.Play("Player_Healing");
    }

    public void AnimDash()
    {
        animator.Play("Player_Dash");
    }

    public void AnimKnife()
    {
        animator.Play("Player_Knife");
    }

    public void AnimBomb()
    {
        animator.Play("Player_Bomb");
    }

    public void OnDead()
    {
        animator.SetTrigger("Dead");
    }

    public void OnHit()
    {
        animator.SetTrigger("Hit");
    }

    public void SetPowerPickup()
    {
        animator.SetTrigger("PowerPickup");
    }
}
