using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitEffect : MonoBehaviour
{
    void Start()
    {
        Destroy(gameObject, 0.3f);
    }    
}
