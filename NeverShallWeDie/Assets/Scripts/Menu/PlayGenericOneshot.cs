using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class PlayGenericOneshot : MonoBehaviour
{
    [SerializeField] EventReference _sound;

    public void PlaySoundEvent()
    {
        RuntimeManager.PlayOneShot(_sound);
    }
}
