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

    //Climb Ledge
    private float wallRayDistance = 0.24f;
    private BoxCollider2D colliderClimbLedge;
    [HideInInspector] public bool touchingWall;
    [BoxGroup("ClimbLedge")] public GameObject _climbLedgePoint;
    [BoxGroup("ClimbLedge")] public LayerMask _groundLayer;

    //Water
    [HideInInspector] public bool outWaterHit;
    [HideInInspector] public bool inWaterHit;

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
    }

    private void OnTriggerExit2D(Collider2D other)
    {

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
        //if (other.gameObject.layer == LayerMask.NameToLayer("Ground")) { player.playerMovement.FinishDash(); }
    }

    private void OnDrawGizmos()
    {
        //box
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, (Vector2)transform.position + Vector2.right * transform.localScale.x * distanceBox);
    }

    RaycastHit2D RaycastWallJump(Vector2 position, Vector2 rayDirection, float length, LayerMask layerMask)
    {
        RaycastHit2D _hit = Physics2D.Raycast(position, rayDirection, length, layerMask);
        Color _color = _hit ? Color.yellow : Color.magenta;
        Debug.DrawRay(position, rayDirection * length, _color);
        return _hit;
    }
}
