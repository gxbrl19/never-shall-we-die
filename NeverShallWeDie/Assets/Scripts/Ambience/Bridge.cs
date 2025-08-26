using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bridge : MonoBehaviour
{
    Collider2D coll;

    void Awake()
    {
        coll = GetComponent<Collider2D>();
    }

    public void PassingThrough()
    {
        coll.enabled = false;
        Invoke("SetDefaultLayer", 0.5f);
    }

    void SetDefaultLayer()
    {
        coll.enabled = true;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        Player _player = other.gameObject.GetComponent<Player>();

        if (_player != null)
        {
            _player.playerMovement.SetBridge(this);
            _player.onBridge = true;
        }
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        Player _player = other.gameObject.GetComponent<Player>();

        if (_player != null)
        {
            _player.onBridge = false;
        }
    }
}
