using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AncestralStone : MonoBehaviour {
    public int _idStone;
    private bool _enabled;
    private bool _triggered;
    private Animator _animation;    
    private PlayerInputs _input;

    void Start() {
        _animation = GetComponent<Animator>();
        _input = FindFirstObjectByType<PlayerInputs>();

        if (GameManager.instance._stones[_idStone] == 1){
            _enabled = true;
        }
        else {
            _enabled = false;
        }
    }
    
    void Update() {
        if (!_enabled && _triggered && _input.vertical > 0) {            
            _enabled = true;
            _animation.SetBool("Triggered", true);     
            GameManager.instance._stones[_idStone] = 1;
        }
        else if(_enabled) {
            _animation.SetTrigger("Enabled");
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player")) {
            _triggered = true; 
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if (other.CompareTag("Player")) {
            _triggered = false;
        }
    }
}
