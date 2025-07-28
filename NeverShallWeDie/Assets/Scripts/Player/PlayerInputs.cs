using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputs : MonoBehaviour
{
    private float _horizontal;
    private float _vertical;
    private float _horizontal_r;
    private float _vertical_r;
    private bool _jumpPressed;
    private bool _jumpHeld;
    private bool _pressRoll;
    private bool _isAttacking;
    private bool _isFireCuting;
    private bool _isSliding;
    private bool _isParachuting;
    private bool _isGrabing;
    private bool _interact;
    private float rawInput;
    private Player _player;
    private PlayerAudio _audio;
    private PlayerAnimations _animation;
    private PlayerHealth _health;
    private PlayerCollision _collision;

    #region Properties

    public float horizontal
    {
        get { return _horizontal; }
        set { _horizontal = value; }
    }

    public float vertical
    {
        get { return _vertical; }
        set { _vertical = value; }
    }

    public float r_horizontal
    {
        get { return _horizontal_r; }
        set { _horizontal_r = value; }
    }

    public float r_vertical
    {
        get { return _vertical_r; }
        set { _vertical_r = value; }
    }

    public bool jumpPressed
    {
        get { return _jumpPressed; }
        set { _jumpPressed = value; }
    }

    public bool jumpHeld
    {
        get { return _jumpHeld; }
        set { _jumpHeld = value; }
    }

    public bool isAttacking
    {
        get { return _isAttacking; }
        set { _isAttacking = value; }
    }

    public bool isFireCuting
    {
        get { return _isFireCuting; }
        set { _isFireCuting = value; }
    }

    public bool pressRoll
    {
        get { return _pressRoll; }
        set { _pressRoll = value; }
    }

    public bool isSliding
    {
        get { return _isSliding; }
        set { _isSliding = value; }
    }

    public bool isParachuting
    {
        get { return _isParachuting; }
        set { _isParachuting = value; }
    }

    public bool isGrabing
    {
        get { return _isGrabing; }
        set { _isGrabing = value; }
    }

    public bool interact
    {
        get { return _interact; }
        set { _interact = value; }
    }
    #endregion

    void Start()
    {
        _player = GetComponent<Player>();
        _audio = GetComponent<PlayerAudio>();
        _animation = GetComponent<PlayerAnimations>();
        _health = GetComponent<PlayerHealth>();
        _collision = GetComponent<PlayerCollision>();
    }

    public void Move(InputAction.CallbackContext callback)
    {
        rawInput = callback.ReadValue<float>(); //sempre atualiza o input
        _horizontal = callback.ReadValue<float>(); //sempre atualiza o input
    }

    public float GetHorizontal()
    {
        if (_player.isDead || _player.canMove == false || _player.newSkillCollected || Time.timeScale == 0f)
            return 0f; //bloqueia o uso do input, mas não a leitura

        return rawInput;
    }

    public void ResetHorizontal()
    {
        rawInput = 0f;
    }

    public void Climb(InputAction.CallbackContext callback)
    {
        if (_player.isDead || Time.timeScale == 0f)
            return;

        _vertical = callback.ReadValue<float>();
    }

    public void R_Horizontal(InputAction.CallbackContext _callback)
    {
        if (_player.isDead || Time.timeScale == 0f)
            return;

        _horizontal_r = _callback.ReadValue<float>();
        if (_horizontal_r > 0f)
        {
            CinemachineShake.instance._offset_x = 4f;
        }
        else if (_horizontal_r < 0f)
        {
            CinemachineShake.instance._offset_x = -4f;
        }
        else
        {
            CinemachineShake.instance._offset_x = 0f;
        }
    }

    public void R_Vertical(InputAction.CallbackContext _callback)
    {
        if (_player.isDead || Time.timeScale == 0f)
            return;

        _vertical_r = _callback.ReadValue<float>();
        if (_vertical_r > 0f)
        {
            CinemachineShake.instance._offset_y = 4f;
        }
        else if (_vertical_r < 0f)
        {
            CinemachineShake.instance._offset_y = -4f;
        }
        else
        {
            CinemachineShake.instance._offset_y = 0f;
        }
    }

    public void Jump(InputAction.CallbackContext callback)
    {
        if (_player.isDead || _player.canMove == false || Time.timeScale == 0f) return;

        if (_player.isGrounded || _collision._onWall || _player.onClimbing || _player.onWater || _player.isGriding && !_player.isDoubleJumping)
        {
            if (callback.started)
                _jumpPressed = true;

            if (!_player.onWater)
                _player.playerMovement.canDoubleJump = true;

            _jumpHeld = callback.performed;
        }
        else if (!_player.isGrounded && PlayerSkills.instance.skills.Contains(Skills.AirGem) && !_player.isDoubleJumping && _player.playerMovement.canDoubleJump)
        {
            if (callback.started && !_collision._onWall)
            {
                _player.isDoubleJumping = true;
                _player.playerMovement.canDoubleJump = false;
            }
        }
    }

    public void Interact(InputAction.CallbackContext _callback)
    {
        if (_player.isDead || _player.isHealing || _isAttacking || _isFireCuting || Time.timeScale == 0f || _player.canMove == false)
            return;

        if (_callback.started && !_player.canGrab)
        {
            _interact = true;
            _isGrabing = false;
        }
        else if (_callback.started && _player.canGrab)
        {
            _isGrabing = true;
            _interact = false;
        }

        if (_callback.canceled)
        {
            _interact = false;
            _isGrabing = false;
        }
    }

    public void Healing(InputAction.CallbackContext _callback)
    {
        if (_player.isDead || !_player.isGrounded || _player.onHit || _isAttacking || _isFireCuting || Time.timeScale == 0f || _player.onWater || _player.canMove == false || _collision._onWall || _player.isGrabing || _player.isRolling || _player.isSliding)
            return;

        if (_callback.started)
        {
            _player.isHealing = true;
        }

        if (_callback.canceled)
        {
            _player.isHealing = false;
        }
    }

    public void SwordAttack(InputAction.CallbackContext _callback)
    {
        if (_player.isDead || _isAttacking || _isFireCuting || Time.timeScale == 0f || _player.onClimbing || _collision._onWall || _player.onHit || _player.isGrabing || _player.isRolling || _player.isSliding || _player.canMove == false)
            return;

        if (_callback.started && !_player.onWater && PlayerEquipment.instance.equipments.Contains(Equipments.Katana))
        {
            _isAttacking = true;
        }
        else if (_callback.started && _player.onWater && PlayerEquipment.instance.equipments.Contains(Equipments.Katana) && PlayerSkills.instance.skills.Contains(Skills.WaterGem))
        {
            if (_health._currentMana > 0 && _player.timeWaterSpin >= _player.timeForSkills)
            {
                _isAttacking = true;
                _player.timeWaterSpin = 0f; //reseta o tempo do water spin para poder fazer a contagem;
                _health.ManaConsumption(_player.waterSpinMana);
            }
        }
    }

    public void Slide(InputAction.CallbackContext _callback)
    {
        if (_player.isDead || !_player.isGrounded || _isAttacking || _isFireCuting || Time.timeScale == 0f || _player.onClimbing || _collision._onWall || _player.onWater || _player.onHit || _player.isGrabing || _player.isRolling || _player.isSliding || _player.canMove == false)
            return;

        if (_callback.started && PlayerEquipment.instance.equipments.Contains(Equipments.Boots))
        {
            _isSliding = true;
            _audio.PlaySlide();
        }
    }

    public void ParachuteButton(InputAction.CallbackContext callback)
    {
        if (_player.isDead || _player.isGrounded || _collision._onWall || _player.onClimbing || _player.onWater || _player.isGriding && !_player.isDoubleJumping) return;

        if (callback.started && PlayerEquipment.instance.equipments.Contains(Equipments.Parachute))
        {
            _isParachuting = true;
            _audio.PlayParachute();
        }
        else if (_player.isGrounded || callback.canceled)
        {
            _isParachuting = false;
        }
    }

    public void AirCutAttack(InputAction.CallbackContext _callback)
    {
        if (_player.isDead || _isAttacking || _isFireCuting || Time.timeScale == 0f || _player.onClimbing || _collision._onWall || _player.onHit || _player.isGrabing || _player.onWater || _player.isRolling || _player.canMove == false)
            return;

        if (PlayerEquipment.instance.equipments.Contains(Equipments.Katana) && PlayerSkills.instance.skills.Contains(Skills.FireGem))
        {
            if (_health._currentMana > 0 && _player.timeAirCut >= _player.timeForSkills)
            {
                isFireCuting = true;
                _player.timeAirCut = 0f; //reseta o tempo do aircut para poder fazer a contagem;
                _animation.OnAirCut();
                //.PlayAudio("aircut");
            }
        }
    }

    public void Roll(InputAction.CallbackContext _callback)
    {
        if (_player.isDead || !_player.isGrounded || _player.canMove == false || _player.onHit || _isAttacking || _player.onWater || Time.timeScale == 0f)
            return;

        if (_callback.started && !_player.canGrab)
        {
            _pressRoll = true;
        }

        if (_callback.canceled)
        {
            _pressRoll = false;
        }
    }

    public void OnHit()
    {
        isAttacking = false;
        isGrabing = false;
        isFireCuting = false;
        isParachuting = false;
        _player.isHealing = false;

        horizontal = 0f;
        vertical = 0f;
    }

}
