using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WantedBoss : MonoBehaviour
{
    [SerializeField] Text _txtName;
    [SerializeField] Image _imgBoss;
    [HideInInspector] public string _bossName;
    [HideInInspector] public Sprite _bossImage;

    Animator _animation;
    Player _player;

    void Awake()
    {
        _player = GetComponent<Player>();
        _animation = GetComponent<Animator>();
    }

    void Update()
    {
        //Verificar o Botão do Player
    }

    public void StartWanted()
    {
        _animation.SetBool("Finish", true);
        _player.DisableControls();
    }

    public void FinishWanted() //chamado na animação
    {
        _txtName.text = _bossName;
        _imgBoss.sprite = _bossImage;
    }

    void DisableWanted()
    {

    }
}
