using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRespawn : MonoBehaviour
{
    private Transform _playerTransform;


    void Start()
    {
        _playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        if (_playerTransform != null)
        {
            Vector3 _playerPos = transform.position;
            _playerPos.z = 0f;

            _playerTransform.position = _playerPos;
        }
    }
}
