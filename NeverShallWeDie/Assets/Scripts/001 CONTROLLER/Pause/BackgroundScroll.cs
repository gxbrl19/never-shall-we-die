using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BackgroundScroll : MonoBehaviour
{
    [SerializeField] RawImage _image;
    [SerializeField] float _x, _y;

    void Update()
    {
        float deltaTime = Time.timeScale == 0 ? 1 : Time.deltaTime * (1f / Time.timeScale);
        _image.uvRect = new Rect(_image.uvRect.position + new Vector2(_x, _y) * deltaTime, _image.uvRect.size);
    }
}
