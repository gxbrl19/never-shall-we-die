using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class KingBoar : MonoBehaviour
{
    [Header("Spikes")]
    [SerializeField] Transform _shootPoint;
    [SerializeField] Transform _spike;
    float _launchForce = 1f;
    Vector2 _velocitySpike;

    [Header("BoarMinion")]
    [SerializeField] GameObject _boarMinion;
    [SerializeField] Transform _boarMinionLeft;
    [SerializeField] Transform _boarMinionRight;

    [Header("FMOD Events")]
    [SerializeField] EventReference grunt;
    [SerializeField] EventReference attack;
    [SerializeField] EventReference minethrow;
    [SerializeField] EventReference callbackup;

    float _speed = 2f;
    float _timer;
    float _timerMinions;
    int _numAttack;
    int _direction;
    bool _intro;
    bool _starter = false;
    bool _attacking = false;
    bool _walking = false;
    BossController _bossController;
    AudioSource _audioSource;
    Player _player;


    void Awake()
    {
        _bossController = GetComponent<BossController>();
        _audioSource = GetComponent<AudioSource>();
        _player = FindObjectOfType<Player>();
    }

    void Start()
    {
        _timer = 0f;
    }

    void Update()
    {
        if (_intro) //tremida na camera
        {
            CinemachineShake.instance.ShakeCamera(3f, 0.15f);
            return;
        }

        if (!_starter || _bossController._isDead || _player._dead) { return; }

        Flip();

        _walking = VerifyDistancePlayer();
        _bossController._animation.SetBool("Walk", _walking);

        //attack
        if (!_attacking) { _timer += Time.deltaTime; }
        if (_timer > 1.3f) { AttackController(); }

        _velocitySpike = (_player.transform.position - transform.position) * _launchForce;
        _timerMinions += Time.deltaTime;
    }

    private void FixedUpdate()
    {
        if (_walking && !_attacking)
        {
            transform.Translate(new Vector3(_speed * _direction, 0f, 0f) * Time.deltaTime);
        }
    }

    bool VerifyDistancePlayer()
    {
        float distance = Vector3.Distance(_player.transform.position, transform.position);
        return distance > 5; //se a distancia do Boss pro Player for maior que 5 habilita o Walk
    }

    #region Attacks
    void AttackController()
    {
        if (_attacking) { return; }
        //_numAttack = Random.Range((int)_range.x, (int)_range.y);

        _attacking = true;

        if (!_walking && _timerMinions < 10f) { _numAttack = 1; }
        else if (_walking && _timerMinions < 10f) { _numAttack = 2; }
        else if (_walking && _timerMinions >= 10f) { _numAttack = 3; }
        _bossController._animation.SetInteger("Index", _numAttack);
        _bossController._animation.SetBool("Attacking", true);
    }

    public void SpikeAttack() //chamado na animação Attack02
    {
        PlayMineThrow();

        Transform spike = Instantiate(_spike, _shootPoint.position, Quaternion.identity);
        spike.GetComponent<Rigidbody2D>().velocity = new Vector2(_velocitySpike.x + 4f, _velocitySpike.y - 0.5f);

        Transform spike2 = Instantiate(_spike, _shootPoint.position, Quaternion.identity);
        spike2.GetComponent<Rigidbody2D>().velocity = new Vector2(_velocitySpike.x, _velocitySpike.y - 0.5f);

        Transform spike3 = Instantiate(_spike, _shootPoint.position, Quaternion.identity);
        spike3.GetComponent<Rigidbody2D>().velocity = new Vector2(_velocitySpike.x - 4f, _velocitySpike.y - 0.5f);
    }

    public void MinionAttack() //chamado na animação Attack03
    {
        PlayCallBackup();

        if (_direction == 1)
        {
            GameObject minion = Instantiate(_boarMinion, _boarMinionLeft.position, Quaternion.identity);
            minion.GetComponent<BoarMinion>()._direction = _direction;
        }
        else
        {
            GameObject minion = Instantiate(_boarMinion, _boarMinionRight.position, Quaternion.identity);
            minion.GetComponent<BoarMinion>()._direction = _direction;
        }

        _timerMinions = 0f;
    }

    public void FinishAttack() //chamado nas animações
    {
        _attacking = false;
        _timer = 0f;
        _bossController._animation.SetBool("Attacking", false);
    }
    #endregion

    void Flip()
    {
        if (_bossController._isDead || _attacking) { return; }

        if (transform.position.x < _player.transform.position.x)
        {
            transform.localScale = new Vector2(1, 1);
            _direction = 1;
        }
        else if (transform.position.x > _player.transform.position.x)
        {
            transform.localScale = new Vector2(-1, 1);
            _direction = -1;
        }
    }

    public void Intro() //chamado na animação de Intro
    {
        _intro = true;
        PlayGrunt();
    }

    public void FinishIntro() //chamado na animação de Intro
    {
        _intro = false;
        _player.EnabledControls();
        _starter = true;
        BackgroundMusic.instance.MusicControl(7);
    }

    public void PlayGrunt()
    {
        RuntimeManager.PlayOneShot(grunt);
    }

    public void PlayAttack() //chamado na animação de ataque
    {
        RuntimeManager.PlayOneShot(attack);
    }

    public void PlayMineThrow()
    {
        RuntimeManager.PlayOneShot(minethrow);
    }

    public void PlayCallBackup()
    {
        RuntimeManager.PlayOneShot(callbackup);
    }
}
