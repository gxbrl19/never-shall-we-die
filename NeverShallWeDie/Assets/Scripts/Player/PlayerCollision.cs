using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    //Box
    private float _distanceBox = 0.6f;
    [BoxGroup("Box")] public LayerMask _boxLayer;
    [BoxGroup("Box")] public GameObject _box;

    //Water
    private float _waterCheckRadius = 0.1f;
    private Vector3 _waterCheckDistance = new Vector3(0f, 0.7f, 0f);
    [HideInInspector] public bool _outWaterHit;
    [HideInInspector] public bool _inWaterHit;
    [BoxGroup("Water")] public GameObject _outWaterPoint;
    [BoxGroup("Water")] public GameObject _dropWater;
    [BoxGroup("Water")] public LayerMask _waterLayer;
    [BoxGroup("Water")] public GameObject _dropLava;

    //Climb Ledge
    private float _wallRayDistance = 0.24f;
    private BoxCollider2D _colliderClimbLedge;
    [HideInInspector] public bool _onWall;
    [BoxGroup("ClimbLedge")] public GameObject _climbLedgePoint;
    [BoxGroup("ClimbLedge")] public LayerMask _groundLayer;

    //Grid


    Collider2D _collider;
    Player _player;
    PlayerInputs _input;
    PlayerHealth _health;
    PlayerAudio _audio;

    private void Awake()
    {
        _colliderClimbLedge = _climbLedgePoint.GetComponent<BoxCollider2D>();
    }

    void Start()
    {
        _collider = GetComponent<Collider2D>();
        _player = GetComponent<Player>();
        _input = GetComponent<PlayerInputs>();
        _health = GetComponent<PlayerHealth>();
        _audio = GetComponent<PlayerAudio>();
    }

    private void Update()
    {

    }

    private void FixedUpdate()
    {
        BoxMoveCheck();
        WaterCheck();
        WallCheck();
    }

    public void BoxMoveCheck()
    {
        Physics2D.queriesStartInColliders = false;
        RaycastHit2D _hit = Physics2D.Raycast(transform.position, Vector2.right * transform.localScale.x, _distanceBox, _boxLayer);

        if (_hit && _player.isGrounded)
        {
            _player.canGrab = true;
        }
        else
        {
            _player.canGrab = false;
        }

        //
        if (_player.canGrab && _input.pressGrab)
        {
            _box = _hit.collider.gameObject;
            _player.isGrabing = true;
            _box.GetComponent<Rigidbody2D>().mass = 1f;
            _box.GetComponent<FixedJoint2D>().enabled = true;
            _box.GetComponent<FixedJoint2D>().connectedBody = this.GetComponent<Rigidbody2D>();
        }
        else if (!_input.pressGrab || !_player.canGrab)
        {
            _player.isGrabing = false;
            _box.GetComponent<Rigidbody2D>().mass = 500f;
            _box.GetComponent<FixedJoint2D>().enabled = false;
        }
    }

    void WaterCheck()
    {
        if (_player.isDead)
            return;

        _outWaterHit = !Physics2D.OverlapCircle(_outWaterPoint.transform.position, _waterCheckRadius, _waterLayer);
        _inWaterHit = Physics2D.OverlapCircle(_outWaterPoint.transform.position - _waterCheckDistance, _waterCheckRadius, _waterLayer);

        if (_inWaterHit)
        {
            _player.onWater = true;
        }
        else
        {
            _player.onWater = false;
        }
    }

    void WallCheck()
    {
        _onWall = false;

        Vector3 _offset = new Vector3(0f, -0.5f, 0f);
        RaycastHit2D _upRay = RaycastWallJump(_climbLedgePoint.transform.position, Vector2.right * _player.playerMovement.playerDirection, _wallRayDistance, _groundLayer);
        RaycastHit2D _downRay = RaycastWallJump(_climbLedgePoint.transform.position + _offset, Vector2.right * _player.playerMovement.playerDirection, _wallRayDistance, _groundLayer);

        if ((!_upRay && _downRay) && !_player.onWater)
        {
            _onWall = true;
            _input.pressParachute = false;
        }

        if (_onWall)
        {
            _colliderClimbLedge.enabled = true;
        }
        else
        {
            _colliderClimbLedge.enabled = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Ladder"))
        {
            _player.playerMovement.ladder = other.transform;
        }

        if (other.gameObject.layer == LayerMask.NameToLayer("Vine"))
        {
            _player.playerMovement.vine = other.transform;
        }

        if (other.gameObject.layer == LayerMask.NameToLayer("Water"))
        {
            //verifica primeiro se é a layer do Player ou do Invencible que está entrando na água
            if (gameObject.layer != LayerMask.NameToLayer("Player") && gameObject.layer != LayerMask.NameToLayer("Invencible")) { return; }

            _player.onWater = true;
            Vector3 position = other.gameObject.GetComponent<Collider2D>().bounds.ClosestPoint(new Vector3(transform.position.x, other.transform.position.y, other.transform.position.z));
            _audio.PlayWaterSplash();

            //drop
            if (other.gameObject.tag.Equals("Lava")) { Instantiate(_dropLava, position, other.transform.rotation); }
            else { Instantiate(_dropWater, position, other.transform.rotation); }

            if (_player.isRoll)
            {
                _player.playerMovement.FinishRoll(); //cancela o Roll ao entrar na água
                return;
            }

            if (_input.pressParachute == true) { _input.pressParachute = false; } //cancela o parachute

            _player.EnterInWater();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Water"))
        {
            //verifica primeiro se é a layer do Player ou do Invencible que está saindo da água
            if (gameObject.layer != LayerMask.NameToLayer("Player") && gameObject.layer != LayerMask.NameToLayer("Invencible")) { return; }

            _player.onWater = false;
            Vector3 position = other.gameObject.GetComponent<Collider2D>().bounds.ClosestPoint(new Vector3(transform.position.x, other.transform.position.y, other.transform.position.z));
            _audio.PlayWaterSplash();

            //drop
            if (other.gameObject.tag.Equals("Lava")) { Instantiate(_dropLava, position, other.transform.rotation); }
            else { Instantiate(_dropWater, position, other.transform.rotation); }
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {

    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        //if (other.gameObject.tag.Equals("Platform")) { this.transform.parent = other.transform; }
        if (other.gameObject.layer == LayerMask.NameToLayer("Platform")) { this.transform.parent = other.transform; }

        //cancela o jump ataque ao cair no chão
        if (other.gameObject.layer == LayerMask.NameToLayer("Ground")) { _input.isAttacking = false; }
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        //if (other.gameObject.tag.Equals("Platform")) { this.transform.parent = null; }
        if (other.gameObject.layer == LayerMask.NameToLayer("Platform")) { this.transform.parent = null; }

        //cancela o Roll ao sair do chão
        if (other.gameObject.layer == LayerMask.NameToLayer("Ground")) { _player.playerMovement.FinishRoll(); }
    }

    private void OnDrawGizmos()
    {
        //box
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, (Vector2)transform.position + Vector2.right * transform.localScale.x * _distanceBox);

        //water
        Gizmos.color = Color.blue; //check out water
        Gizmos.DrawWireSphere(_outWaterPoint.transform.position, _waterCheckRadius);
        Gizmos.DrawWireSphere(_outWaterPoint.transform.position, _waterCheckRadius);
        Gizmos.color = Color.blue; //check in water
        Gizmos.DrawWireSphere(_outWaterPoint.transform.position - _waterCheckDistance, _waterCheckRadius);
        Gizmos.DrawWireSphere(_outWaterPoint.transform.position - _waterCheckDistance, _waterCheckRadius);
    }

    RaycastHit2D RaycastWallJump(Vector2 position, Vector2 rayDirection, float length, LayerMask layerMask)
    {
        RaycastHit2D _hit = Physics2D.Raycast(position, rayDirection, length, layerMask);
        Color _color = _hit ? Color.yellow : Color.magenta;
        Debug.DrawRay(position, rayDirection * length, _color);
        return _hit;
    }
}
