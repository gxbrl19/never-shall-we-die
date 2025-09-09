using UnityEngine;

public class VoidcallerOctopus : EnemyBase
{
    private enum State { Idle }
    private State currentState = State.Idle;

    [Header("Stats")]
    [HideInInspector] public int direction;
    Transform player;

    private void Start()
    {
        if (player == null)
            player = GameObject.FindGameObjectWithTag("Player")?.transform;

        transform.localScale = new Vector2(direction, 1f);
    }

    private void Update()
    {

    }

    private void ChangeState(State newState)
    {
        if (currentState == newState) return;

        currentState = newState;
    }
}
