using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AirCut_Block : MonoBehaviour
{
    public int _id;

    Animator _animation;
    
    private void Awake() {
        _animation = GetComponent<Animator>();
    }

    private void Start() {
        if (GameManager.instance._airCutblock[_id] == 1) {
            _animation.SetBool("Disable", true);
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.layer == 28) { //AirCut
            _animation.SetBool("Destroy", true);
            GameManager.instance._airCutblock[_id] = 1;
        }
    }
}