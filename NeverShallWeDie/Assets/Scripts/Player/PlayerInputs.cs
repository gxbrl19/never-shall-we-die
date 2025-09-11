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
    private bool _pressDash;
    private bool _pressAttack;
    private bool _pressRightTrigger;
    private bool _pressLeftTrigger;
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

    public bool pressRightTrigger
    {
        get { return _pressRightTrigger; }
        set { _pressRightTrigger = value; }
    }

    public bool pressDash
    {
        get { return _pressDash; }
        set { _pressDash = value; }
    }

    public bool pressLeftTrigger
    {
        get { return _pressLeftTrigger; }
        set { _pressLeftTrigger = value; }
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

        if (player.isGrounded || player.onLedge || player.isWallSliding || player.onClimbing || player.onWater || player.isGriding)
        {
            if (callback.started)
                _pressJump = true;

            _jumpHeld = callback.performed;
        }
    }

    public void ButtonNorth(InputAction.CallbackContext _callback)
    {
        if (player.isDead || player.isHealing || player.isAttacking || _pressRightTrigger || Time.timeScale == 0f || player.canMove == false)
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
        if (player.isDead || player.isAttacking || _pressRightTrigger || Time.timeScale == 0f || player.onClimbing || player.onLedge
        || player.onHit || player.isGrabing || player.isGriding || player.isDashing || player.onAirSpecial || !player.canMove
        || !PlayerEquipment.instance.equipments.Contains(Equipments.Katana))
            return;

        if (callback.started && !player.onWater)
        {
            _pressAttack = true;
            player.OnKatana();
        }
    }

    public void ButtonEast(InputAction.CallbackContext callback)
    {
        if (player.isDead || player.isDashing || player.canMove == false || player.onAirSpecial || player.isAttacking || player.onWater || player.canGrab || Time.timeScale == 0f || player.onHit)
            return;

        if (callback.started)
            _pressDash = true;

        if (callback.canceled)
            _pressDash = false;
    }

    public void LeftShoulder(InputAction.CallbackContext callback)
    {
        if (player.isDead || !player.isGrounded || player.onHit || player.isAttacking || _pressRightTrigger || Time.timeScale == 0f || player.onWater || player.canMove == false || player.onLedge || player.isGrabing || player.isDashing || player.onAirSpecial)
            return;

        if (callback.started)
            player.isHealing = true;

        if (callback.canceled)
            player.isHealing = false;
    }

    public void LeftTrigger(InputAction.CallbackContext callback)
    {
        if (player.isDead || player.isAttacking || player.isHealing || Time.timeScale == 0f || player.onClimbing || player.onLedge || player.isWallSliding || player.onHit || player.isGrabing || player.isDashing || player.onWaterSpecial || player.onAirSpecial || player.canMove == false)
            return;

        else if (callback.started && PlayerSkills.instance.skills.Contains(Skills.WaterGem) && player.onWater)
        {
            if (player.playerHealth.currentMana > 0 && player.timeWaterGem >= player.timeForSkills)
            {
                _pressLeftTrigger = true;
                player.timeWaterGem = 0f; //reseta o tempo
                player.playerHealth.ManaConsumption(player.waterMana);
            }
        }
        else if (callback.started && PlayerSkills.instance.skills.Contains(Skills.AirGem) && !player.isGrounded && !player.onWater)
        {
            if (player.playerHealth.currentMana > 0 && player.timeAirGem >= player.timeForSkills)
            {
                _pressLeftTrigger = true;
                player.onAirSpecial = true;
                player.timeAirGem = 0f; //reseta o tempo
                player.playerHealth.ManaConsumption(player.airMana);
            }
        }
    }

    public void RightShoulder(InputAction.CallbackContext callback)
    {
        if (player.isDead || player.isGrounded || player.onLedge || player.onClimbing || player.onWater || player.isGriding) return;

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
        if (player.isDead || player.isAttacking || _pressRightTrigger || Time.timeScale == 0f || player.onClimbing || player.onLedge || player.onHit || player.isGrabing || player.onWater || player.isDashing || player.canMove == false)
            return;

        if (PlayerEquipment.instance.equipments.Contains(Equipments.Katana) && PlayerSkills.instance.skills.Contains(Skills.FireGem))
        {
            if (player.playerHealth.currentMana > 0 && player.timeFireGem >= player.timeForSkills)
            {
                pressRightTrigger = true;
                player.timeFireGem = 0f; //reseta o tempo do aircut para poder fazer a contagem;
                player.playerAnimations.OnAirCut();
                //.PlayAudio("aircut");
            }
        }
    }

    public void CancelInputs()
    {
        pressAttack = false;
        pressDash = false;
        pressGrab = false;
        pressLeftTrigger = false;
        pressParachute = false;
        pressRightTrigger = false;
        player.isHealing = false;

        horizontal = 0f;
        vertical = 0f;
    }
}
