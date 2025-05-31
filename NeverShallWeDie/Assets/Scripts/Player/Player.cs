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

    //Config
    private float _normalSpeed = 8f;
    private float _initialGravity;
    [HideInInspector] public int _direction = 1;
    [HideInInspector] public float _speed;

    //Check Ground
    private float _footOffset = 0.28f;
    private float _groundOffset = -0.2f;
    private float _groundDistance = 0.3f;

    //Jump
    private bool _isJumping;
    private float _jumpForce = 16f;

    //Jump Hold
    private float _jumpHoldForce = 1.7f;
    private float _jumpHoldDuration = 0.17f;
    private float _jumpTime;

    //Ghost Jump
    private float _ghostDuration = 0.15f;
    private float _ghostTime;

    //Roll
    private bool _canRoll = true;
    private float _rollForce = 10f;

    //Parachute
    private float _normalFallSpeed = 0f;
    private float _speedParachute = 20f;

    //Climb
    private float _climbSpeed = 3.5f;
    private float _checkRadius = 0.5f;
    private Vector3 _checkPositionUp = new Vector3(0f, 0.6f, 0f);
    private Vector3 _checkPositionDown = new Vector3(0f, -1.2f, 0f);
    [HideInInspector] public Transform _ladder;
    [HideInInspector] public Transform _vine;
    [HideInInspector] public bool _onClimbing;

    //Slide
    [HideInInspector] public bool _isSliding = false;
    bool _hitSlide = false;
    float _timeSlide = 0f;
    float _limitSlide = 0.5f;
    float _slideForce = 15f;

    //Grab
    private float _grabSpeed = 2f;

    //Hit
    [HideInInspector] public bool _knockback;
    private float _knockbackForce = 5f;
    [HideInInspector] public bool _onHit = false;

    //Slope
    private float _slopeCheckDistance = 1f;
    private float _slopeDownAngle;
    private float _slopeDownAngleOld;
    private float _slopeSideAngle;
    private Vector2 _slopeNormalPerp;

    //Bridge
    private float _bridgeCheckDistance = -1.09f;

    //Water
    private float _waterGravity = 0.4f;
    private float _waterSpeed = 4f;
    private float _swimForce = 3f;
    private float _jumpOutWater = 10f;
    private float _swinLimit = 0.2f;
    [HideInInspector] public bool _canSwin = true;
    [HideInInspector] public bool _onWater;

    //Skills
    [HideInInspector] public float _timeForSkills;

    //Water Spin
    [HideInInspector] public float _timeWaterSpin;
    private float _waterSpinForce = 10f;
    [HideInInspector] public bool _inWaterSpin;
    [HideInInspector] public float _waterSpinMana;

    //Air Cut
    [HideInInspector] public float _timeAirCut;
    [BoxGroup("GameObjects")] public AirCut _aircut;
    [BoxGroup("Components")] public Transform _aircutPoint;
    [HideInInspector] public float _aircutMana;

    //Tornado
    [HideInInspector] public bool _inTornado;
    [HideInInspector] public float _timeTornado;
    [BoxGroup("GameObjects")] public WindSpin _tornado;
    [BoxGroup("Components")] public Transform _tornadoPoint;
    private float _tornadoMana;

    //Particles
    [SerializeField][Header("Particles")][BoxGroup("GameObjects")] private GameObject _dust;
    [SerializeField][BoxGroup("GameObjects")] private GameObject _recoveryEffect;

    [HideInInspector] public bool _isGrounded;
    [HideInInspector] public bool _healing;
    [HideInInspector] public bool _isOnSlope;
    [HideInInspector] public bool _canGrab;
    [HideInInspector] public bool _isGrabing;
    [HideInInspector] public bool _isRolling;
    [HideInInspector] public bool _onBridge;
    [HideInInspector] public bool _bridgeHit;
    public bool _isGriding;
    [HideInInspector] public bool _canMove = true;
    [HideInInspector] public bool _newSkillCollected;
    [HideInInspector] public bool _dead = false;
    [HideInInspector] public Rigidbody2D _body;

    Vector2 _colliderSize;
    CapsuleCollider2D _collider;
    PlayerInputs _input;
    PlayerAnimations _animation;
    PlayerHealth _health;
    PlayerCollision _collision;
    PlayerAudio _audio;
    Bridge _bridge;

    private void Awake()
    {
        _body = GetComponent<Rigidbody2D>();
        _collider = GetComponent<CapsuleCollider2D>();
        _input = GetComponent<PlayerInputs>();
        _animation = GetComponent<PlayerAnimations>();
        _collision = GetComponent<PlayerCollision>();
        _audio = GetComponent<PlayerAudio>();
        _health = GetComponent<PlayerHealth>();

        _waterSpinMana = 4f;
        _aircutMana = 4f;
        _tornadoMana = 4f;

        //adiciona as habilidades para usar na demo ( TODO: comentar essa parte quando for a versão final)
        //if (!PlayerEquipment.instance.equipments.Contains(Equipments.Katana)) { PlayerEquipment.instance.equipments.Add(Equipments.Katana); }
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
        _colliderSize = _collider.size;
        _speed = _normalSpeed;
        _initialGravity = _body.gravityScale;
        _onWater = false;
        _dead = false;

        _timeForSkills = 3f;
        _timeAirCut = _timeForSkills;
        _timeTornado = _timeForSkills;
        _timeWaterSpin = _timeForSkills;

        if (_scriptablePosition.SceneTransition)
        {
            int _index = _scriptablePosition.Index;
            StartPosition _startPosition = FindFirstObjectByType<StartPosition>();
            Vector3 _position = new Vector3(_startPosition._startPositions[_index].position.x, _startPosition._startPositions[_index].position.y, _startPosition._startPositions[_index].position.z);

            transform.position = _position;
            if (_scriptablePosition.Direction == -1) { Flip(); }
        }
        else
        {
            _health.ResetHealth();
        }
    }

    private void Update()
    {
        //contagem das skills
        _timeAirCut += Time.deltaTime;
        _timeTornado += Time.deltaTime;
        _timeWaterSpin += Time.deltaTime;
    }

    void FixedUpdate()
    {
        PhysicsCheck();
        JumpControl();
        CheckMove();
        BlockMove();
        CheckSlope();
        CheckBridge();
        CheckSlide();
        OnSlide();
        OnWater();
        OnClimb();
        OnGrid();
        OnRoll();
        OnParachute();
        OnHit();

        //Special Attacks
        WaterSpin();
    }

    #region Movement

    public void DisableControls()
    {
        _body.velocity = Vector2.zero;
        _input.horizontal = 0f;
        _input.vertical = 0f;
        _canMove = false;
    }

    public void EnabledControls()
    {
        _canMove = true;
    }

    void PhysicsCheck()
    {
        if (!_dead)
        {
            _isGrounded = false;

            //dispara um raio para baixo de cada pé para checagem do chão
            RaycastHit2D _leftFoot = Raycast(new Vector2(-_footOffset, -_groundOffset), Vector2.down, _groundDistance, _groundLayer);
            RaycastHit2D _rightFoot = Raycast(new Vector2(_footOffset, -_groundOffset), Vector2.down, _groundDistance, _groundLayer);

            if ((_leftFoot || _rightFoot) && !_bridgeHit && !_onClimbing)
            {
                _isGrounded = true;
                _input.isParachuting = false;
            }

            if (_body.velocity.y <= 0.0f)
            {
                _isJumping = false;
            }
        }
        else
        {
            _body.velocity = Vector2.zero;
        }

        //gravidade na água
        if (_onWater)
        {
            _body.gravityScale = _waterGravity;
        }
        else
        {
            _body.gravityScale = _initialGravity;
        }
    }

    void CheckMove()
    {
        float _xVelocity = 0f;
        float _yVelocity = 0f;

        if (_isGrounded && !_isOnSlope && !_isGrabing && !_isJumping && !_onWater && !_input.isAttacking && !_healing) //chão comum
        {
            _xVelocity = _speed * _input.horizontal;
            _yVelocity = 0.0f;
            _body.velocity = new Vector2(_xVelocity, _yVelocity);
        }
        else if (_isGrounded && _isOnSlope && !_isGrabing && !_isJumping && !_onWater && !_healing & !_input.isAttacking) //diagonal
        {
            _xVelocity = _speed * _slopeNormalPerp.x * -_input.horizontal;
            _yVelocity = _speed * _slopeNormalPerp.y * -_input.horizontal;
            _body.velocity = new Vector2(_xVelocity, _yVelocity);
        }
        else if (_isGrounded && _isGrabing && !_isOnSlope && !_isJumping) //empurrando caixa
        {
            _xVelocity = _grabSpeed * _input.horizontal;
            _yVelocity = _body.velocity.y;
            _body.velocity = new Vector2(_xVelocity, _yVelocity);
        }
        else if (!_isGrounded && !_onWater) //no ar
        {
            _xVelocity = _speed * _input.horizontal;
            _yVelocity = _body.velocity.y;
            _body.velocity = new Vector2(_xVelocity, _yVelocity);
        }
        else if (_onWater) //na água
        {
            _xVelocity = _waterSpeed * _input.horizontal;
            _yVelocity = _body.velocity.y;
            _body.velocity = new Vector2(_xVelocity, _yVelocity);
        }

        if (_direction * _xVelocity < 0)
        {
            Flip();
        }

        if (_isGrounded)
        {
            _ghostTime = Time.time + _ghostDuration;
        }

        _animation.xVelocity = Mathf.Abs(_xVelocity);
        _animation.yVelocity = Mathf.Abs(_yVelocity);
    }

    void CheckSlope()
    {
        Vector2 _checkPos = transform.position - new Vector3(0.0f, _colliderSize.y / 2);

        SlopeCheckHorizontal(_checkPos);
        SlopeCheckVertical(_checkPos);
    }

    void SlopeCheckHorizontal(Vector2 checkPos)
    {
        RaycastHit2D _slopeHitFront = Physics2D.Raycast(checkPos, transform.right, _slopeCheckDistance, _slopeLayer);
        RaycastHit2D _slopeHitBack = Physics2D.Raycast(checkPos, -transform.right, _slopeCheckDistance, _slopeLayer);

        if (_slopeHitFront)
        {
            _isOnSlope = true;
            _slopeSideAngle = Vector2.Angle(_slopeHitFront.normal, Vector2.up);
        }
        else if (_slopeHitBack)
        {
            _isOnSlope = true;
            _slopeSideAngle = Vector2.Angle(_slopeHitBack.normal, Vector2.up);
        }
        else
        {
            _slopeSideAngle = 0.0f;
            _isOnSlope = false;
        }
    }

    void SlopeCheckVertical(Vector2 checkPos)
    {
        RaycastHit2D _hit = Physics2D.Raycast(checkPos, Vector2.down, _slopeCheckDistance, _groundLayer);

        if (_hit)
        {
            _slopeNormalPerp = Vector2.Perpendicular(_hit.normal).normalized;
            _slopeDownAngle = Vector2.Angle(_hit.normal, Vector2.up);

            if (_slopeDownAngle != _slopeDownAngleOld)
            {
                _isOnSlope = true;
            }

            _slopeDownAngleOld = _slopeDownAngle;

            Debug.DrawRay(_hit.point, _slopeNormalPerp, Color.green);
            Debug.DrawRay(_hit.point, _hit.normal, Color.yellow);
        }

        if (_isOnSlope && (_input.horizontal == 0.0f || _isOnSlope && _input.isAttacking || _healing))
        {
            _body.sharedMaterial = _frictionMaterial;
        }
        else
        {
            _body.sharedMaterial = _noFrictionMaterial;
        }
    }

    void JumpControl()
    {
        if (_input.isJumping && (_isGrounded || _ghostTime > Time.time) && !_onWater && _input.vertical > -0.3f && !_onClimbing && !_inTornado && !_healing) //pulo comum
        {
            _isJumping = true;
            _input.isJumping = false;

            _body.AddForce(Vector2.up * _jumpForce, ForceMode2D.Impulse);

            _jumpTime = Time.time + _jumpHoldDuration;

            _ghostTime = Time.time;
            _audio.PlayJump();
            CreateDust(1);
        }
        else if (_input.isJumping && _onWater && _canSwin && !_collision._onWall) // na água
        {
            if (_collision._outWaterHit && _collision._inWaterHit)
            {
                _isJumping = true;
                _input.isJumping = false;
                _body.velocity = Vector2.zero;
                _body.AddForce(Vector2.up * _jumpOutWater, ForceMode2D.Impulse);
                _jumpTime = Time.time + _jumpHoldDuration;
                _ghostTime = Time.time;
                _audio.PlayJump();
            }
            else if (!_collision._outWaterHit && _collision._inWaterHit)
            {
                _body.velocity = Vector2.zero;
                _body.AddForce(Vector2.up * _swimForce, ForceMode2D.Impulse);
                _canSwin = false;
            }
        }
        else if (_input.isJumping && _input.vertical <= -0.3f && !_onClimbing) // pulo por baixo da plataforma
        {
            PassThroughBridge();
        }
        else if (_input.isJumping && _collision._onWall && !_isGrounded && !_onWater) // pulo parede
        {
            _isJumping = true;
            _input.isJumping = false;

            _body.AddForce(new Vector2((_jumpForce + 2f) * -_direction, _jumpForce + 7f), ForceMode2D.Impulse);
        }
        else if (_input.isJumping && (_onClimbing || _isGriding) && (TouchingVine() || TouchingLadder() || TouchingGrid())) //pulo da LADDER ou VINE
        {
            FinishClimb();
            FinishGrid();
            _isJumping = true;
            _input.vertical = 0f;
            _body.velocity = Vector2.zero;
            _body.AddForce(Vector2.up * _jumpForce, ForceMode2D.Impulse);
            _jumpTime = Time.time + _jumpHoldDuration;
            _audio.PlayJump();
        }

        if (_isJumping)
        {
            if (_input.jumpHeld)
            {
                _body.AddForce(Vector2.up * _jumpHoldForce, ForceMode2D.Impulse);
            }

            if (_jumpTime <= Time.time)
            {
                _isJumping = false;
            }
        }

        _input.isJumping = false;
    }

    void BlockMove() //verifica se está no ar e tira a gravidade do player
    {
        if (_dead || !_canMove)
            return;

        if ((_input.isAttacking && _isGrounded && !_onWater) || _healing) { _body.velocity = Vector2.zero; }

        //para no ar
        if (_input.isAirCuting || _input.isTornado)
        {
            if (!_isGrounded)
            {
                _body.velocity = Vector2.zero;
                _body.gravityScale = 0f;
            }
            else
            {
                _body.velocity = Vector2.zero;
            }
        }
    }

    public void FinishAttack() //chamado na animação de ataque da katana
    {
        _input.isAttacking = false;
        _body.gravityScale = _initialGravity;
    }

    void OnParachute()
    {
        if (_input.isParachuting)
        {
            _body.drag = _speedParachute;
        }
        else
        {
            _input.isParachuting = false;
            _body.drag = _normalFallSpeed;
        }
    }

    void OnWater()
    {
        if (_dead || !_onWater)
            return;

        if (!_canSwin)
        {
            StartCoroutine(SwimControl());
        }
    }

    public void EnterInWater()
    {
        Invoke("InTheWater", 0.03f);
    }

    void InTheWater() //cancela a velocidade de queda ao entrar na agua
    {
        _body.velocity = new Vector2(_body.velocity.x, 0f);
    }

    public void OnDead()
    {
        _dead = true;
        _input.horizontal = 0;
        DisableControls();
        _animation.OnDead();
        SceneController.instance.GameOver();
    }

    public void SetPowerPickup(Sprite sprite)
    {
        _newSkillCollected = true;
        _input.horizontal = 0f;
        DisableControls();
        _animation.SetPowerPickup();

        if (sprite != null)
        {
            _newSkill.sprite = sprite;
            _powerPickup.SetActive(true);
        }
    }

    void EndPowerPickup() //chamado na animação
    {
        _newSkillCollected = false;
        EnabledControls();
        _powerPickup.SetActive(false);
    }

    void Flip()
    {
        if (_dead || !_canMove || _isGrabing || _input.isAirCuting || _inWaterSpin || _input.isAttacking || _isRolling)
            return;

        if (_isGrounded)
        {
            CreateDust(-1);
        }

        _direction *= -1;

        Vector3 _scale = transform.localScale;
        _scale.x *= -1;
        transform.localScale = _scale;
    }

    #endregion

    #region Skills
    public void AirCut() //chamado na animação de AirCut
    {
        if (_dead || !_canMove)
            return;

        _aircut._direction = _direction;

        Vector3 _scale = _aircut.transform.localScale;
        _scale.x = transform.localScale.x;
        _aircut.transform.localScale = _scale;

        Instantiate(_aircut.gameObject, _aircutPoint.position, _aircutPoint.rotation);
        _health.ManaConsumption(_aircutMana);
    }

    public void Tornado() //chamado na animação de Tornado
    {
        if (_dead || !_canMove) { return; }

        Instantiate(_tornado.gameObject, _tornadoPoint.position, _tornadoPoint.rotation);
        _health.ManaConsumption(_tornadoMana);
    }

    void WaterSpin()
    {
        if (_dead || !_canMove)
            return;

        if (_input.isAttacking && _onWater && PlayerSkills.instance.skills.Contains(Skills.WaterSpin))
        {
            gameObject.layer = LayerMask.NameToLayer("WaterSpin");
            _inWaterSpin = true;

            if (_direction < 0)
            {
                _body.velocity = Vector2.left * _waterSpinForce;
            }
            else if (_direction > 0)
            {
                _body.velocity = Vector2.right * _waterSpinForce;
            }
        }
    }

    public void FinishWaterSpin() //chamado também na animação de Water Spin
    {
        _inWaterSpin = false;
        _input.isAttacking = false;
        gameObject.layer = LayerMask.NameToLayer("Player");
    }
    #endregion

    #region Roll
    void OnRoll()
    {
        if ((_input.isRolling && _canRoll) || _isRolling)
        {
            gameObject.layer = LayerMask.NameToLayer("Invencible");

            _canRoll = false;
            _isRolling = true;

            if (_direction < 0)
            {
                _body.velocity = Vector2.left * _rollForce;
            }
            else if (_direction > 0)
            {
                _body.velocity = Vector2.right * _rollForce;
            }
        }
    }

    public void FinishRoll() //chamado também na animação de Roll
    {
        _canRoll = true;
        _isRolling = false;
        _input.isRolling = false;
        gameObject.layer = LayerMask.NameToLayer("Player");
    }
    #endregion

    #region Slide
    void OnSlide()
    {
        if (_input.isSliding && !_isSliding) { _isSliding = true; }

        if (_isSliding && ((_timeSlide < _limitSlide) || _hitSlide)) //_hitSlide verifica se ainda tem GroundLayer em cima
        {

            DisableControls(); ;
            _timeSlide += Time.deltaTime;
            if (_direction < 0) { _body.velocity = Vector2.left * _slideForce; }
            else if (_direction > 0) { _body.velocity = Vector2.right * _slideForce; }
        }
        else if (_isSliding && _timeSlide >= _limitSlide)
        {
            if (!_hitSlide) //se ainda estiver em baixo do GroundLayer continua o Slide
            {
                _timeSlide = 0f;
                _input.isSliding = false;
                _isSliding = false;
                _body.velocity = Vector2.zero;
                EnabledControls();
            }
        }
    }

    public void CheckSlide()
    {
        _hitSlide = false;
        Vector2 position = new Vector2(transform.position.x, transform.position.y - 1f);
        RaycastHit2D _slideHit = RaycastSlide(position, Vector2.up, 2f, _groundLayer);

        if (_slideHit)
        {
            _hitSlide = true;
        }
    }

    #endregion

    #region Climb
    bool TouchingLadder()
    {
        return _collider.IsTouchingLayers(_ladderLayer);
    }

    bool TouchingVine()
    {
        return _collider.IsTouchingLayers(_vineLayer);
    }

    bool TouchingGrid()
    {
        return _collider.IsTouchingLayers(_gridLayer);
    }

    void OnClimb()
    {
        bool _circleUpLadder = Physics2D.OverlapCircle(transform.position + _checkPositionUp, _checkRadius, _ladderLayer);
        bool _circleDownLadder = Physics2D.OverlapCircle(transform.position + _checkPositionDown, _checkRadius, _ladderLayer);

        bool _circleUpVine = Physics2D.OverlapCircle(transform.position + _checkPositionUp, _checkRadius, _vineLayer);
        bool _circleDownVine = Physics2D.OverlapCircle(transform.position + _checkPositionDown, _checkRadius, _vineLayer);

        if ((_input.vertical >= 0.5 || _input.vertical <= -0.5) && (TouchingLadder() || TouchingVine()) && _input.horizontal <= 0.05)
        {
            _onClimbing = true;
            _body.isKinematic = true;

            if (TouchingLadder())
            {
                float _xPos = _ladder.position.x;
                transform.position = new Vector2(_xPos, transform.position.y);
            }
            else if (TouchingVine())
            {
                float _xPos = _vine.position.x;
                transform.position = new Vector2(_xPos, transform.position.y);
            }

        }

        if (_onClimbing && TouchingLadder())
        {
            if (!_circleUpLadder && _input.vertical >= 0)
            {
                FinishClimb();
                return;
            }

            if (!_circleDownLadder && _input.vertical <= 0)
            {
                FinishClimb();
                return;
            }

            //corrigindo o bug do controle não pegar o 1 e -1 no analógico
            float speed = _input.vertical;
            if (speed > -1 && speed < 0)
            {
                speed = -1;
            }
            else if (speed < 1 && speed > 0)
            {
                speed = 1;
            }
            else
            {
                speed = _input.vertical;
            }

            float y = speed * _climbSpeed;
            _body.velocity = new Vector2(0, y);
        }
        else if (_onClimbing && TouchingVine())
        {
            if (!_circleUpVine && _input.vertical >= 0)
            {
                FinishClimb();
                return;
            }

            if (!_circleDownVine && _input.vertical <= 0)
            {
                FinishClimb();
                return;
            }

            //corrigindo o bug do controle não pegar o 1 e -1 no analógico
            float speed = _input.vertical;
            if (speed > -1 && speed < 0)
            {
                speed = -1;
            }
            else if (speed < 1 && speed > 0)
            {
                speed = 1;
            }
            else
            {
                speed = _input.vertical;
            }

            float y = speed * _climbSpeed;
            _body.velocity = new Vector2(0, y);
        }
    }

    void FinishClimb()
    {
        if (_onClimbing)
        {
            _onClimbing = false;
            _body.isKinematic = false;
            _canMove = true;
        }
    }

    void OnGrid()
    {
        if ((_input.vertical >= 0.5 || _input.vertical <= -0.5) && TouchingGrid() && !_isGrounded)
        {
            _body.velocity = Vector2.zero;
            _isGriding = true;
            _body.isKinematic = true;
        }

        if (!TouchingGrid())
        {
            FinishGrid();
        }

        if (_isGriding && TouchingGrid())
        {
            float vertical = _input.vertical;
            float horizontal = _input.horizontal;

            //corrigindo o bug do controle não pegar o 1 e -1 no analógico
            if (vertical > -1 && vertical < 0) { vertical = -1; }
            else if (vertical < 1 && vertical > 0) { vertical = 1; }
            else { vertical = _input.vertical; }

            float y = vertical * _climbSpeed;
            float x = horizontal * _climbSpeed;
            _body.velocity = new Vector2(x, y);
        }
    }

    void FinishGrid()
    {
        if (_isGriding)
        {
            _isGriding = false;
            _body.isKinematic = false;
            //_canMove = true;
        }
    }

    #endregion

    #region BridgePass
    public void CheckBridge()
    {
        _bridgeHit = false;
        RaycastHit2D _bridgeHitUp = RaycastBridge(transform.position, Vector2.up, _bridgeCheckDistance, _bridgeLayer);

        if (_bridgeHitUp)
        {
            _bridgeHit = true;
        }
    }

    public void SetBridge(Bridge bridge)
    {
        _bridge = bridge;
    }

    public void PassThroughBridge()
    {
        if (_bridge != null)
        {
            _bridge.PassingThrough();
        }
    }
    #endregion

    #region Hit
    void OnHit()
    {
        if (!_onHit || _dead)
            return;

        _input.OnHit(); //cancela os inputs quando toma dano
        //_input.horizontal = 0;
        _body.velocity = Vector2.zero;

        if (_knockback) { _body.AddForce(Vector2.left * _knockbackForce, ForceMode2D.Impulse); }
        else { _body.AddForce(Vector2.right * _knockbackForce, ForceMode2D.Impulse); }
    }

    public void FinishKnockback() //chamado na animação de Hit
    {
        _onHit = false;
        _canMove = true;
        _health.FinishHit();
        _knockback = false;
    }
    #endregion

    public void CreateDust(int dir)
    {
        if (_onWater)
            return;

        Vector3 _scale = _dust.transform.localScale;
        Vector3 _position = _groundCheck.position;
        _scale.x *= dir;
        _dust.transform.localScale = _scale;

        Instantiate(_dust, _position, Quaternion.identity);
    }

    public void CreateRecoveryEffect()
    {
        Instantiate(_recoveryEffect, transform.position, Quaternion.identity);
    }

    RaycastHit2D Raycast(Vector2 offset, Vector2 rayDirection, float length, LayerMask layerMask)
    { //DISPARA UM RAIO DE COLISÃO PARA DETECTAR O CHÃO
        Vector2 _playerPos = _groundCheck.position;
        RaycastHit2D _hit = Physics2D.Raycast(_playerPos + offset, rayDirection, length, layerMask);
        Color _color = _hit ? Color.red : Color.green;
        Debug.DrawRay(_playerPos + offset, rayDirection * length, _color);
        return _hit;
    }

    RaycastHit2D RaycastBridge(Vector2 offset, Vector2 rayDirection, float length, LayerMask layerMask)
    {
        Vector2 _position = new Vector2(offset.x, offset.y + 0.10f);
        RaycastHit2D _hit = Physics2D.Raycast(_position, rayDirection, length, layerMask);
        Color _color = _hit ? Color.cyan : Color.blue;
        Debug.DrawRay(_position, rayDirection * length, _color);
        return _hit;
    }

    RaycastHit2D RaycastSlide(Vector2 offset, Vector2 rayDirection, float length, LayerMask layerMask)
    {
        Vector2 _position = new Vector2(offset.x, offset.y + 0.10f);
        RaycastHit2D _hit = Physics2D.Raycast(_position, rayDirection, length, layerMask);
        Color _color = _hit ? Color.green : Color.white;
        Debug.DrawRay(_position, rayDirection * length, _color);
        return _hit;
    }

    public IEnumerator SwimControl()
    {
        yield return new WaitForSeconds(_swinLimit);
        _input.isJumping = false;
        _canSwin = true;
    }

    private void OnDrawGizmos()
    {
        //ladder
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position + _checkPositionUp, _checkRadius);
        Gizmos.DrawWireSphere(transform.position + _checkPositionDown, _checkRadius);
    }
}
