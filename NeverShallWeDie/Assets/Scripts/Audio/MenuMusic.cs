using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuMusic : MonoBehaviour
{
    public static MenuMusic instance;

    public AudioClip _nswdIntro;
    public AudioClip _nswdTheme;

    AudioSource _audioSource;

    private void Awake()
    {
        _audioSource = gameObject.GetComponent<AudioSource>();
    }

    void Start()
    {
        if (_audioSource.clip == _nswdIntro)
        {
            _audioSource.loop = false;
            StartCoroutine(PlayLoop(_nswdIntro));
        }
        else
        {
            _audioSource.loop = true;
        }
    }

    private System.Collections.IEnumerator PlayLoop(AudioClip clip)
    {
        yield return new WaitForSeconds(clip.length);

        _audioSource.clip = _nswdTheme;
        _audioSource.loop = true;
        _audioSource.Play();
    }
}
