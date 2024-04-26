using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossTrigger : MonoBehaviour
{
    public GameObject _bossObject;
    public BossDoor _bossDoor;
    public bool _intro;
    public GameObject _bossCamera;

    public AudioSource _backgroundMusic;
    private AudioSource _audioSource;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == 9 && !_intro)//Player
        {
            _bossObject.GetComponent<Animator>().SetBool("Enabled", true);
            _bossObject.SetActive(true);
            _intro = true;
            _bossDoor._tiggered = true;
            _backgroundMusic.Stop();
            _audioSource.Play();
            Invoke("RestartBackgroundMusic", 3.5f);
        }
    }

    public void RestartBackgroundMusic()
    {
        _backgroundMusic.Play();
    }

    public void ResetIntro()
    {
        _bossDoor._tiggered = false;
        _intro = false;
    }
}
