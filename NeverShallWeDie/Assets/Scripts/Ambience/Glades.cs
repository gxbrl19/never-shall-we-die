using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Glades : MonoBehaviour
{
    Collider2D _collider;
    Animator _animation;
    private void Awake()
    {
        _collider = GetComponent<Collider2D>();
        _animation = GetComponent<Animator>();
    }

    void Update()
    {
        if (GameManager.instance._navigator == "KNOWING")
        {
            gameObject.SetActive(true);
            _collider.enabled = false;
        }
        else if (GameManager.instance._navigator == "QUEST")
        {
            gameObject.SetActive(true);
            _collider.enabled = true;
        }
        else if (GameManager.instance._navigator == "COMPLETE" || GameManager.instance._navigator == "CREW")
        {
            gameObject.SetActive(false);
            _collider.enabled = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "SwordAtk")
        {
            _animation.SetTrigger("Break");
        }
    }

    public void SetNewState() //chamado na animação de break
    {
        GameManager.instance._navigator = "COMPLETE";
    }
}
