using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.Windows;

public class Player : MonoBehaviour
{
    [BoxGroup("Components")] public LayerMask _groundLayer;
    [BoxGroup("Components")] public LayerMask _slopeLayer;
    [BoxGroup("Components")] public LayerMask _ladderLayer;
    [BoxGroup("Components")] public LayerMask _vineLayer;
    [BoxGroup("Components")] public LayerMask _bridgeLayer;
    [BoxGroup("Components")] public LayerMask _gridLayer;
    [BoxGroup("Components")] public Transform _groundCheck;
    [BoxGroup("Components")] public SpriteRenderer _newSkill;
    [BoxGroup("Components")] public PhysicsMaterial2D _noFrictionMaterial;
    [BoxGroup("Components")] public PhysicsMaterial2D _frictionMaterial;
    [BoxGroup("Components")] public PlayerPosition _scriptablePosition;
    [BoxGroup("GameObjects")] public GameObject _powerPickup;
    [Header("Particles")][BoxGroup("GameObjects")] public GameObject _dust;
    [BoxGroup("GameObjects")] public GameObject _recoveryEffect;

    //Parachute
    private float normalFallSpeed = 0f;
    private float speedParachute = 20f;

    //Slide
    [HideInInspector] public bool isDashing = false;
    float dashTimer = 0f;
    float dashCooldown = .5f;
    float dashForce = 32f;

    //Skills
    [HideInInspector] public float timeForSkills;

    //Water Spin
    [HideInInspector] public float timeWaterSpin;
    private float waterSpinForce = 10f;
    [HideInInspector] public bool inWaterSpin;
    [HideInInspector] public float waterSpinMana;

    //Air Cut
    [HideInInspector] public float timeAirCut;
    [BoxGroup("GameObjects")] public AirCut _aircut;
    [BoxGroup("Components")] public Transform _aircutPoint;
    [HideInInspector] public float aircutMana;

    //States
    [HideInInspector] public bool canMove = true;
    [HideInInspector] public bool canGrab;
    [HideInInspector] public bool isGrounded;
    [HideInInspector] public bool isJumping;
    [HideInInspector] public bool isAttacking;
    [HideInInspector] public bool isRolling;
    [HideInInspector] public bool isDoubleJumping;
    [HideInInspector] public bool isHealing;
    [HideInInspector] public bool isGrabing;
    [HideInInspector] public bool isGriding;
    [HideInInspector] public bool canDash;
    [HideInInspector] public bool isDead = false;
    [HideInInspector] public bool onSlope;
    [HideInInspector] public bool onBridge;
    [HideInInspector] public bool onWater;
    [HideInInspector] public bool onHit = false;
    [HideInInspector] public bool onClimbing;
    [HideInInspector] public bool newSkillCollected;

    [HideInInspector] public Rigidbody2D rb;
    [HideInInspector] public PlayerMovement playerMovement;
    [HideInInspector] public CapsuleCollider2D playerCollider;
    [HideInInspector] public PlayerInputs playerInputs;
    [HideInInspector] public PlayerAnimations playerAnimations;
    [HideInInspector] public PlayerHealth playerHealth;
    [HideInInspector] public PlayerCollision playerCollision;
    [HideInInspector] public PlayerAudio playerAudio;
    [HideInInspector] public Bridge bridge;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        playerMovement = GetComponent<PlayerMovement>();
        playerCollider = GetComponent<CapsuleCollider2D>();
        playerInputs = GetComponent<PlayerInputs>();
        playerAnimations = GetComponent<PlayerAnimations>();
        playerCollision = GetComponent<PlayerCollision>();
        playerAudio = GetComponent<PlayerAudio>();
        playerHealth = GetComponent<PlayerHealth>();

        waterSpinMana = 4f;
        aircutMana = 4f;

        //adiciona as habilidades para usar na demo ( TODO: comentar essa parte quando for a versão final)
        if (!PlayerEquipment.instance.equipments.Contains(Equipments.Katana)) { PlayerEquipment.instance.equipments.Add(Equipments.Katana); }
        //if (!PlayerEquipment.instance.equipments.Contains(Equipments.Boots)) { PlayerEquipment.instance.equipments.Add(Equipments.Boots); }
        //if (!PlayerEquipment.instance.equipments.Contains(Equipments.Parachute)) { PlayerEquipment.instance.equipments.Add(Equipments.Parachute); }
        if (!PlayerEquipment.instance.equipments.Contains(Equipments.Lantern)) { PlayerEquipment.instance.equipments.Add(Equipments.Lantern); }
        //if (!PlayerEquipment.instance.equipments.Contains(Equipments.Compass)) { PlayerEquipment.instance.equipments.Add(Equipments.Compass); }
        //if (!PlayerSkills.instance.skills.Contains(Skills.FireGem)) { PlayerSkills.instance.skills.Add(Skills.FireGem); }
        //if (!PlayerSkills.instance.skills.Contains(Skills.AirGem)) { PlayerSkills.instance.skills.Add(Skills.AirGem); }
        //if (!PlayerSkills.instance.skills.Contains(Skills.WaterGem)) { PlayerSkills.instance.skills.Add(Skills.WaterGem); }
    }

    void Start()
    {
        onWater = false;
        isDead = false;

        timeForSkills = 3f;
        timeAirCut = timeForSkills;
        timeWaterSpin = timeForSkills;

        if (_scriptablePosition.SceneTransition)
        {
            int _index = _scriptablePosition.Index;
            StartPosition _startPosition = FindFirstObjectByType<StartPosition>();
            Vector3 _position = new Vector3(_startPosition._startPositions[_index].position.x, _startPosition._startPositions[_index].position.y, _startPosition._startPositions[_index].position.z);

            transform.position = _position;
            if (_scriptablePosition.Direction == -1) { playerMovement.Flip(); }
        }
        else
        {
            playerHealth.ResetHealth();
        }
    }

    private void Update()
    {
        OnKatana();

        dashTimer += Time.deltaTime;
        //contagem das skills
        timeAirCut += Time.deltaTime;
        timeWaterSpin += Time.deltaTime;
    }

    void FixedUpdate()
    {
        OnDash();
        OnParachute();

        //Special Attacks
        WaterSpin();
    }

    #region Movement

    public void DisableControls()
    {
        rb.velocity = Vector2.zero;
        playerInputs.ResetHorizontal();
        playerInputs.vertical = 0f;
        canMove = false;
    }

    public void EnabledControls()
    {
        canMove = true;
    }

    #region Katana
    public void OnKatana()
    {
        if (playerInputs.pressAttack)
        {
            isAttacking = true;
            playerInputs.pressAttack = false;
            playerAnimations.AnimAttack();
            Invoke("FinishAttack", .2f);
        }
    }
    public void FinishAttack() //chamado na animação de ataque da katana
    {
        isAttacking = false;
        rb.gravityScale = playerMovement.initialGravity;
    }
    #endregion

    void OnParachute()
    {
        if (playerInputs.pressParachute && playerMovement.currentStamina > 0f)
        {
            rb.drag = speedParachute;
            playerMovement.StaminaConsumption(.04f);
        }
        else
        {
            playerInputs.pressParachute = false;
            rb.drag = normalFallSpeed;
        }
    }

    public void EnterInWater()
    {
        Invoke("InTheWater", 0.03f);
    }

    void InTheWater() //cancela a velocidade de queda ao entrar na agua
    {
        rb.velocity = new Vector2(rb.velocity.x, 0f);
    }

    public void OnDead()
    {
        isDead = true;
        playerInputs.ResetHorizontal();
        DisableControls();
        playerAnimations.OnDead();
        SceneController.instance.GameOver();
    }

    public void SetPowerPickup(Sprite sprite)
    {
        newSkillCollected = true;
        playerInputs.ResetHorizontal();
        DisableControls();
        playerAnimations.SetPowerPickup();

        if (sprite != null)
        {
            _newSkill.sprite = sprite;
            _powerPickup.SetActive(true);
        }
    }

    void EndPowerPickup() //chamado na animação
    {
        newSkillCollected = false;
        EnabledControls();
        _powerPickup.SetActive(false);
    }

    #endregion

    #region Skills
    public void AirCut() //chamado na animação de AirCut
    {
        if (isDead || !canMove)
            return;

        _aircut._direction = playerMovement.playerDirection;

        Vector3 _scale = _aircut.transform.localScale;
        _scale.x = transform.localScale.x;
        _aircut.transform.localScale = _scale;

        Instantiate(_aircut.gameObject, _aircutPoint.position, _aircutPoint.rotation);
        playerHealth.ManaConsumption(aircutMana);
    }

    void WaterSpin()
    {
        if (isDead || !canMove)
            return;

        if (playerInputs.pressAttack && onWater && PlayerSkills.instance.skills.Contains(Skills.WaterGem))
        {
            gameObject.layer = LayerMask.NameToLayer("WaterSpin");
            inWaterSpin = true;

            if (playerMovement.playerDirection < 0)
            {
                rb.velocity = Vector2.left * waterSpinForce;
            }
            else if (playerMovement.playerDirection > 0)
            {
                rb.velocity = Vector2.right * waterSpinForce;
            }
        }
    }

    public void FinishWaterSpin() //chamado também na animação de Water Spin
    {
        inWaterSpin = false;
        playerInputs.pressAttack = false;
        gameObject.layer = LayerMask.NameToLayer("Player");
    }
    #endregion

    #region Dash
    void OnDash()
    {
        canDash = playerMovement.currentStamina > 0f && PlayerEquipment.instance.equipments.Contains(Equipments.Boots) && !playerMovement.isExhausted && dashTimer >= dashCooldown;

        if (playerInputs.pressDash && canDash && !isDashing)
        {
            canMove = false;
            isDashing = true;
            playerInputs.pressDash = false;
            playerMovement.StaminaConsumption(1.3f);
            dashTimer = 0f;
            rb.velocity = Vector2.zero;
        }

        if (isDashing)
            ExecuteDash();
    }

    void ExecuteDash()
    {
        rb.gravityScale = 0f;

        Vector2 dir = playerMovement.playerDirection < 0 ? Vector2.left : Vector2.right;
        rb.AddForce(dir * dashForce, ForceMode2D.Impulse);

        Invoke("FinishDash", .2f);
    }

    public void FinishDash() //chamado no Invoke dentro de ExecuteDash
    {
        canMove = true;
        rb.gravityScale = 8f;
        isDashing = false;
    }
    #endregion

    public void CreateRecoveryEffect()
    {
        Instantiate(_recoveryEffect, transform.position, Quaternion.identity);
    }
}
