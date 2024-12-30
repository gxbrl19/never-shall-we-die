using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BossTrigger : MonoBehaviour
{
    [SerializeField] BossController _bossController;
    [SerializeField] BossDoor _bossDoor;
    [SerializeField] BossDoor _bossDoor2;

    Collider2D _collider;
    Player _player;

    private void Awake()
    {
        _collider = GetComponent<Collider2D>();
        _player = FindObjectOfType<Player>();
    }

    void Start()
    {
        if (GameManager.instance._bosses[_bossController._bossID] == 1)
        {
            _collider.enabled = false;
            _bossController.gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == 9)//Player
        {
            _bossController.GetComponent<Animator>().SetBool("Intro", true);
            _bossDoor._tiggered = true;
            _bossDoor2._tiggered = true;
            _collider.enabled = false;
            _player.DisableControls();

            UIManager.instance.BossEnabled();
            _bossController.EnabledUI();
            BackgroundMusic.instance.BossMusic();
        }
    }
}
