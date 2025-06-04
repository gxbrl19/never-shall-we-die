using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipOpenWorld : MonoBehaviour
{
    public static ShipOpenWorld instance;

    float _accelerationFactor = 1.5f;
    float _turnFactor = 0.8f;
    float _accelerationInput = 0;
    float _steeringInput = 0;
    float _rotationAngle = 0;
    float _maxSpeed = 2f;

    [HideInInspector] public bool _canMove;
    bool _canAttack = true;

    [SerializeField] Transform _cannonPosition;
    [SerializeField] GameObject _shipCannon;
    [SerializeField] GameObject _cannonEffect;

    [HideInInspector] public bool _submarine;
    [HideInInspector] public Transform _targetSubmarine;
    float _speedSubmarine = 1f;

    [HideInInspector] public bool _inPropulsion;
    float _propulsionForce = 10f;

    Rigidbody2D _body;
    Animator _animation;
    Vector2 _movement;
    Collider2D _collider;
    ShipInput _input;

    private void Awake()
    {
        instance = this;
        _body = GetComponent<Rigidbody2D>();
        _animation = GetComponent<Animator>();
        _collider = GetComponent<Collider2D>();
        _input = GetComponent<ShipInput>();
    }

    void Start()
    {
        _canMove = true;

        float _xPosition = GameManager.instance._shipPosX;
        float _yPosition = GameManager.instance._shipPosY;
        transform.position = new Vector3(_xPosition, _yPosition, 0f);
    }

    void Update()
    {
        SubmarineMove();

        if (!_canMove) { return; }
        InputVector();
        Cannon();
    }

    void FixedUpdate()
    {
        if (!_canMove) { return; }
        ApplyEngineForce();
        ApplySteering();
        Deceleration();
        _animation.SetFloat("Speed", _body.velocity.magnitude);
    }

    void ApplyEngineForce()
    {
        if (_input.accelerate == 0f) { return; }

        Vector2 engineForce = transform.up * _accelerationInput * _accelerationFactor;

        if (_body.velocity.magnitude < _maxSpeed) // Verifica se o corpo já atingiu ou ultrapassou a velocidade máxima na direção da força
        {
            _body.AddForce(engineForce, ForceMode2D.Force);
        }
        else
        {
            // Opcional: Se já estiver na velocidade máxima, você pode aplicar uma força menor
            // ou nenhuma força para evitar que a velocidade exceda demais.
            float currentSpeed = _body.velocity.magnitude;
            Vector2 brakingForce = -_body.velocity.normalized * (currentSpeed - _maxSpeed) * _accelerationFactor;
            _body.AddForce(brakingForce, ForceMode2D.Force);
        }
    }

    void Deceleration()
    {
        if (_input.accelerate > 0f) { return; }

        if (_body.velocity.magnitude > 0.1f) // Verificar se o Rigidbody está em movimento
        {
            Vector3 decelerationForceVector = -_body.velocity.normalized * _accelerationFactor;
            _body.AddForce(decelerationForceVector, ForceMode2D.Force);
        }
    }

    void ApplySteering()
    {
        _rotationAngle -= _steeringInput * _turnFactor; //muda o angulo de rotação baseado no input
        _body.MoveRotation(_rotationAngle); //aplica a direção de acordo com a rotação do navio
    }

    void InputVector()
    {
        _steeringInput = Input.GetAxisRaw("Horizontal");
        _accelerationInput = _input.accelerate;
    }

    public void SavePos()
    {
        GameManager.instance._shipPosX = transform.position.x;
        GameManager.instance._shipPosY = transform.position.y;
    }

    public void SubmarineMove()
    {
        if (_submarine)
        {
            _collider.enabled = false;
            transform.position = Vector3.MoveTowards(transform.position, _targetSubmarine.position, _speedSubmarine * Time.deltaTime);

            if (transform.position == _targetSubmarine.position)
            {
                _animation.SetBool("Submarine", false);
            }
        }
    }

    public void ActiveAnimation()
    {
        _canMove = false;
        _animation.SetBool("Submarine", true);
    }

    public void BeginSubmarine() //chamado na animação
    {
        _submarine = true;
        _input.submarine = false;
    }

    public void FinishSubmarine() //chamado na animação
    {
        _submarine = false;
        _canMove = true;
        _collider.enabled = true;
    }

    void Propulsion()
    {
        if (_input.propulsion)
        {
            _inPropulsion = true;
            _body.velocity = new Vector2(_movement.x * _propulsionForce, _movement.y * _propulsionForce);
        }
        else if (!_input.propulsion)
        {
            _inPropulsion = false;
        }
    }

    void Cannon()
    {
        if (_canAttack && _input.cannon)
        {
            _canAttack = false;

            GameObject ball = Instantiate(_shipCannon.gameObject, _cannonPosition.position, Quaternion.identity);
            float x = _cannonPosition.localPosition.x;
            float y = _cannonPosition.localPosition.y;
            ball.GetComponent<ShipCannon>()._directionX = x;
            ball.GetComponent<ShipCannon>()._directionY = y;

            Instantiate(_cannonEffect, _cannonPosition.position, Quaternion.identity);

            Invoke("FinishCannon", 2f);
        }
    }

    public void FinishCannon() //chamado na função Cannon
    {
        _canAttack = true;
    }

    public void StopMove()
    {
        _body.velocity = Vector2.zero;
    }
}
