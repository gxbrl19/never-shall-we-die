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
    private bool _pressJump;
    private bool _jumpHeld;
    private bool _pressRoll;
    private bool _pressAttack;
    private bool _isFireCuting;
    private bool _pressDash;
    private bool _pressParachute;
    private bool _pressGrab;
    private bool _pressInteract;
    private float rawInput;
    private Player player;

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

    public bool pressJump
    {
        get { return _pressJump; }
        set { _pressJump = value; }
    }

    public bool jumpHeld
    {
        get { return _jumpHeld; }
        set { _jumpHeld = value; }
    }

    public bool pressAttack
    {
        get { return _pressAttack; }
        set { _pressAttack = value; }
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

    public bool pressDash
    {
        get { return _pressDash; }
        set { _pressDash = value; }
    }

    public bool pressParachute
    {
        get { return _pressParachute; }
        set { _pressParachute = value; }
    }

    public bool pressGrab
    {
        get { return _pressGrab; }
        set { _pressGrab = value; }
    }

    public bool pressInteract
    {
        get { return _pressInteract; }
        set { _pressInteract = value; }
    }
    #endregion

    void Start()
    {
        player = GetComponent<Player>();
    }

    public void Move(InputAction.CallbackContext callback)
    {
        rawInput = callback.ReadValue<float>(); //sempre atualiza o input
        _horizontal = callback.ReadValue<float>(); //sempre atualiza o input
    }

    public float GetHorizontal()
    {
        if (player.isDead || player.canMove == false || player.newSkillCollected || Time.timeScale == 0f)
            return 0f; //bloqueia o uso do input, mas não a leitura

        return rawInput;
    }

    public void ResetHorizontal()
    {
        rawInput = 0f;
    }

    public void UpAndDown(InputAction.CallbackContext callback)
    {
        if (player.isDead || Time.timeScale == 0f)
            return;

        _vertical = callback.ReadValue<float>();
    }

    public void R_Horizontal(InputAction.CallbackContext _callback)
    {
        if (player.isDead || Time.timeScale == 0f)
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
        if (player.isDead || Time.timeScale == 0f)
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

    public void ButtonSouth(InputAction.CallbackContext callback)
    {
        if (player.isDead || player.canMove == false || Time.timeScale == 0f) return;

        if (player.isGrounded || player.playerCollision._onWall || player.onClimbing || player.onWater || player.isGriding && !player.isDoubleJumping)
        {
            if (callback.started)
                _pressJump = true;

            if (!player.onWater)
                player.playerMovement.canDoubleJump = player.playerMovement.currentStamina > 0f && !player.playerMovement.isExhausted;

            _jumpHeld = callback.performed;
        }
        else if (!player.isGrounded && PlayerSkills.instance.skills.Contains(Skills.AirGem) && !player.isDoubleJumping && player.playerMovement.canDoubleJump)
        {
            if (callback.started && !player.playerCollision._onWall)
            {
                player.isDoubleJumping = true;
                player.playerMovement.canDoubleJump = false;
                player.playerMovement.StaminaConsumption(1.3f);
            }
        }
    }

    public void ButtonNorth(InputAction.CallbackContext _callback)
    {
        if (player.isDead || player.isHealing || _pressAttack || _isFireCuting || Time.timeScale == 0f || player.canMove == false)
            return;

        if (_callback.started && !player.canGrab)
        {
            _pressInteract = true;
            _pressGrab = false;
        }
        else if (_callback.started && player.canGrab)
        {
            _pressGrab = true;
            _pressInteract = false;
        }

        if (_callback.canceled)
        {
            _pressInteract = false;
            _pressGrab = false;
        }
    }

    public void ButtonWest(InputAction.CallbackContext callback)
    {
        if (player.isDead || player.isAttacking || _isFireCuting || Time.timeScale == 0f || player.onClimbing || player.playerCollision._onWall || player.onHit || player.isGrabing || player.isRolling || player.isDashing || !player.canMove)
            return;

        if (callback.started && PlayerEquipment.instance.equipments.Contains(Equipments.Katana) && !player.onWater)
        {
            _pressAttack = true;
        }
        else if (callback.started && player.onWater && PlayerEquipment.instance.equipments.Contains(Equipments.Katana) && PlayerSkills.instance.skills.Contains(Skills.WaterGem))
        {
            if (player.playerHealth.currentMana > 0 && player.timeWaterSpin >= player.timeForSkills)
            {
                _pressAttack = true;
                player.timeWaterSpin = 0f; //reseta o tempo do water spin para poder fazer a contagem;
                player.playerHealth.ManaConsumption(player.waterSpinMana);
            }
        }
    }

    public void ButtonEast(InputAction.CallbackContext _callback)
    {
        if (player.isDead || !player.isGrounded || player.canMove == false || player.isDashing || _pressAttack || player.onWater || player.canGrab || Time.timeScale == 0f || player.onHit)
            return;

        if (_callback.started)
            _pressRoll = true;

        if (_callback.canceled)
            _pressRoll = false;
    }

    public void LeftShoulder(InputAction.CallbackContext _callback)
    {
        if (player.isDead || !player.isGrounded || player.onHit || _pressAttack || _isFireCuting || Time.timeScale == 0f || player.onWater || player.canMove == false || player.playerCollision._onWall || player.isGrabing || player.isRolling || player.isDashing)
            return;

        if (_callback.started)
        {
            player.isHealing = true;
        }

        if (_callback.canceled)
        {
            player.isHealing = false;
        }
    }

    public void LeftTrigger(InputAction.CallbackContext callback)
    {
        if (player.isDead || _pressAttack || _isFireCuting || Time.timeScale == 0f || player.onClimbing || player.playerCollision._onWall || player.onWater || player.onHit || player.isGrabing || player.isRolling || player.isDashing || player.canMove == false)
            return;

        if (callback.started)
            _pressDash = true;

        if (callback.canceled)
            _pressDash = false;
    }

    public void RightShoulder(InputAction.CallbackContext callback)
    {
        if (player.isDead || player.isGrounded || player.playerCollision._onWall || player.onClimbing || player.onWater || player.isGriding) return;

        if (callback.started && PlayerEquipment.instance.equipments.Contains(Equipments.Parachute) && player.playerMovement.currentStamina > 0f && !player.playerMovement.isExhausted)
        {
            _pressParachute = true;
            player.playerAudio.PlayParachute();
        }
        else if (callback.canceled)
        {
            _pressParachute = false;
        }
    }

    public void RightTrigger(InputAction.CallbackContext _callback)
    {
        if (player.isDead || _pressAttack || _isFireCuting || Time.timeScale == 0f || player.onClimbing || player.playerCollision._onWall || player.onHit || player.isGrabing || player.onWater || player.isRolling || player.canMove == false)
            return;

        if (PlayerEquipment.instance.equipments.Contains(Equipments.Katana) && PlayerSkills.instance.skills.Contains(Skills.FireGem))
        {
            if (player.playerHealth.currentMana > 0 && player.timeAirCut >= player.timeForSkills)
            {
                isFireCuting = true;
                player.timeAirCut = 0f; //reseta o tempo do aircut para poder fazer a contagem;
                player.playerAnimations.OnAirCut();
                //.PlayAudio("aircut");
            }
        }
    }

    public void CancelInputs()
    {
        pressAttack = false;
        pressGrab = false;
        isFireCuting = false;
        pressDash = false;
        pressParachute = false;
        player.isHealing = false;

        horizontal = 0f;
        vertical = 0f;
    }
}
