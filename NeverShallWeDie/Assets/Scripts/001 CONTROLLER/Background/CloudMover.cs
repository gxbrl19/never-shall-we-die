using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudMover : MonoBehaviour
{
    public float speed = 0.5f;
    float resetX = 81f; // Posição onde some
    float startX = -8f;  // Posição onde reaparece

    void Update()
    {
        if (transform.position.x > resetX)
        {
            Vector3 newPos = transform.position;
            newPos.x = startX;
            this.transform.position = newPos;
        }

        this.transform.position += Vector3.right * speed * Time.deltaTime;
    }
}
