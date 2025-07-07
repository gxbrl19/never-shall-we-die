using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;
using FMODUnity;

public class Chest : MonoBehaviour
{
    public int _idChest;

    Animator _animation;

    [Header("FMOD Events")]
    [SerializeField] EventReference openChest;

    void Start()
    {
        _animation = GetComponent<Animator>();
        if (GameManager.instance._chests[_idChest] == 1) { DisableChest(); }
    }

    void Hited()
    {
        _animation.SetBool("Hited", true);
        GameManager.instance._chests[_idChest] = 1;
    }

    public void DisableChest()
    {
        _animation.SetBool("Disabled", true);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "SwordAtk")
        {
            Hited();
            RuntimeManager.PlayOneShot(openChest);
        }
    }
}
