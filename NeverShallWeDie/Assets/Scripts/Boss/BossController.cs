using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : MonoBehaviour
{
    public static BossController instance;

    public EnemyObject _enemyObject;
    public GameObject _itemDrop;
    public BossTrigger _bossTrigger;

    private string _enemyType;

    private int _maxHealth;
    [SerializeField] private int _currentHealth;
    private float _invincibleTime;

    private AudioClip _damageSound;
    private float _volume;
    private float _finishAttackTime;

    [SerializeField] private Vector3 _initialPos;
    [SerializeField] private Vector3 _initialScale;

    [HideInInspector] public float _direction = 1;
    [HideInInspector] public Animator _animation;
    [HideInInspector] public bool _isDead;
    [HideInInspector] public bool _onHit;

    void Awake()
    {
        instance = this;

        _initialPos = transform.position;
        _initialScale = transform.localScale;

        _animation = GetComponent<Animator>();
        
        _maxHealth = _enemyObject.maxHealth;
        _damageSound = _enemyObject.deadSound;
        _volume = _enemyObject.volume;
    }

    void Start()
    {
        ResetBoss();
    }

    void Update()
    {

    }

    public void TakeDamage(int damage)
    {
        if (_onHit)
            return;

        _onHit = true;
        _currentHealth -= damage;

        CinemachineShake.instance.ShakeCamera(3f, 0.15f); 

        //
        if (_currentHealth <= 0)
        {
            _isDead = true;
            _animation.SetTrigger("Dead");
            Invoke("DropItem", 2f);
        }

        if (!_isDead)
        {
            StartCoroutine(FinishHit());
        }
    }

    public void ResetBoss()
    {
        _animation.SetBool("Enabled", false);
        transform.position = _initialPos;
        transform.localScale = _initialScale;
        _direction = 1;        
        _currentHealth = _maxHealth;
        _bossTrigger.ResetIntro();
        _isDead = false;
        _onHit = false;
    }

    IEnumerator FinishHit()
    {
        yield return new WaitForSeconds(_invincibleTime);
        _onHit = false;
    }

    void DropItem() //dentro da fun��o TakeDamage
    {
        _itemDrop.SetActive(true);
    }
}
