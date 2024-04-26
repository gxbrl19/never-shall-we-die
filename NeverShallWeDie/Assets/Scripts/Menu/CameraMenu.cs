using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMenu : MonoBehaviour
{
    public float _speed;

    private void Update()
    {
        transform.Translate(new Vector3(_speed * Time.deltaTime, 0f, 0f));
    }
}
