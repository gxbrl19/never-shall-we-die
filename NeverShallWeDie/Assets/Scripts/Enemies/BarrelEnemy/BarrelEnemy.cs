using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrelEnemy : MonoBehaviour
{    
    public LayerMask _playerLayer;
    public float _speed; 
    public float _raycastSize;
    public Transform _finishPoint;

    public bool _detectPlayer = false;
    public bool _beginRoll = false;
    public bool _isRolling = false;
    EnemyController _controller;

    void Awake() {
        _controller = GetComponent<EnemyController>();
    }

    void Update() {
        if (_controller._isDead)
            return;
        
        if (_detectPlayer) {  _controller._animation.SetBool("DetectPlayer", true); }
        if (_detectPlayer && !_beginRoll) {                
            _isRolling = true;
            transform.position = Vector2.MoveTowards(transform.position, new Vector2(_finishPoint.position.x, transform.position.y), _speed * Time.deltaTime);
        }  
    }

    void FixedUpdate() {
        DetectPlayer();   
    }

    public void FinishBeginRoll() { //chamado na animação de BeginRoll
        _beginRoll = false;
        _controller._animation.SetBool("DetectPlayer", false);
    }

    void DetectPlayer() {
        if(_controller._isDead || _isRolling || _detectPlayer)
            return;

        Vector2 _raycastDirection;
        _raycastDirection = Vector2.right * transform.localScale.x;     
        RaycastHit2D _hit = Physics2D.Raycast(transform.position, _raycastDirection, _raycastSize, _playerLayer);
        if (_hit) { 
            _beginRoll = true;
            _detectPlayer = true; 
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.layer == 8 && _isRolling) { //no ground só zera a vida
            _isRolling = false;
            _detectPlayer = false;     
            _controller._currentHealth = 0;  
            _controller._animation.SetBool("Dead", true);     
        }   
        else if (other.gameObject.layer == 16 && _isRolling) { //na katana faz o efeito de HIT
            _isRolling = false;
            _detectPlayer = false;    
            _controller.TakeDamage(1);
        }
    }

    private void OnDrawGizmos() {
        Vector2 _raycastDirection;
        _raycastDirection = Vector2.right * transform.localScale.x;  
        Debug.DrawRay(transform.position, _raycastDirection * _raycastSize, Color.red);
    }
}
