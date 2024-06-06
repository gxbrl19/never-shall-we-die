using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossTrigger : MonoBehaviour
{
    //public GameObject _bossObject;
    public BossDoor _bossDoor;
    public BossDoor _bossDoor2;

    Collider2D _collider;
    Player _player;

    private void Awake()
    {
        _collider = GetComponent<Collider2D>();
        _player = FindObjectOfType<Player>();
        //_audioSource = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == 9)//Player
        {
            //_bossObject.GetComponent<Animator>().SetBool("Enabled", true);
            //_bossObject.SetActive(true);
            _bossDoor._tiggered = true;
            _bossDoor2._tiggered = true;
            _collider.enabled = false;
            _player.DisableControls();
            Invoke("FinishIntro", 3.5f);
        }
    }

    public void FinishIntro() //invocado no OnTriggerEnter
    {
        //_backgroundMusic.Play();
        _player.EnabledControls();
    }
}
