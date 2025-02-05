using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuMusic : MonoBehaviour
{
    public static MenuMusic instance;

    public double time;

    public AudioSource[] audioSource;

    private void Start()
    {
        AudioClip introClip = audioSource[0].clip;

        // Obtém o tempo atual do sistema de áudio
        double startTime = AudioSettings.dspTime;

        // Toca o primeiro áudio imediatamente
        audioSource[0].PlayScheduled(startTime);

        // Calcular o início do segundo clipe com precisão
        double introDuration = (double)introClip.samples / introClip.frequency;
        double secondClipStartTime = startTime + introDuration;

        // Agenda o segundo áudio para tocar logo após o primeiro
        audioSource[1].PlayScheduled(secondClipStartTime);
    }
}
