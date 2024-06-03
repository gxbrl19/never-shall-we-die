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
    private bool _isJumping;
    private bool _jumpHeld;
    private bool _isRolling;
    private bool _isAttacking;
    private bool _isAirCuting;
    private bool _isTornado;
    private bool _isSliding;
    private bool _isParachuting;
    private bool _isGrabing;
    private bool _interact;
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

    public bool isJumping
    {
        get { return _isJumping; }
        set { _isJumping = value; }
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

    public bool isAirCuting
    {
        get { return _isAirCuting; }
        set { _isAirCuting = value; }
    }

    public bool isTornado
    {
        get { return _isTornado; }
        set { _isTornado = value; }
    }

    public bool isRolling
    {
        get { return _isRolling; }
        set { _isRolling = value; }
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

    public void Move(InputAction.CallbackContext _callback)
    {
        if (_player._dead || _player._canMove == false || _player._newSkillCollected || Time.timeScale == 0f)
            return;

        _horizontal = _callback.ReadValue<float>();
    }

    public void Climb(InputAction.CallbackContext _callback)
    {
        if (_player._dead || Time.timeScale == 0f)
            return;

        _vertical = _callback.ReadValue<float>();
    }

    public void R_Horizontal(InputAction.CallbackContext _callback)
    {
        if (_player._dead || Time.timeScale == 0f)
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
        if (_player._dead || Time.timeScale == 0f)
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

    public void Jump(InputAction.CallbackContext _callback)
    {
        if (_player._dead || _player._canMove == false || Time.timeScale == 0f)
            return;

        if (_player._isGrounded || _collision._onWall || _player._onClimbing || _player._onWater)
        {
            if (_callback.started)
            {
                _isJumping = true;
            }

            _jumpHeld = _callback.performed;
        }
        else
        {
            if (_callback.started && PlayerEquipment.instance.equipments.Contains(Equipments.Parachute) && !_collision._onWall)
            {
                _isParachuting = true;
            }
            else if (_player._isGrounded || _callback.canceled)
            {
                _isParachuting = false;
            }
        }
    }

    public void Interact(InputAction.CallbackContext _callback)
    {
        if (_player._dead || _player._healing || _isAttacking || _isAirCuting || Time.timeScale == 0f || _player._canMove == false)
            return;

        if (_callback.started && !_player._canGrab)
        {
            _interact = true;
            _isGrabing = false;
        }
        else if (_callback.started && _player._canGrab)
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
        if (_player._dead || _isAttacking || _isAirCuting || Time.timeScale == 0f || _player._onWater || _player._canMove == false)
            return;

        if (_callback.started)
        {
            _player._healing = true;
        }

        if (_callback.canceled)
        {
            _player._healing = false;
        }
    }

    public void SwordAttack(InputAction.CallbackContext _callback)
    {
        if (_player._dead || _isAttacking || _isAirCuting || _isTornado || Time.timeScale == 0f || _player._onClimbing || _collision._onWall || _player._onHit || _player._isGrabing || _player._isDoubleJumping || _player._isRolling || _player._isSliding || _player._canMove == false)
            return;

        if (_callback.started && !_player._onWater && PlayerEquipment.instance.equipments.Contains(Equipments.Katana))
        {
            _isAttacking = true;
        }
        else if (_callback.started && _player._onWater && PlayerEquipment.instance.equipments.Contains(Equipments.Katana) && PlayerSkills.instance.skills.Contains(Skills.WaterSpin))
        {
            if (_health._currentMana > 0 && _player._timeWaterSpin >= _player._timeForSkills)
            {
                _isAttacking = true;
                _player._timeWaterSpin = 0f; //reseta o tempo do water spin para poder fazer a contagem;
                _health.ManaConsumption(_player._aircutMana);
            }
        }
    }

    public void Slide(InputAction.CallbackContext _callback)
    {
        if (_player._dead || !_player._isGrounded || _isAttacking || _isAirCuting || _isTornado || Time.timeScale == 0f || _player._onClimbing || _collision._onWall || _player._onWater || _player._onHit || _player._isGrabing || _player._isDoubleJumping || _player._isRolling || _player._isSliding || _player._canMove == false)
            return;

        if (_callback.started && PlayerEquipment.instance.equipments.Contains(Equipments.Boots))
        {
            _isSliding = true;
        }
    }

    public void TornadoAttack(InputAction.CallbackContext _callback)
    {
        if (_player._dead || !_player._isGrounded || _isAttacking || _isAirCuting || _isTornado || Time.timeScale == 0f || _player._onClimbing || _collision._onWall || _player._onHit || _player._isGrabing || _player._onWater || _player._isDoubleJumping || _player._isRolling || _player._canMove == false)
            return;

        if (PlayerEquipment.instance.equipments.Contains(Equipments.Katana) && PlayerSkills.instance.skills.Contains(Skills.Tornado))
        {
            if (_health._currentMana > 0 && _player._timeTornado >= _player._timeForSkills)
            {
                isTornado = true;
                _player._timeTornado = 0f; //reseta o tempo do tornado para poder fazer a contagem;
                _animation.OnTornado();
            }
        }
    }

    public void AirCutAttack(InputAction.CallbackContext _callback)
    {
        if (_player._dead || _isAttacking || _isAirCuting || _isTornado || Time.timeScale == 0f || _player._onClimbing || _collision._onWall || _player._onHit || _player._isGrabing || _player._onWater || _player._isDoubleJumping || _player._isRolling || _player._canMove == false)
            return;

        if (PlayerEquipment.instance.equipments.Contains(Equipments.Katana) && PlayerSkills.instance.skills.Contains(Skills.AirCut))
        {
            if (_health._currentMana > 0 && _player._timeAirCut >= _player._timeForSkills)
            {
                isAirCuting = true;
                _player._timeAirCut = 0f; //reseta o tempo do aircut para poder fazer a contagem;
                _animation.OnAirCut();
            }
        }
    }

    public void Roll(InputAction.CallbackContext _callback)
    {
        if (_player._dead || !_player._isGrounded || _player._canMove == false || _isAttacking || _player._onWater || Time.timeScale == 0f)
            return;

        if (_callback.started && !_player._canGrab)
        {
            _isRolling = true;
        }

        if (_callback.canceled)
        {
            _isRolling = false;
        }
    }

    public void OnHit()
    { //cancela os inputs quando toma dano
        isAttacking = false;
        isGrabing = false;
        isAirCuting = false;
        isTornado = false;
    }
}
