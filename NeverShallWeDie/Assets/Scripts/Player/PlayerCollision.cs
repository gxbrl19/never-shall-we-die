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

    //Acid
    [HideInInspector] public bool _outAcidHit;
    [HideInInspector] public bool _inAcidHit;
    [BoxGroup("Acid")] public GameObject _dropAcid;
    [BoxGroup("Acid")] public LayerMask _acidLayer;

    //Climb Ledge   
    private float _wallRayDistance = 0.24f;
    private BoxCollider2D _colliderClimbLedge;
    [HideInInspector] public bool _onWall;
    [BoxGroup("ClimbLedge")] public GameObject _climbLedgePoint;
    [BoxGroup("ClimbLedge")] public LayerMask _groundLayer;

    Collider2D _collider;
    Player _player;
    PlayerInputs _input;
    PlayerHealth _health;

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
    }

    private void Update()
    {

    }

    private void FixedUpdate()
    {
        BoxMoveCheck();
        WaterCheck();
        AcidCheck();
        WallCheck();
    }

    public void BoxMoveCheck()
    {
        Physics2D.queriesStartInColliders = false;
        RaycastHit2D _hit = Physics2D.Raycast(transform.position, Vector2.right * transform.localScale.x, _distanceBox, _boxLayer);

        if (_hit)
        {
            _player._canGrab = true;
        }
        else
        {
            _player._canGrab = false;
        }

        //
        if (_player._canGrab && _input.isGrabing)
        {
            _box = _hit.collider.gameObject;
            _player._isGrabing = true;
            _box.GetComponent<Rigidbody2D>().mass = 1f;
            _box.GetComponent<FixedJoint2D>().enabled = true;
            _box.GetComponent<FixedJoint2D>().connectedBody = this.GetComponent<Rigidbody2D>();
        }
        else if (!_input.isGrabing || !_player._canGrab)
        {
            _player._isGrabing = false;
            _box.GetComponent<Rigidbody2D>().mass = 500f;
            _box.GetComponent<FixedJoint2D>().enabled = false;
        }
    }

    void WaterCheck()
    {
        if (_player._dead)
            return;

        _outWaterHit = !Physics2D.OverlapCircle(_outWaterPoint.transform.position, _waterCheckRadius, _waterLayer);
        _inWaterHit = Physics2D.OverlapCircle(_outWaterPoint.transform.position - _waterCheckDistance, _waterCheckRadius, _waterLayer);

        if (_inWaterHit)
        {
            _player._onWater = true;
        }
        else
        {
            _player._onWater = false;
        }
    }

    void AcidCheck()
    {
        if (_player._dead)
            return;

        _outAcidHit = !Physics2D.OverlapCircle(_outWaterPoint.transform.position, _waterCheckRadius, _acidLayer);
        _inAcidHit = Physics2D.OverlapCircle(_outWaterPoint.transform.position - _waterCheckDistance, _waterCheckRadius, _acidLayer);

        if (_inAcidHit)
        {
            _player._onAcid = true;
        }
        else
        {
            _player._onAcid = false;
        }
    }

    void WallCheck()
    {
        _onWall = false;

        Vector3 _offset = new Vector3(0f, -0.5f, 0f);
        RaycastHit2D _upRay = RaycastWallJump(_climbLedgePoint.transform.position, Vector2.right * _player._direction, _wallRayDistance, _groundLayer);
        RaycastHit2D _downRay = RaycastWallJump(_climbLedgePoint.transform.position + _offset, Vector2.right * _player._direction, _wallRayDistance, _groundLayer);

        if ((!_upRay && _downRay) && !_player._onWater)
        {
            _onWall = true;
            _player._canDoubleJump = false;
            _input.isParachuting = false;
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
            _player._ladder = other.transform;
        }

        if (other.gameObject.layer == LayerMask.NameToLayer("Vine"))
        {
            _player._vine = other.transform;
        }

        if (other.gameObject.layer == LayerMask.NameToLayer("Water"))
        {
            _player._onWater = true;
            Vector3 position = other.gameObject.GetComponent<Collider2D>().bounds.ClosestPoint(new Vector3(transform.position.x, other.transform.position.y, other.transform.position.z));
            Instantiate(_dropWater, position, other.transform.rotation);

            if (_player._isRolling)
            {
                _player.FinishRoll(); //cancela o Roll ao entrar na água
                return;
            }

            _player.EnterInWater();
        }

        if (other.gameObject.layer == LayerMask.NameToLayer("Acid"))
        {
            _player._onAcid = true;
            Vector3 position = other.gameObject.GetComponent<Collider2D>().bounds.ClosestPoint(new Vector3(transform.position.x, other.transform.position.y, other.transform.position.z));
            Instantiate(_dropAcid, position, other.transform.rotation);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Water"))
        {
            _player._onWater = false;
            Vector3 position = other.gameObject.GetComponent<Collider2D>().bounds.ClosestPoint(new Vector3(transform.position.x, other.transform.position.y, other.transform.position.z));
            Instantiate(_dropWater, position, other.transform.rotation);
        }

        if (other.gameObject.layer == LayerMask.NameToLayer("Acid"))
        {
            _player._onAcid = false;
            Vector3 position = other.gameObject.GetComponent<Collider2D>().bounds.ClosestPoint(new Vector3(transform.position.x, other.transform.position.y, other.transform.position.z));
            Instantiate(_dropAcid, position, other.transform.rotation);
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
        if (other.gameObject.layer == LayerMask.NameToLayer("Ground")) { _player.FinishRoll(); }
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
