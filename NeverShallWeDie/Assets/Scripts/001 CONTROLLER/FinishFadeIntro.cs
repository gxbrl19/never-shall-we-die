using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class FinishFadeIntro : MonoBehaviour
{
    public RectTransform _textZone;
    public RectTransform _textMap;

    public RectTransform _centerPoint;
    public RectTransform _basePoint1;
    public RectTransform _basePoint2;

    private Player _player;    

    void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }

    public void StartIntro()
    {
        _textZone.DOMoveX(_centerPoint.position.x, 0.7f).SetEase(Ease.InOutSine);
        _textMap.DOMoveX(_centerPoint.position.x, 0.7f).SetEase(Ease.InOutSine);
    }

    public void EnabledControlAfterFade()
    {
        _player.EnabledControls();
        _textZone.DOMoveX(_basePoint1.position.x, 0.5f).SetEase(Ease.InOutSine);
        _textMap.DOMoveX(_basePoint2.position.x, 0.5f).SetEase(Ease.InOutSine);
    }
}
