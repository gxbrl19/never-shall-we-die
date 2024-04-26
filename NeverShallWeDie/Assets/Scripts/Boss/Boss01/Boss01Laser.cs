using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss01Laser : MonoBehaviour
{
    void Start()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == 8 || other.gameObject.layer == 9) //Ground ou Player
        {
            Destroy(gameObject);
        }
    }
}
