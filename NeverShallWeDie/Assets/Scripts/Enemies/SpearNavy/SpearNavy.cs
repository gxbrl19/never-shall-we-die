using UnityEngine;

public class SpearNavy : MonoBehaviour
{
    [Header("Config")]
    public float visionRange = 10f;
    public float throwRange = 5f;
    public float moveSpeed = 2f;
    public float throwCooldown = 2f;

    [Header("Referências")]
    public GameObject spearPrefab;
    public Transform throwPoint;

    enum State { Idle, Chase, Throw }
    State currentState = State.Idle;
    float throwTimer = 0f;
    bool isThrowing = false;

    Rigidbody2D _body;
    EnemyController _controller;
    Transform _player;

    private void Awake()
    {
        _player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        _controller = GetComponent<EnemyController>();
        _body = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (_player == null) return;

        throwTimer += Time.deltaTime;

        float distance = Vector2.Distance(transform.position, _player.position);

        switch (currentState)
        {
            case State.Idle:
                _body.velocity = Vector2.zero;
                _controller._animation.Play("SpearNavy_Idle");

                if (distance <= visionRange)
                {
                    if (distance <= throwRange)
                        ChangeState(State.Throw);
                    else
                        ChangeState(State.Chase);
                }
                break;

            case State.Chase:
                _controller._animation.Play("SpearNavy_Walk");

                if (distance > visionRange)
                {
                    ChangeState(State.Idle);
                }
                else if (distance <= throwRange)
                {
                    ChangeState(State.Throw);
                }
                else
                {
                    Vector2 dir = (_player.position - transform.position).normalized;
                    _body.velocity = dir * moveSpeed;

                    if (dir.x != 0)
                        transform.localScale = new Vector3(Mathf.Sign(dir.x), 1, 1);
                }
                break;

            case State.Throw:
                _body.velocity = Vector2.zero;

                if (distance > throwRange)
                {
                    ChangeState(State.Chase);
                }
                else if (!isThrowing && throwTimer >= throwCooldown)
                {
                    _controller._animation.Play("SpearNavy_Throw");
                    isThrowing = true;
                    throwTimer = 0f;

                    //delay sincronizado com a animação
                    //Invoke(nameof(ThrowSpear), 0.5f);
                }
                break;
        }
    }

    private void ChangeState(State newState)
    {
        currentState = newState;
        isThrowing = false;
    }

    public void ThrowSpear() //chamado na animação
    {
        if (spearPrefab == null || throwPoint == null || _player == null) return;

        Vector2 dir = (_player.position - throwPoint.position).normalized;

        GameObject spear = Instantiate(spearPrefab, throwPoint.position, Quaternion.identity);
        spear.GetComponent<SpearProjectile>().SetDirection(dir);

        //flip
        if (dir.x != 0)
            transform.localScale = new Vector3(Mathf.Sign(dir.x), 1, 1);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, visionRange);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, throwRange);
    }
}
