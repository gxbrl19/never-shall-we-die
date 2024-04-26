using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using System.Numerics;
using System;

public class CinemachineShake : MonoBehaviour
{
    public static CinemachineShake instance { get; private set; }

    [SerializeField] bool _oneCamScene;
    [HideInInspector] public float _offset_x;
    [HideInInspector] public float _offset_y;
    private CinemachineVirtualCamera _cineVC;
    CinemachineFramingTransposer f_tranposer;
    private float _shakeTimer;

    private void Awake()
    {
        instance = this;
        _cineVC = GetComponent<CinemachineVirtualCamera>();
    }

    private void Start()
    {
        f_tranposer = _cineVC.GetCinemachineComponent<CinemachineFramingTransposer>();

        if (_oneCamScene)
        {
            //2.37037 21:9
            //1.333333 4:3
            //1.777778 16:9
            float _screenWidth = Screen.width;
            float _screenHeight = Screen.height;

            float _aspectRadio = _screenWidth / _screenHeight;

            if (_aspectRadio > 2f)
            {
                _cineVC.m_Lens.OrthographicSize = 6;
            }
            else
            {
                _cineVC.m_Lens.OrthographicSize = 8;
            }
        }
    }

    private void Update()
    {
        if (_shakeTimer > 0)
        {
            _shakeTimer -= Time.deltaTime;
            if (_shakeTimer <= 0f)
            {
                CinemachineBasicMultiChannelPerlin _cineBMCP = _cineVC.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

                _cineBMCP.m_AmplitudeGain = 0f;
            }
        }

        if (f_tranposer != null)
        {
            f_tranposer.m_TrackedObjectOffset.x = _offset_x;
            f_tranposer.m_TrackedObjectOffset.y = _offset_y;
        }
    }

    public void ShakeCamera(float intensity, float time)
    {
        CinemachineBasicMultiChannelPerlin _cineBMCP = _cineVC.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

        _cineBMCP.m_AmplitudeGain = intensity;
        _shakeTimer = time;
    }
}
