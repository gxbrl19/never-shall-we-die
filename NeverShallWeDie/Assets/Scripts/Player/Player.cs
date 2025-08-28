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

    //Skills
    [HideInInspector] public float timeForSkills;
    [HideInInspector] public float impulseForce = 35f;

    //Air Gem
    [HideInInspector] public float timeAirGem;
    [BoxGroup("GameObjects")] public Impulse _ImpulseEffect;
    [BoxGroup("Components")] public Transform _airGemPoint;
    [HideInInspector] public float airMana;


    //Water Gem
    [HideInInspector] public float timeWaterGem;
    private float waterSpinForce = 10f;
    [HideInInspector] public float waterMana;

    //Fire Gem
    [HideInInspector] public float timeFireGem;
    [BoxGroup("GameObjects")] public AirCut _fireGem;
    [BoxGroup("Components")] public Transform _fireGemPoint;
    [HideInInspector] public float fireMana;

    //States
    [HideInInspector] public bool canMove = true;
    [HideInInspector] public bool canGrab;
    [HideInInspector] public bool isGrounded;
    [HideInInspector] public bool isJumping;
    [HideInInspector] public bool isAttacking;
    [HideInInspector] public bool isRolling;
    [HideInInspector] public bool isWallSliding;
    [HideInInspector] public bool isHealing;
    [HideInInspector] public bool isGrabing;
    [HideInInspector] public bool isGriding;
    [HideInInspector] public bool canDash;
    [HideInInspector] public bool onSlope;
    [HideInInspector] public bool onBridge;
    [HideInInspector] public bool onLedge;
    [HideInInspector] public bool onWater;
    [HideInInspector] public bool onHit = false;
    [HideInInspector] public bool onClimbing;
    [HideInInspector] public bool onFireSpecial = false;
    [HideInInspector] public bool onWaterSpecial = false;
    [HideInInspector] public bool onAirSpecial = false;
    [HideInInspector] public bool newSkillCollected;
    [HideInInspector] public bool isDead = false;

    [HideInInspector] public Rigidbody2D rb;
    [HideInInspector] public PlayerMovement playerMovement;
    [HideInInspector] public CapsuleCollider2D playerCollider;
    [HideInInspector] public PlayerInputs playerInputs;
    [HideInInspector] public PlayerAnimations playerAnimations;
    [HideInInspector] public PlayerHealth playerHealth;
    [HideInInspector] public PlayerCollision playerCollision;
    [HideInInspector] public PlayerAudio playerAudio;

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

        waterMana = 4f;
        fireMana = 4f;
        airMana = 4;

        //adiciona as habilidades para usar na demo ( TODO: comentar essa parte quando for a versão final)
        if (!PlayerEquipment.instance.equipments.Contains(Equipments.Katana)) { PlayerEquipment.instance.equipments.Add(Equipments.Katana); }
        //if (!PlayerEquipment.instance.equipments.Contains(Equipments.Boots)) { PlayerEquipment.instance.equipments.Add(Equipments.Boots); }
        if (!PlayerEquipment.instance.equipments.Contains(Equipments.Parachute)) { PlayerEquipment.instance.equipments.Add(Equipments.Parachute); }
        //if (!PlayerEquipment.instance.equipments.Contains(Equipments.Lantern)) { PlayerEquipment.instance.equipments.Add(Equipments.Lantern); }
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
        timeFireGem = timeForSkills;
        timeWaterGem = timeForSkills;
        timeAirGem = timeForSkills;

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
        //contagem das skills
        timeFireGem += Time.deltaTime;
        timeWaterGem += Time.deltaTime;
        timeAirGem += Time.deltaTime;
    }

    void FixedUpdate()
    {
        OnParachute();

        //Special Attacks
        WaterSpin();
    }

    public void CancelMovesOnHit()
    {
        FinishAttack();
        playerMovement.FinishRoll();
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
        isAttacking = true;
        playerInputs.pressAttack = false;
        playerAnimations.AnimAttack();
        Invoke("FinishAttack", .3f); //garante o fim do ataque
    }
    public void FinishAttack() //chamado também na animação de ataque da katana
    {
        isAttacking = false;
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

    public void AirGem()
    {
        Vector3 _scale = _ImpulseEffect.transform.localScale;
        _scale.x = transform.localScale.x;
        _ImpulseEffect.transform.localScale = _scale;

        Instantiate(_ImpulseEffect.gameObject, _airGemPoint.position, _airGemPoint.rotation);
    }
    public void AirCut() //chamado na animação de AirCut
    {
        if (isDead || !canMove)
            return;

        _fireGem._direction = playerMovement.playerDirection;

        Vector3 _scale = _fireGem.transform.localScale;
        _scale.x = transform.localScale.x;
        _fireGem.transform.localScale = _scale;

        Instantiate(_fireGem.gameObject, _fireGemPoint.position, _fireGemPoint.rotation);
        playerHealth.ManaConsumption(fireMana);
    }

    void WaterSpin()
    {
        if (isDead || !canMove)
            return;

        if (playerInputs.pressLeftTrigger && onWater && PlayerSkills.instance.skills.Contains(Skills.WaterGem))
        {
            gameObject.layer = LayerMask.NameToLayer("WaterSpin");
            onWaterSpecial = true;

            if (playerMovement.playerDirection < 0)
                rb.velocity = Vector2.left * waterSpinForce;
            else if (playerMovement.playerDirection > 0)
                rb.velocity = Vector2.right * waterSpinForce;
        }
    }

    public void FinishWaterSpin() //chamado também na animação de Water Spin
    {
        onWaterSpecial = false;
        playerInputs.pressLeftTrigger = false;
        gameObject.layer = LayerMask.NameToLayer("Player");
    }
    #endregion

    public void CreateRecoveryEffect()
    {
        Instantiate(_recoveryEffect, transform.position, Quaternion.identity);
    }
}
