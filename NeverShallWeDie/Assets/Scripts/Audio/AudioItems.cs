using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioItems : MonoBehaviour {
    public static AudioItems instance;
    [SerializeField] private AudioSource _audioSource;

    [Header("Audio")]
    public AudioClip _goldSound;
    public AudioClip _powerPickupSound;
    public AudioClip _hitSound;

    [Header ("Volume")]
    public float _powerVolume;
    public float _goldVolume;    
    public float _hitVolume;

    private void Awake() {
        if (instance == null) {
            instance = this;
        }
    }
    
    public void PlaySound(AudioClip sound, float volume) {
        _audioSource.volume = volume;
        _audioSource.PlayOneShot(sound);
    }
}
