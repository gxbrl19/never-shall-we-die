using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class SawRotate : MonoBehaviour
{
    public float _speed;

    void Update()
    {
        //efeito de rotacao 
        transform.DORotate(new Vector3(0f, 0f, 360f), _speed, RotateMode.FastBeyond360)
        .SetLoops(-1, LoopType.Restart)
        .SetRelative()
        .SetEase(Ease.Linear);

    }
}
