using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoarMinion : MonoBehaviour
{
    [HideInInspector] public float minionDirection;
    Vector2 minionSpeed = new Vector2(8f, 0f);

    void Start()
    {
        transform.localScale = new Vector2(-minionDirection, 1f);
    }

    void FixedUpdate()
    {
        transform.Translate(minionSpeed * minionDirection * Time.deltaTime);
    }
}
