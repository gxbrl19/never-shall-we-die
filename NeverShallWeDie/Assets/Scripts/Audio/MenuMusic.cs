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
        // Obtém o tempo atual do sistema de áudio
        double startTime = AudioSettings.dspTime;

        // Toca o primeiro áudio imediatamente
        audioSource[0].PlayScheduled(startTime);

        // Calcula o tempo de início do segundo áudio
        double secondClipStartTime = startTime + audioSource[0].clip.length;

        // Agenda o segundo áudio para tocar logo após o primeiro
        audioSource[1].PlayScheduled(secondClipStartTime + time);
    }
}
