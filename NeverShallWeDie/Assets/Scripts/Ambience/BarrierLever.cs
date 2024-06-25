using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrierLever : MonoBehaviour
{
    [SerializeField] int _id;

    [Header("Camera")]
    [SerializeField] Animator _animation;
    [SerializeField] GameObject _mainCamera;
    [SerializeField] GameObject _barrierCamera;

    [Header("Audio")]
    [SerializeField] AudioClip _moveSound;

    AudioSource _audio;
    Player _player;

    private void Awake()
    {
        _audio = GetComponent<AudioSource>();
        _player = FindObjectOfType<Player>();
    }

    private void Start()
    {
        //verifica se já foi acionado
        bool finish = GameManager.instance._barriersLever[_id] == 1;
        _animation.SetBool("Finish", finish);
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
        _animation.SetBool("Enabled", true);
        _audio.PlayOneShot(_moveSound);
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
