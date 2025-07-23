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
    [HideInInspector] public bool isSliding = false;
    bool hitSlide = false;
    float timeSlide = 0f;
    float limitSlide = 0.5f;
    float slideForce = 15f;

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

    //Tornado
    [HideInInspector] public bool inTornado;
    [HideInInspector] public float timeTornado;
    [BoxGroup("GameObjects")] public WindSpin _tornado;
    [BoxGroup("Components")] public Transform _tornadoPoint;
    private float tornadoMana;

    //States
    [HideInInspector] public bool canMove = true;
    [HideInInspector] public bool canGrab;
    [HideInInspector] public bool isGrounded;
    [HideInInspector] public bool isJumping;
    [HideInInspector] public bool isHealing;
    [HideInInspector] public bool isRolling;
    [HideInInspector] public bool isGrabing;
    [HideInInspector] public bool isGriding;
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
        tornadoMana = 4f;

        //adiciona as habilidades para usar na demo ( TODO: comentar essa parte quando for a versão final)
        if (!PlayerEquipment.instance.equipments.Contains(Equipments.Katana)) { PlayerEquipment.instance.equipments.Add(Equipments.Katana); }
        //if (!PlayerEquipment.instance.equipments.Contains(Equipments.Boots)) { PlayerEquipment.instance.equipments.Add(Equipments.Boots); }
        //if (!PlayerEquipment.instance.equipments.Contains(Equipments.Parachute)) { PlayerEquipment.instance.equipments.Add(Equipments.Parachute); }
        //if (!PlayerEquipment.instance.equipments.Contains(Equipments.Lantern)) { PlayerEquipment.instance.equipments.Add(Equipments.Lantern); }
        //if (!PlayerEquipment.instance.equipments.Contains(Equipments.Compass)) { PlayerEquipment.instance.equipments.Add(Equipments.Compass); }
        //if (!PlayerSkills.instance.skills.Contains(Skills.AirCut)) { PlayerSkills.instance.skills.Add(Skills.AirCut); }
        //if (!PlayerSkills.instance.skills.Contains(Skills.Tornado)) { PlayerSkills.instance.skills.Add(Skills.Tornado); }
        //if (!PlayerSkills.instance.skills.Contains(Skills.WaterSpin)) { PlayerSkills.instance.skills.Add(Skills.WaterSpin); }
    }

    void Start()
    {
        onWater = false;
        isDead = false;

        timeForSkills = 3f;
        timeAirCut = timeForSkills;
        timeTornado = timeForSkills;
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
        //contagem das skills
        timeAirCut += Time.deltaTime;
        timeTornado += Time.deltaTime;
        timeWaterSpin += Time.deltaTime;
    }

    void FixedUpdate()
    {
        CheckSlide();
        OnSlide();
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

    public void FinishAttack() //chamado na animação de ataque da katana
    {
        playerInputs.isAttacking = false;
        rb.gravityScale = playerMovement.initialGravity;
    }

    void OnParachute()
    {
        if (playerInputs.isParachuting)
        {
            rb.drag = speedParachute;
        }
        else
        {
            playerInputs.isParachuting = false;
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

    public void Tornado() //chamado na animação de Tornado
    {
        if (isDead || !canMove) { return; }

        Instantiate(_tornado.gameObject, _tornadoPoint.position, _tornadoPoint.rotation);
        playerHealth.ManaConsumption(tornadoMana);
    }

    void WaterSpin()
    {
        if (isDead || !canMove)
            return;

        if (playerInputs.isAttacking && onWater && PlayerSkills.instance.skills.Contains(Skills.WaterSpin))
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
        playerInputs.isAttacking = false;
        gameObject.layer = LayerMask.NameToLayer("Player");
    }
    #endregion

    #region Slide
    void OnSlide()
    {
        if (playerInputs.isSliding && !isSliding) { isSliding = true; }

        if (isSliding && ((timeSlide < limitSlide) || hitSlide)) //_hitSlide verifica se ainda tem GroundLayer em cima
        {

            DisableControls(); ;
            timeSlide += Time.deltaTime;
            if (playerMovement.playerDirection < 0) { rb.velocity = Vector2.left * slideForce; }
            else if (playerMovement.playerDirection > 0) { rb.velocity = Vector2.right * slideForce; }
        }
        else if (isSliding && timeSlide >= limitSlide)
        {
            if (!hitSlide) //se ainda estiver em baixo do GroundLayer continua o Slide
            {
                timeSlide = 0f;
                playerInputs.isSliding = false;
                isSliding = false;
                rb.velocity = Vector2.zero;
                EnabledControls();
            }
        }
    }

    public void CheckSlide()
    {
        hitSlide = false;
        Vector2 position = new Vector2(transform.position.x, transform.position.y - 1f);
        RaycastHit2D _slideHit = RaycastSlide(position, Vector2.up, 2f, _groundLayer);

        if (_slideHit)
        {
            hitSlide = true;
        }
    }

    #endregion

    public void CreateRecoveryEffect()
    {
        Instantiate(_recoveryEffect, transform.position, Quaternion.identity);
    }

    RaycastHit2D RaycastSlide(Vector2 offset, Vector2 rayDirection, float length, LayerMask layerMask)
    {
        Vector2 _position = new Vector2(offset.x, offset.y + 0.10f);
        RaycastHit2D _hit = Physics2D.Raycast(_position, rayDirection, length, layerMask);
        Color _color = _hit ? Color.green : Color.white;
        Debug.DrawRay(_position, rayDirection * length, _color);
        return _hit;
    }
}
