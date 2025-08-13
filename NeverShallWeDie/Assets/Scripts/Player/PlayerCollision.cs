using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    //Box
    private float distanceBox = 0.6f;
    [BoxGroup("Box")] public LayerMask _boxLayer;
    [BoxGroup("Box")] public GameObject _box;

    //Water
    private float waterCheckRadius = 0.1f;
    private Vector3 waterCheckDistance = new Vector3(0f, 0.7f, 0f);
    [HideInInspector] public bool outWaterHit;
    [HideInInspector] public bool inWaterHit;
    [BoxGroup("Water")] public GameObject _outWaterPoint;
    [BoxGroup("Water")] public GameObject _dropWater;
    [BoxGroup("Water")] public LayerMask _waterLayer;
    [BoxGroup("Water")] public GameObject _dropLava;

    //Climb Ledge
    private float wallRayDistance = 0.24f;
    private BoxCollider2D colliderClimbLedge;
    [HideInInspector] public bool touchingWall;
    [BoxGroup("ClimbLedge")] public GameObject _climbLedgePoint;
    [BoxGroup("ClimbLedge")] public LayerMask _groundLayer;

    //Grid

    Player player;

    private void Awake()
    {
        colliderClimbLedge = _climbLedgePoint.GetComponent<BoxCollider2D>();
    }

    void Start()
    {
        player = GetComponent<Player>();
    }

    private void Update()
    {

    }

    private void FixedUpdate()
    {
        BoxMoveCheck();
        WaterCheck();
        LedgeCheck();
        WallCheck();
    }

    public void BoxMoveCheck()
    {
        Physics2D.queriesStartInColliders = false;
        RaycastHit2D _hit = Physics2D.Raycast(transform.position, Vector2.right * transform.localScale.x, distanceBox, _boxLayer);

        if (_hit && player.isGrounded)
        {
            player.canGrab = true;
        }
        else
        {
            player.canGrab = false;
        }

        //
        if (player.canGrab && player.playerInputs.pressGrab)
        {
            _box = _hit.collider.gameObject;
            player.isGrabing = true;
            _box.GetComponent<Rigidbody2D>().mass = 1f;
            _box.GetComponent<FixedJoint2D>().enabled = true;
            _box.GetComponent<FixedJoint2D>().connectedBody = this.GetComponent<Rigidbody2D>();
        }
        else if (!player.playerInputs.pressGrab || !player.canGrab)
        {
            player.isGrabing = false;
            _box.GetComponent<Rigidbody2D>().mass = 500f;
            _box.GetComponent<FixedJoint2D>().enabled = false;
        }
    }

    void WaterCheck()
    {
        if (player.isDead)
            return;

        outWaterHit = !Physics2D.OverlapCircle(_outWaterPoint.transform.position, waterCheckRadius, _waterLayer);
        inWaterHit = Physics2D.OverlapCircle(_outWaterPoint.transform.position - waterCheckDistance, waterCheckRadius, _waterLayer);

        if (inWaterHit)
        {
            player.onWater = true;
        }
        else
        {
            player.onWater = false;
        }
    }

    void LedgeCheck()
    {
        player.onLedge = false;

        Vector3 offset = new Vector3(0f, -0.5f, 0f);
        RaycastHit2D upRay = RaycastWallJump(_climbLedgePoint.transform.position, Vector2.right * player.playerMovement.playerDirection, wallRayDistance, _groundLayer);
        RaycastHit2D downRay = RaycastWallJump(_climbLedgePoint.transform.position + offset, Vector2.right * player.playerMovement.playerDirection, wallRayDistance, _groundLayer);

        if ((!upRay && downRay) && !player.onWater)
        {
            player.onLedge = true;
            player.playerInputs.pressParachute = false;
        }

        if (player.onLedge)
            colliderClimbLedge.enabled = true;
        else
            colliderClimbLedge.enabled = false;

    }

    void WallCheck()
    {
        Vector3 offset = new Vector3(0f, -0.5f, 0f);
        RaycastHit2D upRay = RaycastWallJump(_climbLedgePoint.transform.position, Vector2.right * player.playerMovement.playerDirection, wallRayDistance, _groundLayer);
        RaycastHit2D downRay = RaycastWallJump(_climbLedgePoint.transform.position + offset, Vector2.right * player.playerMovement.playerDirection, wallRayDistance, _groundLayer);

        touchingWall = upRay && downRay;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Ladder"))
        {
            player.playerMovement.ladder = other.transform;
        }

        if (other.gameObject.layer == LayerMask.NameToLayer("Vine"))
        {
            player.playerMovement.vine = other.transform;
        }

        if (other.gameObject.layer == LayerMask.NameToLayer("Water"))
        {
            //verifica primeiro se é a layer do Player ou do Invencible que está entrando na água
            if (gameObject.layer != LayerMask.NameToLayer("Player") && gameObject.layer != LayerMask.NameToLayer("Invencible")) { return; }

            player.onWater = true;
            Vector3 position = other.gameObject.GetComponent<Collider2D>().bounds.ClosestPoint(new Vector3(transform.position.x, other.transform.position.y, other.transform.position.z));
            player.playerAudio.PlayWaterSplash();

            //drop
            if (other.gameObject.tag.Equals("Lava")) { Instantiate(_dropLava, position, other.transform.rotation); }
            else { Instantiate(_dropWater, position, other.transform.rotation); }

            if (player.isRolling)
            {
                player.playerMovement.FinishRoll(); //cancela o Roll ao entrar na água
                return;
            }

            if (player.playerInputs.pressParachute == true) { player.playerInputs.pressParachute = false; } //cancela o parachute

            player.EnterInWater();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Water"))
        {
            //verifica primeiro se é a layer do Player ou do Invencible que está saindo da água
            if (gameObject.layer != LayerMask.NameToLayer("Player") && gameObject.layer != LayerMask.NameToLayer("Invencible")) { return; }

            player.onWater = false;
            Vector3 position = other.gameObject.GetComponent<Collider2D>().bounds.ClosestPoint(new Vector3(transform.position.x, other.transform.position.y, other.transform.position.z));
            player.playerAudio.PlayWaterSplash();

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
        if (other.gameObject.layer == LayerMask.NameToLayer("Ground")) { player.playerInputs.pressAttack = false; }
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        //if (other.gameObject.tag.Equals("Platform")) { this.transform.parent = null; }
        if (other.gameObject.layer == LayerMask.NameToLayer("Platform")) { this.transform.parent = null; }

        //cancela o Roll ao sair do chão
        if (other.gameObject.layer == LayerMask.NameToLayer("Ground")) { player.playerMovement.FinishRoll(); }
    }

    private void OnDrawGizmos()
    {
        //box
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, (Vector2)transform.position + Vector2.right * transform.localScale.x * distanceBox);

        //water
        Gizmos.color = Color.blue; //check out water
        Gizmos.DrawWireSphere(_outWaterPoint.transform.position, waterCheckRadius);
        Gizmos.DrawWireSphere(_outWaterPoint.transform.position, waterCheckRadius);
        Gizmos.color = Color.blue; //check in water
        Gizmos.DrawWireSphere(_outWaterPoint.transform.position - waterCheckDistance, waterCheckRadius);
        Gizmos.DrawWireSphere(_outWaterPoint.transform.position - waterCheckDistance, waterCheckRadius);
    }

    RaycastHit2D RaycastWallJump(Vector2 position, Vector2 rayDirection, float length, LayerMask layerMask)
    {
        RaycastHit2D _hit = Physics2D.Raycast(position, rayDirection, length, layerMask);
        Color _color = _hit ? Color.yellow : Color.magenta;
        Debug.DrawRay(position, rayDirection * length, _color);
        return _hit;
    }
}
