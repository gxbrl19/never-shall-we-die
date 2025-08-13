using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Impulse : MonoBehaviour
{
    void Awake()
    {
        Destroy(gameObject, 6f);
    }
}
