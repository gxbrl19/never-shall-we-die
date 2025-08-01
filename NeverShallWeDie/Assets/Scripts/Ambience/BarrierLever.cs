using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class BarrierLever : MonoBehaviour
{
    [SerializeField] int _id;

    [HideInInspector] public bool _opened;

    [Header("Camera")]
    [SerializeField] Animator _animation;
    [SerializeField] GameObject _mainCamera;
    [SerializeField] GameObject _barrierCamera;

    [Header("FMOD Events")]
    [SerializeField] EventReference portalOpen;
    Player _player;

    private void Awake()
    {
        _player = FindObjectOfType<Player>();
    }

    private void Start()
    {
        //verifica se já foi acionado
        _opened = GameManager.instance._barriersLever[_id] == 1;
        _animation.SetBool("Finish", _opened);
    }

    public void EnabledCamera()
    {
        _player.DisableControls();
        _mainCamera.SetActive(false);
        _barrierCamera.SetActive(true);
        Invoke("EnabledBarrier", 1f);
    }

    void EnabledBarrier()
    {
        GameManager.instance._barriersLever[_id] = 1;
        _opened = true;
        _animation.SetBool("Enabled", true);
        RuntimeManager.PlayOneShot(portalOpen);
        Invoke("FinishEnabled", 2.02f);
    }

    public void FinishEnabled()
    {
        _player.EnabledControls();
        _animation.SetBool("Finish", true);
        _mainCamera.SetActive(true);
        _barrierCamera.SetActive(false);
    }
}
