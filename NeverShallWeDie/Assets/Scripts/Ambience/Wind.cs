using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wind : MonoBehaviour
{
    private bool _playerIn = false;
    private BuoyancyEffector2D _effect;    
    private PlayerInputs _input;

    private void Awake() {        
        _input = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInputs>();
        _effect = GetComponent<BuoyancyEffector2D>();
    }

    private void Start() {
        _effect.enabled = false;
    }

    private void Update() {
        if (_playerIn) { 
            if (_input.isParachuting) {
                _effect.enabled = true;
            }
            else {
                _effect.enabled = false;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.tag == "Player") {
            _playerIn = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if (other.tag == "Player") {
            _playerIn = false;
            _effect.enabled = false;
        }
    }
}
