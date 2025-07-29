using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateMechanism : MonoBehaviour
{
    bool _triggered;
    [SerializeField] GateBoss _gateBoss;
    Animator _animation;
    SpriteRenderer _sprite;
    PlayerInputs _input;

    void Awake()
    {
        _sprite = GetComponent<SpriteRenderer>();
        _animation = GetComponent<Animator>();
        _input = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInputs>();
    }

    private void Update()
    {
        if (_triggered && _input.pressInteract)
        {
            if (GameManager.instance._gateMechanism == 0)
            {
                Debug.Log("Parece estar quebrado");
                //TODO: mensagem que os sistema está quebrado
                _input.pressInteract = false;
                return;
            }
            if (GameManager.instance._gateMechanism == 1 && GameManager.instance._gateBoss == 0)
            {
                GameManager.instance._gateBoss = 1;
                _animation.SetBool("Ok", true); //conserta o mecanismo
                _input.pressInteract = false;
                return;
            }
            if (GameManager.instance._gateMechanism == 1 && GameManager.instance._gateBoss == 1)
            {
                _gateBoss.EnabledGate(); //abre o portão
                _input.pressInteract = false;
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player") || other.gameObject.layer == LayerMask.NameToLayer("Invencible"))
        {
            _triggered = true;
        }
    }
}
