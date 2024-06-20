using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deadshot : MonoBehaviour
{

    float _raycastSize = 15f;
    bool _canAttack = true;
    bool _detectPlayer = false;
    bool _attacking = false;
    EnemyController _controller;
    [SerializeField] Transform _shootPoint;
    [SerializeField] GameObject _projectile;
    [SerializeField] LayerMask _playerLayer;

    void Awake()
    {
        _controller = GetComponent<EnemyController>();
    }

    void Update()
    {
        if (_canAttack && _detectPlayer && !_attacking)
        {
            _attacking = true;
            _canAttack = false;
            _controller._animation.SetBool("Attack", true);
        }
    }

    void FixedUpdate()
    {
        DetectPlayer();
    }

    void DetectPlayer()
    {
        if (_controller._isDead)
            return;

        Vector2 _raycastDirection = Vector2.right * transform.localScale.x;
        Vector2 _raycastPosition = new Vector2(transform.position.x, transform.position.y + 0.5f);
        RaycastHit2D _hit = Physics2D.Raycast(_raycastPosition, _raycastDirection, _raycastSize, _playerLayer);

        if (_hit)
        {
            _detectPlayer = true;
        }
        else
        {
            _detectPlayer = false;
        }
    }

    public void Attack() //chamado na animacao
    {
        GameObject proj = Instantiate(_projectile.gameObject, _shootPoint.position, Quaternion.identity);
        float dir = transform.localScale.x;
        proj.GetComponent<Deadshot_Projectile>()._direction = dir;
    }

    public void FinishAttack() //chamado na animacao
    {
        _attacking = false;
        _controller._animation.SetBool("Attack", false);
    }

    public void FinishReload() //chamado na animacao
    {
        _canAttack = true;
    }

    private void OnDrawGizmos()
    {
        Vector2 _raycastDirection = Vector2.right * transform.localScale.x;
        Vector2 _raycastPosition = new Vector2(transform.position.x, transform.position.y + 0.5f);
        Debug.DrawRay(_raycastPosition, _raycastDirection * _raycastSize, Color.red);
    }

    public void PlaySound(AudioClip audio) //chamado na animação
    {
        _controller._audio.PlayOneShot(audio);
    }
}
