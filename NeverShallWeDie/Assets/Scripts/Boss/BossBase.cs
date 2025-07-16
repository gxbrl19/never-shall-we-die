using UnityEngine;
using FMODUnity;

[RequireComponent(typeof(Rigidbody2D))]
public abstract class BossBase : MonoBehaviour
{
    [Header("Stats")]
    public BossObject bossObject;
    [HideInInspector] public int bossId;
    protected Color damageColor;

    protected float currentHealth;
    protected bool isDead = false;
    protected bool onHit = false;

    protected Rigidbody2D rb;
    protected Animator animator;
    protected SpriteRenderer spriteRenderer;
    Color defaultColor;

    [SerializeField] BossDoor bossDoor;
    [SerializeField] BossDoor bossDoor2;
    [SerializeField] WantedBoss wantedBoss;

    [Header("FMOD Events")]
    [SerializeField] EventReference hit;
    [SerializeField] EventReference dead;

    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        currentHealth = bossObject.maxHealth;
        defaultColor = spriteRenderer.color;
    }

    protected virtual void Update()
    {
        if (isDead) return;

        //barra de vida
        UIManager.instance._healthBoss.fillAmount = currentHealth / bossObject.maxHealth;
    }

    public virtual void TakeDamage(float damage)
    {
        if (isDead || onHit) return;

        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            Die();
        }
        else
        {
            OnHit();
        }
    }

    protected virtual void OnHit()
    {
        onHit = true;
    }

    protected virtual void Die()
    {
        isDead = true;
        rb.velocity = Vector2.zero;
        OnDeath();
    }

    protected virtual void OnDeath()
    {
        if (isDead) { return; }

        isDead = true;
        animator.SetTrigger("Dead");
        GameManager.instance._bosses[bossId] = 1;
        bossDoor._tiggered = false;
        bossDoor2._tiggered = false;
        UIManager.instance.BossDisabled();
        PlayDead();

        //da um pouco de XP ao player
        PlayerLevel _playerLevel = FindFirstObjectByType<PlayerLevel>();
        _playerLevel.GainXP(5);
    }

    protected virtual void ActivateBossUI()
    {
        UIManager.instance.BossEnabled();
        UIManager.instance._txtBossName.text = bossObject.name;
    }

    protected virtual void FinishHit() //chamado no TakeDamage()
    {
        onHit = false;
        spriteRenderer.color = defaultColor;
    }

    public virtual void SetWanted() //chamado na animação de morte
    {
        if (wantedBoss == null)
            return;

        wantedBoss.StartWanted();
    }

    protected virtual void PlayHit()
    {
        RuntimeManager.PlayOneShot(hit);
    }

    protected virtual void PlayDead()
    {
        RuntimeManager.PlayOneShot(dead);
    }

    public BossObject GetData() => bossObject;
}
