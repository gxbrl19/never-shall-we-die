using UnityEngine;

public class Crawler : EnemyBase
{
    private enum State { Move }
    private State currentState = State.Move;

    [Header("Crawler Settings")]
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Transform checkPoint;
    [SerializeField] private float speed = 1f;
    [SerializeField] private float flipCooldown = 0.25f;

    private Vector2 raycastDirection;
    private float nextFlipTime;
    private int directionIndex = 0; //0: Down, 1: Left, 2: Up, 3: Right

    protected override void Awake()
    {
        base.Awake();
    }

    private void Start()
    {
        SetDirectionVector();
    }

    protected override void Update()
    {
        if (isDead) return;

        if (currentState == State.Move)
        {
            SetDirectionVector();

            //checando a beirada
            bool hasGround = Physics2D.Raycast(checkPoint.position, raycastDirection, 1f, groundLayer);
            Debug.DrawRay(checkPoint.position, raycastDirection * 1f, Color.red);

            if (!hasGround && Time.time > nextFlipTime)
                RotateCrawler();
        }
    }

    private void FixedUpdate()
    {
        if (isDead) return;

        if (!isHurt && currentState == State.Move)
            rb.velocity = transform.right * speed;
        else
            rb.velocity = Vector2.zero;
    }

    private void RotateCrawler()
    {
        nextFlipTime = Time.time + flipCooldown;
        transform.Rotate(0, 0, -90);
        directionIndex = (directionIndex + 1) % 4;
    }

    private void SetDirectionVector()
    {
        switch (directionIndex)
        {
            case 0: raycastDirection = Vector2.down; break;
            case 1: raycastDirection = Vector2.left; break;
            case 2: raycastDirection = Vector2.up; break;
            case 3: raycastDirection = Vector2.right; break;
        }
    }
}
