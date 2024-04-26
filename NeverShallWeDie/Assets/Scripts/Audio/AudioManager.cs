using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private AudioSource _audioSource;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    public void PlayAudio(AudioClip clip)
    {
        _audioSource.clip = clip;
        _audioSource.Play();
    }

    public void PlayAudioAtPoint(Vector2 audioPosition, AudioClip clip)
    {
        Vector3 _newAudioPos = new Vector3(audioPosition.x, audioPosition.y, Camera.main.transform.position.z);
        AudioSource.PlayClipAtPoint(clip, _newAudioPos);
    }
}
