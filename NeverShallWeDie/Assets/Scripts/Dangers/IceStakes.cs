using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceStakes : MonoBehaviour
{
    //[SerializeField] private AudioClip _soundCollision;
    [SerializeField] private Rigidbody2D _bodyStake;

    Collider2D _collider;

    void Start()
    {
        _collider = GetComponent<Collider2D>();
    }

    private void OnTriggerEnter2D(Collider2D _other)
    {
        if (_other.gameObject.tag == "Player" || _other.gameObject.tag == "AttackPoint")
        {
            _bodyStake.bodyType = RigidbodyType2D.Dynamic;
            Destroy(_collider);
        }
    }
}
