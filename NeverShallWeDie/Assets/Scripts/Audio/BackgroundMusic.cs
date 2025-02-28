using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMusic : MonoBehaviour
{
    public static BackgroundMusic instance;

    public GameObject[] _musicObjects;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    public void MusicInCheckpoint(int scene)
    {
        if (scene == 0)
        {
            MusicControl(0); //TODO: remover depois da DEMO
            //MusicControl(6); //descomentar depois da DEMO (Kingdom)
        }
        else
        {
            //TODO: verificar a ilha que está selecionada para passar a musica
            if (scene == 4) { MusicControl(0); } //ship
            else if (scene == 8 || scene == 9 || scene == 10) { MusicControl(1); } //forest
        }
    }

    public void MusicControl(int index)
    {
        for (int i = 0; i < _musicObjects.Length; i++)
        {
            _musicObjects[i].SetActive(false);
        }

        if (index == -1) { return; } //-1 para todas as musicas
        _musicObjects[index].SetActive(true);
    }

    public void FinishBoss()
    {
        MusicControl(8);
    }

    public void BackToMapMusic() //método para retornar a música após vencer o Boss
    {
        MusicControl(1); //TODO: pegar a musica da ilha atual
    }
}
