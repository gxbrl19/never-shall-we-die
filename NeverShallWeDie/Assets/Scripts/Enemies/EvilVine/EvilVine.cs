using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class EvilVine : EnemyBase
{
    enum State { Idle, Shoot }
    State currentState = State.Idle;

    [Header("Comportamento")]
    float raycastSize = 12f;
    float shootCooldown = .5f;
    float shootTimer = 0f;
    bool detectPlayer = false;

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

    protected override void Update()
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
    }

    void DetectPlayer()
    {
        Vector2 raycastDirection = Vector2.right * transform.localScale.x;
        Vector2 raycastPosition = new Vector2(transform.position.x, transform.position.y - 0.5f);
        RaycastHit2D hit = Physics2D.Raycast(raycastPosition, raycastDirection, raycastSize, playerLayer);
        detectPlayer = hit;
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
        proj.GetComponent<EvilVine_Projectile>().direction = dir;
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
        Vector2 raycastPosition = new Vector2(transform.position.x, transform.position.y - 0.5f);
        Debug.DrawRay(raycastPosition, raycastDirection * raycastSize, Color.red);
    }
}
