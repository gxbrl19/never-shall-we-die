using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimations : MonoBehaviour
{
    private Player _player;
    private PlayerInputs _input;
    private PlayerCollision _collision;
    private Animator _animation;

    [HideInInspector] public float xVelocity;
    [HideInInspector] public float yVelocity;

    void Start()
    {
        _player = GetComponent<Player>();
        _input = GetComponent<PlayerInputs>();
        _animation = GetComponent<Animator>();
        _collision = GetComponent<PlayerCollision>();
    }

    void Update()
    {
        AnimatorController();
    }

    void AnimatorController()
    {
        //Idle
        _animation.SetBool("IsGrounded", _player._isGrounded);

        //Fall
        _animation.SetFloat("Fall", _player._body.velocity.y);

        //Walk
        _animation.SetFloat("Speed", Mathf.Abs(xVelocity));

        //Climb
        _animation.SetBool("IsClimbing", _player._onClimbing);
        _animation.SetFloat("Vertical", Mathf.Abs(yVelocity));

        //Attack
        _animation.SetBool("IsAttacking", _input.isAttacking);

        //Dash
        _animation.SetBool("IsRolling", _input.isRolling);

        //Parachute
        _animation.SetBool("IsParachuting", _input.isParachuting);

        //WallSlide
        _animation.SetBool("IsWalling", _collision._onWall);

        //Healing
        _animation.SetBool("Healing", _player._healing);

        //Grid
        _animation.SetBool("IsGriding", _player._isGriding);
        _animation.SetBool("GridMove", xVelocity != 0 || yVelocity != 0);

        //Swim
        if (_player._onWater) { _animation.SetBool("IsSwimming", true); } else { _animation.SetBool("IsSwimming", false); }

        //Water Spin
        _animation.SetBool("WaterSpin", _player._inWaterSpin);

        //Slide
        _animation.SetBool("IsSliding", _input.isSliding);

        //Grab
        _animation.SetBool("IsGrabing", _player._isGrabing);

        if (_player._direction == 1 && _input.horizontal > 0 && _player._isGrabing)
        {
            _animation.SetBool("IsPulling", false); //puxar
            _animation.SetBool("IsPushing", true); //empurrar
        }
        else if (_player._direction == 1 && _input.horizontal < 0 && _player._isGrabing)
        {
            _animation.SetBool("IsPulling", true); //puxar
            _animation.SetBool("IsPushing", false); //empurrar
        }
        else if (_player._direction == -1 && _input.horizontal > 0 && _player._isGrabing)
        {
            _animation.SetBool("IsPulling", true); //puxar
            _animation.SetBool("IsPushing", false); //empurrar
        }
        else if (_player._direction == -1 && _input.horizontal < 0 && _player._isGrabing)
        {
            _animation.SetBool("IsPulling", false); //puxar
            _animation.SetBool("IsPushing", true); //empurrar
        }
        else
        {
            _animation.SetBool("IsPulling", false); //puxar
            _animation.SetBool("IsPushing", false); //empurrar
        }
    }

    public void OnAirCut()
    {
        _animation.SetBool("AirCut", true);
    }

    void StopAirCut()
    { //chamado na animação de Air Cut
        _input.isAirCuting = false;
        _animation.SetBool("AirCut", false);
    }

    public void OnHealing()
    {
        _animation.SetTrigger("Healing");
    }

    public void OnTornado()
    {
        _animation.SetBool("Tornado", true);
    }

    void StopTornado() //chamado na animação de Tornado
    {
        _input.isTornado = false;
        _animation.SetBool("Tornado", false);
    }

    public void OnDead()
    {
        _animation.SetTrigger("Dead");
    }

    public void OnHit()
    {
        _animation.SetTrigger("Hit");
    }

    public void SetPowerPickup()
    {
        _animation.SetTrigger("PowerPickup");
    }
}
