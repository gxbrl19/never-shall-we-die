using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BossTrigger : MonoBehaviour
{
    [SerializeField] BossBase bossBase;
    [SerializeField] private float introDelay = 0f;
    [SerializeField] BossDoor bossDoor;
    [SerializeField] BossDoor bossDoor2;

    private bool triggered = false;

    Collider2D col;
    Player player;

    private void Awake()
    {
        col = GetComponent<Collider2D>();
        player = FindObjectOfType<Player>();
    }

    void Start()
    {
        if (GameManager.instance._bosses[bossBase.bossId] == 1)
        {
            col.enabled = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (triggered) return;

        if (other.CompareTag("Player") && bossBase != null)
        {
            triggered = true;

            //toca a animação de intro, se houver
            Animator anim = bossBase.GetComponent<Animator>();
            if (anim != null && anim.HasState(0, Animator.StringToHash("Intro")))
            {
                anim.SetBool("Intro", true);
            }

            bossDoor._tiggered = true;
            bossDoor2._tiggered = true;
            player.DisableControls();

            //ativa o boss após o tempo da intro (ou imediatamente, se delay = 0)
            Invoke(nameof(ActivateBoss), introDelay);

            //desativa o trigger
            gameObject.SetActive(false);
            BackgroundMusic.instance.MusicControl(-1);
        }
    }

    private void ActivateBoss()
    {
        bossBase.SendMessage("StartIntro", SendMessageOptions.DontRequireReceiver);
    }
}
