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
    private float jumpForce = 27f;
    private float ghostDuration = 0.15f;
    private float ghostTime;
    private float holdJumpForce = 0.4f; // força extra enquanto segura
    private float maxHoldTime = 0.2f;   // tempo máximo de sustentação

    private float jumpHoldTimer;

    //Stamina
    [HideInInspector] public float maxStamina = 5f;
    public float currentStamina;
    [HideInInspector] public bool isExhausted = false; //se zerar a stamina
    private float staminaCooldown = 1f;
    private float rechargeStamina = 1f;
    private float lastStaminaTime = -Mathf.Infinity;

    //Dash
    private float dashForce = 30f;
    private float dashTimer;
    private float dashDuration = 0.1f;
    private float dashCooldownTimer;
    private float dashCooldown = 0.7f;

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
    [HideInInspector] public Bridge bridge;

    //Grab
    private float grabSpeed = 2f;

    //WallJump
    [Header("Wall Jump Settings")]
    private Vector2 wallJumpForce = new Vector2(10f, 20f);
    private float wallSlideSpeed = 1.6f;
    private float wallJumpLockTime = 0.25f; //tempo que bloqueia controle horizontal após pulo
    private float wallJumpLockCounter;
    private int wallDirection;

    //Water
    private float waterGravity = 0.4f;
    private float waterSpeed = 4f;
    private float swimForce = 4f;
    private float jumpOutWater = 20f;
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
        currentStamina = maxStamina;
        initialGravity = player.rb.gravityScale;
        colliderSize = player.playerCollider.size;
    }

    void Update()
    {
        if (Time.time > lastStaminaTime + staminaCooldown && currentStamina < maxStamina)
        {
            currentStamina += rechargeStamina * Time.deltaTime;

            if (currentStamina >= maxStamina)
            {
                currentStamina = maxStamina;
                isExhausted = false; //stamina cheia novamente
            }
        }

        OnDash();

        if (wallJumpLockCounter > 0)
        {
            wallJumpLockCounter -= Time.deltaTime;
        }

        HandleWallSlide();
        HandleImpulse();
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
    }

    #region Move
    void PhysicsCheck()
    {
        if (!player.isDead)
        {
            player.isGrounded = false;

            //dispara um raio para baixo de cada pé para checagem do chão
            RaycastHit2D leftFoot = Raycast(new Vector2(-footOffset, -groundOffset), Vector2.down, groundDistance, player._groundLayer);
            RaycastHit2D rightFoot = Raycast(new Vector2(footOffset, -groundOffset), Vector2.down, groundDistance, player._groundLayer);

            if ((leftFoot || rightFoot) && !bridgeHit && !player.onClimbing) //encostou no chão
            {
                player.isGrounded = true;
                player.playerInputs.pressParachute = false;
            }

            if (player.rb.velocity.y <= 0.0f) //queda
            {
                player.isJumping = false;
            }
        }
        else
        {
            player.rb.velocity = Vector2.zero;
        }

        ChangeGravity();
    }

    void ChangeGravity()
    {
        if (player.onWater)
            player.rb.gravityScale = waterGravity;
        else if (player.isDashing)
            player.rb.gravityScale = 0f;
        else
            player.rb.gravityScale = initialGravity;
    }
    #endregion

    void CheckMove()
    {
        float xVelocity = 0f;
        float yVelocity = 0f;
        float horizontal = player.playerInputs.GetHorizontal();

        if (player.isGrounded && !player.onSlope && !player.isGrabing && !player.isJumping && !player.onWater && !player.isHealing && !player.isDashing) //chão comum
        {
            xVelocity = speed * horizontal;
            yVelocity = 0.0f;
            player.rb.velocity = new Vector2(xVelocity, yVelocity);
        }
        else if (player.isGrounded && player.onSlope && !player.isGrabing && !player.isJumping && !player.onWater && !player.isHealing && !player.isDashing) //diagonal
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
        else if (!player.isGrounded && !player.onWater && wallJumpLockCounter <= 0f && !player.isDashing) //no ar
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

    #region Slope
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
        RaycastHit2D hit = Physics2D.Raycast(checkPos, Vector2.down, slopeCheckDistance, player._groundLayer);

        if (hit)
        {
            slopeNormalPerp = Vector2.Perpendicular(hit.normal).normalized;
            slopeDownAngle = Vector2.Angle(hit.normal, Vector2.up);

            if (slopeDownAngle != slopeDownAngleOld)
            {
                player.onSlope = true;
            }

            slopeDownAngleOld = slopeDownAngle;

            Debug.DrawRay(hit.point, slopeNormalPerp, Color.green);
            Debug.DrawRay(hit.point, hit.normal, Color.yellow);
        }

        if (player.onSlope && (player.playerInputs.horizontal == 0.0f || player.onSlope && player.playerInputs.pressAttack || player.isHealing))
        {
            player.rb.sharedMaterial = player._frictionMaterial;
        }
        else
        {
            player.rb.sharedMaterial = player._noFrictionMaterial;
        }
    }
    #endregion

    #region Jump
    void JumpControl()
    {
        if (player.isAttacking) return;

        // --- IMPULSE (DOUBLE JUMP) - tratar antes do pulo normal ---
        if (player.playerInputs.pressJump
            && !player.isGrounded
            && !player.onLedge
            && !player.isImpulsing
            && !player.onClimbing
            && !player.onWater
            && !player.isGriding
            && !player.isHealing
            && !player.isDashing
            && PlayerSkills.instance.skills.Contains(Skills.Impulse))
        {
            // executa o impulso (pulo duplo)
            player.playerInputs.pressJump = false;
            player.isImpulsing = true;
            player.isJumping = true;

            // garante impulso consistente
            player.rb.velocity = new Vector2(player.rb.velocity.x, 0f);
            player.rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);

            player.playerAudio.PlayJump();
            return; // opcional: evita que outras checagens interfiram neste frame
        }

        // --- PULO NORMAL ---
        if (player.playerInputs.pressJump && (player.isGrounded || ghostTime > Time.time) &&
            !player.isImpulsing && !player.onWater && player.playerInputs.vertical > -0.3f &&
            !player.onClimbing && !player.isHealing && !player.isDashing)
        {
            player.playerInputs.pressJump = false;
            player.isJumping = true;
            jumpHoldTimer = 0f;

            // garante que o pulo não seja anulado pela gravidade no mesmo frame
            player.isGrounded = false;

            // zera a velocidade vertical e aplica o impulso
            Vector2 vel = player.rb.velocity;
            vel.y = 0f;
            player.rb.velocity = vel;

            player.rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);

            ghostTime = Time.time;
            player.playerAudio.PlayJump();
            CreateDust(1);
        }

        // --- PULO VARIÁVEL (enquanto segura o botão) ---
        if (player.isJumping && player.playerInputs.holdJump && jumpHoldTimer < maxHoldTime)
        {
            jumpHoldTimer += Time.fixedDeltaTime;
            player.rb.AddForce(Vector2.up * holdJumpForce, ForceMode2D.Force);
        }

        // --- CORTE DO PULO (soltou o botão) ---
        if (!player.playerInputs.holdJump && player.isJumping && player.rb.velocity.y > 0f)
        {
            player.rb.velocity = new Vector2(player.rb.velocity.x, player.rb.velocity.y * 0.5f);
            player.isJumping = false;
        }

        // --- RESTANTE DOS CASOS (água, ponte, wall jump, ledge, climb/grid) ---
        else if (player.playerInputs.pressJump && !player.isGrounded && !player.onLedge && !player.isImpulsing &&
                 !player.onWater && !player.onClimbing && !player.isHealing && !player.isDashing && player.onBridge)
        {
            player.playerInputs.pressJump = false;
            player.isImpulsing = true;

            player.rb.velocity = Vector2.zero;
            player.rb.AddForce(new Vector2(player.rb.velocity.x, jumpForce - 5f), ForceMode2D.Impulse);
            player.playerAudio.PlayJump();
        }
        else if (player.playerInputs.pressJump && player.onWater && canSwin && !player.onLedge)
        {
            if (player.playerCollision.outWaterHit && player.playerCollision.inWaterHit)
            {
                player.playerInputs.pressJump = false;
                player.isJumping = true;

                player.rb.velocity = Vector2.zero;
                player.rb.AddForce(Vector2.up * jumpOutWater, ForceMode2D.Impulse);

                player.playerAudio.PlayJump();
                player.playerAudio.PlayWaterSplash();
            }
            else if (!player.playerCollision.outWaterHit && player.playerCollision.inWaterHit)
            {
                player.rb.velocity = Vector2.zero;
                player.rb.AddForce(Vector2.up * swimForce, ForceMode2D.Impulse);
                canSwin = false;
            }
        }
        else if (player.playerInputs.pressJump && player.playerInputs.vertical <= -0.3f && player.onBridge && !player.onClimbing)
        {
            PassThroughBridge();
        }
        else if (player.playerInputs.pressJump && player.isWallSliding)
        {
            player.playerInputs.pressJump = false;
            player.isJumping = true;

            wallJumpLockCounter = wallJumpLockTime;
            player.rb.velocity = Vector2.zero;
            player.rb.velocity = new Vector2(wallJumpForce.x * -wallDirection, wallJumpForce.y);

            player.isWallSliding = false;
            Flip();
        }
        else if (player.playerInputs.pressJump && player.onLedge && !player.isGrounded && !player.onWater)
        {
            player.isJumping = true;
            player.playerInputs.pressJump = false;
            player.rb.AddForce(new Vector2((jumpForce + 2f) * -playerDirection, jumpForce - 5f), ForceMode2D.Impulse);
        }
        else if (player.playerInputs.pressJump && (player.onClimbing || player.isGriding) &&
                 (TouchingVine() || TouchingLadder() || TouchingGrid()))
        {
            player.playerInputs.pressJump = false;
            player.isJumping = true;

            FinishClimb();
            FinishGrid();

            player.playerInputs.vertical = 0f;
            player.rb.velocity = Vector2.zero;
            player.rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            player.playerAudio.PlayJump();
        }

        // --- Resetar flag ao tocar o chão ---
        if (player.isGrounded && player.rb.velocity.y <= 0f)
            player.isJumping = false;
    }

    #endregion

    void BlockMove() //verifica se está no ar e tira a gravidade do player
    {
        if (player.isDead || !player.canMove)
            return;

        if ((player.playerInputs.pressAttack && player.isGrounded && !player.onWater) || player.isHealing) { player.rb.velocity = Vector2.zero; }
    }

    #region Wall Slide
    void HandleWallSlide()
    {
        bool canSlide =
        player.playerCollision.touchingWall &&
        PlayerSkills.instance.skills.Contains(Skills.Slide) &&
        //player.playerInputs.horizontal != 0f && //define se precisa apertar o botão horizontal para fazer o wall slide
        !player.isGrounded &&
        !player.onLedge &&
        !player.onClimbing &&
        !player.onHit &&
        player.rb.velocity.y < 0;

        if (canSlide)
        {
            player.isWallSliding = true;
            wallDirection = playerDirection; //wallDirection = direção da parede (oposta ao movimento)

            player.rb.velocity = new Vector2(player.rb.velocity.x, Mathf.Clamp(player.rb.velocity.y, -wallSlideSpeed, float.MaxValue)); //limita a velocidade de descida
        }
        else
        {
            player.isWallSliding = false;
        }
    }
    #endregion

    #region Impulse
    void HandleImpulse()
    {
        if (!player.isImpulsing) return;

        if (player.isGrounded || player.onLedge || player.onWater || player.onClimbing || player.isHealing || player.isDashing || player.isAttacking)
            player.isImpulsing = false;
    }
    #endregion

    #region Stamina
    public void StaminaConsumption(float consume)
    {
        lastStaminaTime = Time.time;

        if (currentStamina < consume)
        {
            currentStamina = 0f;
            isExhausted = true; //entra em exaustão
        }
        else
        {
            currentStamina -= consume;
        }
    }
    #endregion

    #region Dash

    void OnDash()
    {
        bool pressDash = player.playerInputs.pressDash;

        bool canDash = !isExhausted && currentStamina > 0f && dashCooldownTimer >= dashCooldown && PlayerSkills.instance.skills.Contains(Skills.Dash);

        if (pressDash && canDash)
            ExecuteDash();

        if (player.isDashing)
        {
            dashTimer += Time.deltaTime;
            if (dashTimer >= dashDuration)
                FinishDash();
        }

        dashCooldownTimer += Time.deltaTime;
    }

    private void ExecuteDash()
    {
        player.playerAnimations.AnimDash(); // cria uma animação separada se quiser
        player.isDashing = true;
        dashTimer = 0f;
        dashCooldownTimer = 0f;
        player.playerInputs.pressDash = false;

        StaminaConsumption(0.9f);

        Vector2 direction = playerDirection < 0 ? Vector2.left : Vector2.right;
        player.rb.velocity = direction * dashForce;
    }

    public void FinishDash()
    {
        player.isDashing = false;
        //player.rb.velocity = Vector2.zero; // cancela a velocidade no fim do dash
    }

    #endregion

    #region Water
    void OnWater()
    {
        if (player.isDead || !player.onWater)
            return;

        if (!canSwin)
            StartCoroutine(SwimControl());
    }

    public IEnumerator SwimControl()
    {
        yield return new WaitForSeconds(swinLimit);
        player.playerInputs.pressJump = false;
        canSwin = true;
    }
    #endregion

    #region BridgePass
    public void CheckBridge()
    {
        bridgeHit = false;
        RaycastHit2D _bridgeHitUp = RaycastBridge(transform.position, Vector2.up, bridgeCheckDistance, player._bridgeLayer);

        if (_bridgeHitUp)
            bridgeHit = true;
    }

    public void SetBridge(Bridge bridgeReference)
    {
        bridge = bridgeReference;
    }

    public void PassThroughBridge()
    {
        if (bridge != null)
            bridge.PassingThrough();
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
    #endregion

    #region Grid
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
        if (player.isDead || !player.canMove || player.isGrabing || player.playerInputs.pressHealing || player.isAttacking || player.isDashing) return;

        if (player.isGrounded)
            CreateDust(-1);

        player.playerMovement.playerDirection *= -1;

        Vector3 _scale = transform.localScale;
        _scale.x *= -1;
        transform.localScale = _scale;
    }

    public void CreateDust(int dir)
    {
        if (player.onWater) return;

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
