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
            //MusicControl(0); //TODO: remover depois da DEMO
            MusicControl(6); //descomentar depois da DEMO (Kingdom)
        }
        else
        {
            if (scene == 4) { MusicControl(0); } //ship
            else if (scene == 8 || scene == 9 || scene == 10) { MusicControl(1); } //forest
            else if (scene == 62 || scene == 63 || scene == 64) { MusicControl(2); } //mizuton
            else if (scene == 122 || scene == 123 || scene == 124) { MusicControl(3); } //winter
            else if (scene == 180 || scene == 181 || scene == 182) { MusicControl(4); } //prison
            //else if (scene == 8 || scene == 9 || scene == 10) { MusicControl(5); } //mansion TODO: passar quando inserir a ultima ilha
            //else if (scene == 8 || scene == 9 || scene == 10) { MusicControl(6); } //kingdom TODO: passar quando inserir a ultima ilha
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
        //verifica se já existe o save _lastPier com o nome da cena do Cais selecionado
        string lastPier = GameManager.instance._lastPier != "" ? GameManager.instance._lastPier : "06/H0";

        switch (lastPier)
        {
            case "06/H0":
                MusicControl(6);
                break;
            case "01/H1":
                MusicControl(1);
                break;
            case "01/H2":
                MusicControl(1);
                break;
            case "02/H3":
                MusicControl(2);
                break;
            case "02/H4":
                MusicControl(2);
                break;
            case "03/H5":
                MusicControl(3);
                break;
            case "03/H6":
                MusicControl(3);
                break;
            case "04/H7":
                MusicControl(4);
                break;
        }
    }
}
