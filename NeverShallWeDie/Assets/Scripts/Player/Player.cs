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

    //Knife
    [BoxGroup("GameObjects")] public Knife _knife;
    [BoxGroup("Components")] public Transform _knifePoint;

    //Bomb
    [BoxGroup("GameObjects")] public Bomb _bomb;
    [BoxGroup("Components")] public Transform _bombPoint;

    //States
    [HideInInspector] public bool canMove = true;
    [HideInInspector] public bool canGrab;
    [HideInInspector] public bool isGrounded;
    [HideInInspector] public bool isJumping;
    [HideInInspector] public bool isAttacking;
    [HideInInspector] public bool isDashing;
    [HideInInspector] public bool isImpulsing;
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

        //adiciona as habilidades para usar na demo ( TODO: comentar essa parte quando for a versão final)
        if (!PlayerEquipment.instance.equipments.Contains(Equipments.Katana)) { PlayerEquipment.instance.equipments.Add(Equipments.Katana); }
        if (!PlayerEquipment.instance.equipments.Contains(Equipments.Lantern)) { PlayerEquipment.instance.equipments.Add(Equipments.Lantern); }
        if (!PlayerEquipment.instance.equipments.Contains(Equipments.Knife)) { PlayerEquipment.instance.equipments.Add(Equipments.Knife); }
        if (!PlayerEquipment.instance.equipments.Contains(Equipments.Bomb)) { PlayerEquipment.instance.equipments.Add(Equipments.Bomb); }
        if (!PlayerEquipment.instance.equipments.Contains(Equipments.Parachute)) { PlayerEquipment.instance.equipments.Add(Equipments.Parachute); }
        //if (!PlayerEquipment.instance.equipments.Contains(Equipments.Compass)) { PlayerEquipment.instance.equipments.Add(Equipments.Compass); }
        if (!PlayerSkills.instance.skills.Contains(Skills.Dash)) { PlayerSkills.instance.skills.Add(Skills.Dash); }
        if (!PlayerSkills.instance.skills.Contains(Skills.Slide)) { PlayerSkills.instance.skills.Add(Skills.Slide); }
        //if (!PlayerSkills.instance.skills.Contains(Skills.Impulse)) { PlayerSkills.instance.skills.Add(Skills.Impulse); }
    }

    void Start()
    {
        onWater = false;
        isDead = false;

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
        OnHealing();
    }

    void FixedUpdate()
    {
        OnParachute();
    }

    public void CancelMovesOnHit()
    {
        FinishAttack();
        playerMovement.FinishDash();
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

    #region Knife
    public void ShotKnife() //chamada na animação
    {
        if (isDead || !canMove)
            return;

        _knife.dir = playerMovement.playerDirection;
        Instantiate(_knife.gameObject, _knifePoint.position, _knifePoint.rotation);
    }
    #endregion

    #region Bomb
    public void ShotBomb() //chamada na animação
    {
        if (isDead || !canMove)
            return;

        _bomb.dir = playerMovement.playerDirection;
        Instantiate(_bomb.gameObject, _bombPoint.position, _bombPoint.rotation);
    }
    #endregion

    public void StartHealing()
    {
        isHealing = true;
        playerInputs.pressHealing = false;
        playerAnimations.AnimHealing();
    }

    public void OnHealing()
    {
        if (!isHealing) return;
        CinemachineShake.instance.ShakeCamera(3f, 0.15f);
    }

    public void FinishHealing() //chamado também na animação
    {
        isHealing = false;
        playerHealth.Healing();
    }

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

    public void CreateRecoveryEffect()
    {
        Instantiate(_recoveryEffect, transform.position, Quaternion.identity);
    }
}
