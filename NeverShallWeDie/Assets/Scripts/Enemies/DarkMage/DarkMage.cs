using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class DarkMage : EnemyBase
{
    enum State { Idle, Shoot }
    State currentState = State.Idle;

    [Header("Comportamento")]
    float raycastSize = 12f;
    float shootCooldown = .5f;
    float shootTimer = 0f;
    bool detectPlayer = false;
    private float direction;

    [Header("Referências")]
    [SerializeField] GameObject projectilePrefab;
    [SerializeField] LayerMask playerLayer;
    [SerializeField] Transform shootPoint;
    Transform player;

    [Header("FMOD Events")]
    [SerializeField] EventReference shootSound;

    protected override void Awake()
    {
        base.Awake();
    }

    void Start()
    {
        player = FindObjectOfType<Player>().GetComponent<Transform>();
    }

    private void Update()
    {
        if (isDead || isHurt || player == null) return;

        DetectPlayer();
        shootTimer += Time.deltaTime;

        switch (currentState)
        {
            case State.Idle:
                if (detectPlayer && shootTimer >= shootCooldown)
                    ChangeState(State.Shoot);

                break;

            case State.Shoot:
                animator.SetBool("Attack", true);

                break;
        }

        FlipTowardsPlayer();
    }

    /*void DetectPlayer()
    {
        Vector2 raycastDirection = Vector2.right * transform.localScale.x;
        Vector2 raycastPosition = new Vector2(transform.position.x, transform.position.y - 1f);
        RaycastHit2D hit = Physics2D.Raycast(raycastPosition, raycastDirection, raycastSize, playerLayer);
        detectPlayer = hit;
    }*/

    void DetectPlayer()
    {
        Vector2 raycastDirection = Vector2.right * transform.localScale.x;
        Vector2 raycastPosition = new Vector2(transform.position.x, transform.position.y - 1f);

        int combinedMask = playerLayer | LayerMask.GetMask("Ground");

        RaycastHit2D[] hits = Physics2D.RaycastAll(raycastPosition, raycastDirection, raycastSize, combinedMask);

        detectPlayer = false;

        foreach (var hit in hits)
        {
            if (hit.collider.CompareTag("Ground"))
            {
                detectPlayer = false; //parede detectada antes do player, bloqueia
                return;
            }
            else if (hit.collider.CompareTag("Player"))
            {
                detectPlayer = true; //player detectado sem nada na frente
                return;
            }
        }
    }

    private void ChangeState(State newState)
    {
        if (isDead) return;

        currentState = newState;
    }

    public void Attack() //chamado na animacao
    {
        GameObject proj = Instantiate(projectilePrefab.gameObject, shootPoint.position, Quaternion.identity);
        float dir = transform.localScale.x;
        proj.GetComponent<DarkMage_Projectile>().direction = dir;
        RuntimeManager.PlayOneShot(shootSound);
    }

    public void FinishAttack() //chamado na animacao
    {
        ChangeState(State.Idle);
        animator.SetBool("Attack", false);
        shootTimer = 0f;
    }

    private void OnDrawGizmos()
    {
        Vector2 raycastDirection = Vector2.right * transform.localScale.x;
        Vector2 raycastPosition = new Vector2(transform.position.x, transform.position.y - 1f);
        Debug.DrawRay(raycastPosition, raycastDirection * raycastSize, Color.red);
    }

    private void FlipTowardsPlayer()
    {
        if (player == null) return;

        float dir = player.position.x - transform.position.x;
        if (dir != 0)
            direction = Mathf.Sign(dir);

        transform.localScale = new Vector3(direction, 1, 1);
    }
}
