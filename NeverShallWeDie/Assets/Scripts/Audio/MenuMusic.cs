using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuMusic : MonoBehaviour
{
    public static MenuMusic instance;

    public AudioSource[] audioSource;
    public AudioClip audioClip;

    private void Start()
    {
        AudioClip introClip = audioSource[0].clip;

        audioSource[1].clip = audioClip;
        audioSource[1].Play();
        audioSource[1].Stop();

        // Pegar o tempo DSP atual
        double startTime = AudioSettings.dspTime;

        // Agendar a intro para começar imediatamente
        audioSource[0].PlayScheduled(startTime);

        // Garantir um cálculo preciso do tempo de transição
        double introDuration = introClip.length; // Usar a duração exata do áudio
        double secondClipStartTime = startTime + introDuration;

        // Agenda o segundo áudio para tocar logo após o primeiro
        audioSource[1].PlayScheduled(secondClipStartTime);

        Debug.Log($"Music: {introClip.name}, Intro Duration: {introDuration}, Scheduled Start: {secondClipStartTime}");
    }
}
