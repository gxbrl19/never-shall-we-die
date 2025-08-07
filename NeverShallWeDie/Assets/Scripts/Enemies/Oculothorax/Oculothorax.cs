using UnityEngine;

public class Oculothorax : EnemyBase
{
    [Header("Oculothorax Settings")]
    [SerializeField] private float flySpeed = 5f;
    [SerializeField] private float launchForce = 8f;
    [SerializeField] private float activationDelay = 1.5f;

    private Transform player;
    private bool isActivated = false;

    protected void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player")?.transform;

        // Fase 1: impulso vertical
        rb.velocity = Vector2.up * launchForce;

        // Fase 2: ativa perseguição após delay
        Invoke(nameof(ActivateChase), activationDelay);
    }

    private void ActivateChase()
    {
        isActivated = true;
    }

    protected void Update()
    {
        if (isActivated && player != null)
            FlyToPlayer();
    }

    private void FlyToPlayer()
    {
        Vector2 direction = (player.position - transform.position).normalized;
        rb.velocity = direction * flySpeed;
        FlipSprite(direction.x);
    }

    private void FlipSprite(float directionX)
    {
        if (directionX != 0)
        {
            Vector3 localScale = transform.localScale;
            localScale.x = Mathf.Sign(directionX) * Mathf.Abs(localScale.x);
            transform.localScale = localScale;
        }
    }
}
