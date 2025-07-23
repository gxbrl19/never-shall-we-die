using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bridge : MonoBehaviour
{
    private int _myLayer;

    public int _ignoreLayer;

    void Start()
    {
        _myLayer = gameObject.layer;
    }

    public void PassingThrough()
    {
        gameObject.layer = _ignoreLayer;
        Invoke("SetDefaultLayer", 0.5f);
    }

    void SetDefaultLayer()
    {
        gameObject.layer = _myLayer;
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
