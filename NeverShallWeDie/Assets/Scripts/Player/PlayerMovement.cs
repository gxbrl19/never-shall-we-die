using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    //Config
    [HideInInspector] public int playerDirection = 1;
    [HideInInspector] public float initialGravity;
    [HideInInspector] public float speed;
    private float normalSpeed = 8f;

    //Check Ground
    private float footOffset = 0.28f;
    private float groundOffset = -0.2f;
    private float groundDistance = 0.3f;

    //Jump
    private float jumpForce = 16f;

    //Jump Hold
    private float jumpHoldForce = 1.7f;
    private float jumpHoldDuration = 0.17f;
    private float jumpTime;

    //Ghost Jump
    private float ghostDuration = 0.15f;
    private float ghostTime;

    //Roll
    private bool canRoll = true;
    private float rollForce = 13f;
    private float rollCooldown = 0.8f;
    private float lastRollTime = -Mathf.Infinity;

    //Slope
    private float slopeCheckDistance = 1f;
    private float slopeDownAngle;
    private float slopeDownAngleOld;
    private float slopeSideAngle;
    private Vector2 slopeNormalPerp;
    private Vector2 colliderSize;

    //Climb
    private float climbSpeed = 3.5f;
    private float checkRadius = 0.5f;
    private Vector3 checkPositionUp = new Vector3(0f, 0.6f, 0f);
    private Vector3 checkPositionDown = new Vector3(0f, -1.2f, 0f);
    [HideInInspector] public Transform ladder;
    [HideInInspector] public Transform vine;

    //Bridge
    private float bridgeCheckDistance = -1.09f;

    //Grab
    private float grabSpeed = 2f;

    //Water
    private float waterGravity = 0.4f;
    private float waterSpeed = 4f;
    private float swimForce = 3f;
    private float jumpOutWater = 10f;
    private float swinLimit = 0.2f;


    [HideInInspector] public bool canSwin = true;
    [HideInInspector] public bool bridgeHit;

    Player player;

    void Awake()
    {
        player = GetComponent<Player>();
    }

    void Start()
    {
        speed = normalSpeed;
        initialGravity = player.rb.gravityScale;
        colliderSize = player.playerCollider.size;
    }

    void FixedUpdate()
    {
        PhysicsCheck();
        JumpControl();
        CheckMove();
        BlockMove();
        CheckSlope();
        CheckBridge();
        OnWater();
        OnClimb();
        OnGrid();
        OnRoll();
    }

    void PhysicsCheck()
    {
        if (!player.isDead)
        {
            player.isGrounded = false;

            //dispara um raio para baixo de cada pé para checagem do chão
            RaycastHit2D _leftFoot = Raycast(new Vector2(-footOffset, -groundOffset), Vector2.down, groundDistance, player._groundLayer);
            RaycastHit2D _rightFoot = Raycast(new Vector2(footOffset, -groundOffset), Vector2.down, groundDistance, player._groundLayer);

            if ((_leftFoot || _rightFoot) && !bridgeHit && !player.onClimbing)
            {
                player.isGrounded = true;
                player.playerInputs.isParachuting = false;
            }

            if (player.rb.velocity.y <= 0.0f)
            {
                player.isJumping = false;
            }
        }
        else
        {
            player.rb.velocity = Vector2.zero;
        }

        //gravidade na água
        if (player.onWater)
        {
            player.rb.gravityScale = waterGravity;
        }
        else
        {
            player.rb.gravityScale = initialGravity;
        }
    }


    void CheckMove()
    {
        float xVelocity = 0f;
        float yVelocity = 0f;
        float horizontal = player.playerInputs.GetHorizontal();

        if (player.isGrounded && !player.onSlope && !player.isGrabing && !player.isJumping && !player.onWater && !player.playerInputs.isAttacking && !player.isHealing && !player.isRolling) //chão comum
        {
            xVelocity = speed * horizontal;
            yVelocity = 0.0f;
            player.rb.velocity = new Vector2(xVelocity, yVelocity);
        }
        else if (player.isGrounded && player.onSlope && !player.isGrabing && !player.isJumping && !player.onWater && !player.isHealing & !player.playerInputs.isAttacking) //diagonal
        {
            xVelocity = speed * slopeNormalPerp.x * -horizontal;
            yVelocity = speed * slopeNormalPerp.y * -horizontal;
            player.rb.velocity = new Vector2(xVelocity, yVelocity);
        }
        else if (player.isGrounded && player.isGrabing && !player.onSlope && !player.isJumping) //empurrando caixa
        {
            xVelocity = grabSpeed * horizontal;
            yVelocity = player.rb.velocity.y;
            player.rb.velocity = new Vector2(xVelocity, yVelocity);
        }
        else if (!player.isGrounded && !player.onWater) //no ar
        {
            xVelocity = speed * horizontal;
            yVelocity = player.rb.velocity.y;
            player.rb.velocity = new Vector2(xVelocity, yVelocity);
        }
        else if (player.onWater) //na água
        {
            xVelocity = waterSpeed * horizontal;
            yVelocity = player.rb.velocity.y;
            player.rb.velocity = new Vector2(xVelocity, yVelocity);
        }

        if (playerDirection * xVelocity < 0)
        {
            Flip();
        }

        if (player.isGrounded)
        {
            ghostTime = Time.time + ghostDuration;
        }

        player.playerAnimations.xVelocity = Mathf.Abs(xVelocity);
        player.playerAnimations.yVelocity = Mathf.Abs(yVelocity);
    }

    void CheckSlope()
    {
        Vector2 _checkPos = transform.position - new Vector3(0.0f, colliderSize.y / 2);

        SlopeCheckHorizontal(_checkPos);
        SlopeCheckVertical(_checkPos);
    }

    void SlopeCheckHorizontal(Vector2 checkPos)
    {
        RaycastHit2D _slopeHitFront = Physics2D.Raycast(checkPos, transform.right, slopeCheckDistance, player._slopeLayer);
        RaycastHit2D _slopeHitBack = Physics2D.Raycast(checkPos, -transform.right, slopeCheckDistance, player._slopeLayer);

        if (_slopeHitFront)
        {
            player.onSlope = true;
            slopeSideAngle = Vector2.Angle(_slopeHitFront.normal, Vector2.up);
        }
        else if (_slopeHitBack)
        {
            player.onSlope = true;
            slopeSideAngle = Vector2.Angle(_slopeHitBack.normal, Vector2.up);
        }
        else
        {
            slopeSideAngle = 0.0f;
            player.onSlope = false;
        }
    }

    void SlopeCheckVertical(Vector2 checkPos)
    {
        RaycastHit2D _hit = Physics2D.Raycast(checkPos, Vector2.down, slopeCheckDistance, player._groundLayer);

        if (_hit)
        {
            slopeNormalPerp = Vector2.Perpendicular(_hit.normal).normalized;
            slopeDownAngle = Vector2.Angle(_hit.normal, Vector2.up);

            if (slopeDownAngle != slopeDownAngleOld)
            {
                player.onSlope = true;
            }

            slopeDownAngleOld = slopeDownAngle;

            Debug.DrawRay(_hit.point, slopeNormalPerp, Color.green);
            Debug.DrawRay(_hit.point, _hit.normal, Color.yellow);
        }

        if (player.onSlope && (player.playerInputs.horizontal == 0.0f || player.onSlope && player.playerInputs.isAttacking || player.isHealing))
        {
            player.rb.sharedMaterial = player._frictionMaterial;
        }
        else
        {
            player.rb.sharedMaterial = player._noFrictionMaterial;
        }
    }

    void JumpControl()
    {
        if (player.playerInputs.isJumping && (player.isGrounded || ghostTime > Time.time) && !player.onWater && player.playerInputs.vertical > -0.3f && !player.onClimbing && !player.inTornado && !player.isHealing && !player.isRolling) //pulo comum
        {
            player.isJumping = true;
            player.playerInputs.isJumping = false;

            player.rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);

            jumpTime = Time.time + jumpHoldDuration;

            ghostTime = Time.time;
            player.playerAudio.PlayJump();
            CreateDust(1);
        }
        else if (player.playerInputs.isJumping && player.onWater && canSwin && !player.playerCollision._onWall) // na água
        {
            if (player.playerCollision._outWaterHit && player.playerCollision._inWaterHit)
            {
                player.isJumping = true;
                player.playerInputs.isJumping = false;
                player.rb.velocity = Vector2.zero;
                player.rb.AddForce(Vector2.up * jumpOutWater, ForceMode2D.Impulse);
                jumpTime = Time.time + jumpHoldDuration;
                ghostTime = Time.time;
                player.playerAudio.PlayJump();
            }
            else if (!player.playerCollision._outWaterHit && player.playerCollision._inWaterHit)
            {
                player.rb.velocity = Vector2.zero;
                player.rb.AddForce(Vector2.up * swimForce, ForceMode2D.Impulse);
                canSwin = false;
            }
        }
        else if (player.playerInputs.isJumping && player.playerInputs.vertical <= -0.3f && !player.onClimbing) // pulo por baixo da plataforma
        {
            PassThroughBridge();
        }
        else if (player.playerInputs.isJumping && player.playerCollision._onWall && !player.isGrounded && !player.onWater) // pulo parede
        {
            player.isJumping = true;
            player.playerInputs.isJumping = false;

            player.rb.AddForce(new Vector2((jumpForce + 2f) * -playerDirection, jumpForce + 7f), ForceMode2D.Impulse);
        }
        else if (player.playerInputs.isJumping && (player.onClimbing || player.isGriding) && (TouchingVine() || TouchingLadder() || TouchingGrid())) //pulo da LADDER ou VINE
        {
            FinishClimb();
            FinishGrid();
            player.isJumping = true;
            player.playerInputs.vertical = 0f;
            player.rb.velocity = Vector2.zero;
            player.rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            jumpTime = Time.time + jumpHoldDuration;
            player.playerAudio.PlayJump();
        }

        if (player.isJumping)
        {
            if (player.playerInputs.jumpHeld)
            {
                player.rb.AddForce(Vector2.up * jumpHoldForce, ForceMode2D.Impulse);
            }

            if (jumpTime <= Time.time)
            {
                player.isJumping = false;
            }
        }

        player.playerInputs.isJumping = false;
    }

    void BlockMove() //verifica se está no ar e tira a gravidade do player
    {
        if (player.isDead || !player.canMove)
            return;

        if ((player.playerInputs.isAttacking && player.isGrounded && !player.onWater) || player.isHealing) { player.rb.velocity = Vector2.zero; }

        //para no ar
        if (player.playerInputs.isAirCuting || player.playerInputs.isTornado)
        {
            if (!player.isGrounded)
            {
                player.rb.velocity = Vector2.zero;
                player.rb.gravityScale = 0f;
            }
            else
            {
                player.rb.velocity = Vector2.zero;
            }
        }
    }

    #region Roll
    void OnRoll()
    {
        canRoll = Time.time > lastRollTime + rollCooldown;

        if (player.playerInputs.pressRoll && canRoll && !player.isRolling)
        {
            ExecuteRoll();
        }
    }

    private void ExecuteRoll()
    {
        gameObject.layer = LayerMask.NameToLayer("Invencible");

        player.isRolling = true;
        player.playerInputs.pressRoll = false;
        lastRollTime = Time.time; //marcando o momento que rolou


        //float horizontal = _input.GetHorizontal();
        Vector2 direction = player.playerMovement.playerDirection < 0 ? Vector2.left : Vector2.right;
        player.rb.velocity = direction * rollForce;
    }

    public void FinishRoll() //chamado também na animação de Roll
    {
        player.isRolling = false;
        gameObject.layer = LayerMask.NameToLayer("Player");
    }
    #endregion

    #region Water
    void OnWater()
    {
        if (player.isDead || !player.onWater)
            return;

        if (!canSwin)
        {
            StartCoroutine(SwimControl());
        }
    }

    public IEnumerator SwimControl()
    {
        yield return new WaitForSeconds(swinLimit);
        player.playerInputs.isJumping = false;
        canSwin = true;
    }
    #endregion

    #region BridgePass
    public void CheckBridge()
    {
        bridgeHit = false;
        RaycastHit2D _bridgeHitUp = RaycastBridge(transform.position, Vector2.up, bridgeCheckDistance, player._bridgeLayer);

        if (_bridgeHitUp)
        {
            bridgeHit = true;
        }
    }

    public void SetBridge(Bridge bridge)
    {
        player.bridge = bridge;
    }

    public void PassThroughBridge()
    {
        if (player.bridge != null)
        {
            player.bridge.PassingThrough();
        }
    }

    RaycastHit2D RaycastBridge(Vector2 offset, Vector2 rayDirection, float length, LayerMask layerMask)
    {
        Vector2 _position = new Vector2(offset.x, offset.y + 0.10f);
        RaycastHit2D _hit = Physics2D.Raycast(_position, rayDirection, length, layerMask);
        Color _color = _hit ? Color.cyan : Color.blue;
        Debug.DrawRay(_position, rayDirection * length, _color);
        return _hit;
    }
    #endregion

    #region Climb
    bool TouchingLadder()
    {
        return player.playerCollider.IsTouchingLayers(player._ladderLayer);
    }

    bool TouchingVine()
    {
        return player.playerCollider.IsTouchingLayers(player._vineLayer);
    }

    bool TouchingGrid()
    {
        return player.playerCollider.IsTouchingLayers(player._gridLayer);
    }

    void OnClimb()
    {
        bool circleUpLadder = Physics2D.OverlapCircle(transform.position + checkPositionUp, checkRadius, player._ladderLayer);
        bool circleDownLadder = Physics2D.OverlapCircle(transform.position + checkPositionDown, checkRadius, player._ladderLayer);

        bool circleUpVine = Physics2D.OverlapCircle(transform.position + checkPositionUp, checkRadius, player._vineLayer);
        bool circleDownVine = Physics2D.OverlapCircle(transform.position + checkPositionDown, checkRadius, player._vineLayer);

        if ((player.playerInputs.vertical >= 0.5 || player.playerInputs.vertical <= -0.5) && (TouchingLadder() || TouchingVine()) && player.playerInputs.horizontal <= 0.05)
        {
            player.onClimbing = true;
            player.rb.isKinematic = true;

            if (TouchingLadder())
            {
                float xPos = ladder.position.x;
                transform.position = new Vector2(xPos, transform.position.y);
            }
            else if (TouchingVine())
            {
                float xPos = vine.position.x;
                transform.position = new Vector2(xPos, transform.position.y);
            }

        }

        if (player.onClimbing && TouchingLadder())
        {
            if (!circleUpLadder && player.playerInputs.vertical >= 0)
            {
                FinishClimb();
                return;
            }

            if (!circleDownLadder && player.playerInputs.vertical <= 0)
            {
                FinishClimb();
                return;
            }

            //corrigindo o bug do controle não pegar o 1 e -1 no analógico
            float speed = player.playerInputs.vertical;
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
                speed = player.playerInputs.vertical;
            }

            float y = speed * climbSpeed;
            player.rb.velocity = new Vector2(0, y);
        }
        else if (player.onClimbing && TouchingVine())
        {
            if (!circleUpVine && player.playerInputs.vertical >= 0)
            {
                FinishClimb();
                return;
            }

            if (!circleDownVine && player.playerInputs.vertical <= 0)
            {
                FinishClimb();
                return;
            }

            //corrigindo o bug do controle não pegar o 1 e -1 no analógico
            float speed = player.playerInputs.vertical;
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
                speed = player.playerInputs.vertical;
            }

            float y = speed * climbSpeed;
            player.rb.velocity = new Vector2(0, y);
        }
    }

    void FinishClimb()
    {
        if (player.onClimbing)
        {
            player.onClimbing = false;
            player.rb.isKinematic = false;
            player.canMove = true;
        }
    }

    void OnGrid()
    {
        if ((player.playerInputs.vertical >= 0.5 || player.playerInputs.vertical <= -0.5) && TouchingGrid() && !player.isGrounded)
        {
            player.rb.velocity = Vector2.zero;
            player.isGriding = true;
            player.rb.isKinematic = true;
        }

        if (!TouchingGrid())
        {
            FinishGrid();
        }

        if (player.isGriding && TouchingGrid())
        {
            float vertical = player.playerInputs.vertical;
            float horizontal = player.playerInputs.GetHorizontal();

            //corrigindo o bug do controle não pegar o 1 e -1 no analógico
            if (vertical > -1 && vertical < 0) { vertical = -1; }
            else if (vertical < 1 && vertical > 0) { vertical = 1; }
            else { vertical = player.playerInputs.vertical; }

            float y = vertical * climbSpeed;
            float x = horizontal * climbSpeed;
            player.rb.velocity = new Vector2(x, y);
        }
    }

    void FinishGrid()
    {
        if (player.isGriding)
        {
            player.isGriding = false;
            player.rb.isKinematic = false;
            //_canMove = true;
        }
    }
    #endregion

    public void Flip()
    {
        if (player.isDead || !player.canMove || player.isGrabing || player.playerInputs.isAirCuting || player.inWaterSpin || player.playerInputs.isAttacking || player.isRolling)
            return;

        if (player.isGrounded)
        {
            CreateDust(-1);
        }

        player.playerMovement.playerDirection *= -1;

        Vector3 _scale = transform.localScale;
        _scale.x *= -1;
        transform.localScale = _scale;
    }

    public void CreateDust(int dir)
    {
        if (player.onWater)
            return;

        Vector3 scale = player._dust.transform.localScale;
        Vector3 position = player._groundCheck.position;
        scale.x *= dir;
        player._dust.transform.localScale = scale;

        Instantiate(player._dust, position, Quaternion.identity);
    }

    RaycastHit2D Raycast(Vector2 offset, Vector2 rayDirection, float length, LayerMask layerMask) //DISPARA UM RAIO DE COLISÃO PARA DETECTAR O CHÃO
    {
        Vector2 playerPos = player._groundCheck.position;
        RaycastHit2D hit = Physics2D.Raycast(playerPos + offset, rayDirection, length, layerMask);
        Color color = hit ? Color.red : Color.green;
        Debug.DrawRay(playerPos + offset, rayDirection * length, color);
        return hit;
    }

    private void OnDrawGizmos()
    {
        //ladder
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position + checkPositionUp, checkRadius);
        Gizmos.DrawWireSphere(transform.position + checkPositionDown, checkRadius);
    }
}
