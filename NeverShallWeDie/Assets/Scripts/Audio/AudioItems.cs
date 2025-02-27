using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class AudioItems : MonoBehaviour
{
    public static AudioItems instance;

    [Header("FMOD Events")]
    [SerializeField] EventReference newSkill;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    public void PlayNewSkill()
    {
        RuntimeManager.PlayOneShot(newSkill);
    }
}
