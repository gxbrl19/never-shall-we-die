using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using FMODUnity;

public class AncientStone : MonoBehaviour
{
    public int _idStone;
    [SerializeField] EventReference _enabledSound;
    [SerializeField] EventReference _thunderSound;
    bool _playerTriggered;
    Animator _animation;
    Collider2D _collider;
    Player _player;
    PlayerInputs _inputs;
    AncientStoneDaily _daily;

    void Awake()
    {
        _animation = GetComponent<Animator>();
        _collider = GetComponent<Collider2D>();
        _daily = GetComponentInChildren<AncientStoneDaily>();
        _player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        _inputs = _player.GetComponent<PlayerInputs>();
    }

    void Start()
    {
        int _enabled = GameManager.instance._ancientStones[_idStone];
        if (_enabled == 1) { _animation.SetBool("Enabled", true); }
    }

    void Update()
    {
        if (_playerTriggered && _inputs.pressInteract)
        {
            _inputs.pressInteract = false;
            _collider.enabled = false;
            GameManager.instance._ancientStones[_idStone] = 1;
            _animation.SetBool("Starting", true);
            _animation.SetBool("Enabled", true);
        }
    }

    public void StartText() //chamado no fim da animação
    {
        UIManager.instance.OpenAncientStone();
        _daily.Next();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            _playerTriggered = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            _playerTriggered = false;
        }
    }

    public void PlayEnabled() //chamado na animação
    {
        RuntimeManager.PlayOneShot(_enabledSound);
    }

    public void PlayThunder() //chamado na animação
    {
        RuntimeManager.PlayOneShot(_thunderSound);
    }
}
