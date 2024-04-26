using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Torch : MonoBehaviour
{
    private Player _player;

    private void Awake()
    {
        _player = FindFirstObjectByType<Player>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        
    }
}
