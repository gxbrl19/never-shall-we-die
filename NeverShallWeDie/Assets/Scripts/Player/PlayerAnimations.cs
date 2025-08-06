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
        if (!player.isDashing)
            animator.SetFloat("Fall", player.rb.velocity.y);

        //Walk
        animator.SetFloat("Speed", Mathf.Abs(xVelocity));

        //Climb
        animator.SetBool("IsClimbing", player.onClimbing);
        animator.SetFloat("Vertical", Mathf.Abs(yVelocity));

        //Attack
        animator.SetBool("IsAttacking", player.isAttacking);

        //DoubleJump
        animator.SetBool("DoubleJump", player.isDoubleJumping);

        //Parachute
        animator.SetBool("IsParachuting", player.playerInputs.pressParachute);

        //WallSlide
        animator.SetBool("IsWalling", player.playerCollision._onWall);

        //Healing
        animator.SetBool("Healing", player.isHealing);

        //Grid
        animator.SetBool("IsGriding", player.isGriding);
        animator.SetBool("GridMove", xVelocity != 0 || yVelocity != 0 && player.isGriding);

        //Swim
        if (player.onWater) { animator.SetBool("IsSwimming", true); } else { animator.SetBool("IsSwimming", false); }

        //Water Spin
        animator.SetBool("WaterSpin", player.inWaterSpin);

        //Dash
        animator.SetBool("IsDashing", player.isDashing);

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
        //animator.SetTrigger("Attack");
    }

    public void AnimParry()
    {
        animator.Play("Player_Parry");
        //animator.SetTrigger("Parry");
    }

    public void OnAirCut()
    {
        animator.SetBool("AirCut", true);
    }

    void StopAirCut()
    { //chamado na animação de Air Cut
        player.playerInputs.pressFireGem = false;
        animator.SetBool("AirCut", false);
    }

    public void OnHealing()
    {
        animator.SetTrigger("Healing");
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
