using UnityEngine;
using FMODUnity;

[RequireComponent(typeof(Rigidbody2D))]
public abstract class BossBase : MonoBehaviour, IBoss
{
    [Header("Stats")]
    public BossObject bossObject;
    [HideInInspector] public int bossId;

    protected float currentHealth;
    protected bool isDead = false;
    protected bool isHurt = false;

    protected Rigidbody2D rb;
    protected Animator animator;
    protected SpriteRenderer spriteRenderer;
    protected Color defaultColor;
    protected Color damageColor = Color.red;

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

        if (bossObject != null)
        {
            currentHealth = bossObject.maxHealth;
            defaultColor = spriteRenderer.color;
        }
        else
        {
            Debug.LogWarning($"{gameObject.name} está sem BossObject atribuído!");
            currentHealth = 3; //fallback
        }
    }

    protected virtual void Update()
    {
        if (isDead) return;

        //barra de vida
        UIManager.instance._healthBoss.fillAmount = currentHealth / bossObject.maxHealth;
    }

    public virtual void TakeHit(int power, Vector2 hitDirection, float knockbackForce = 5f)
    {
        if (isHurt || isDead) return;

        isHurt = true;
        Invoke("ResetHurt", .3f);

        TakeDamage(power);
    }

    public virtual void TakeDamage(int amount)
    {
        currentHealth -= amount;
        spriteRenderer.color = damageColor;
        CinemachineShake.instance.ShakeCamera(3f, 0.15f); //tremida na camera

        if (currentHealth <= 0)
        {
            OnDeath();
        }
        else
        {
            RuntimeManager.PlayOneShot(hit);
        }
    }

    protected virtual void OnDeath()
    {
        //if (isDead) { return; }

        isDead = true;
        rb.velocity = Vector2.zero;
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

    protected virtual void ResetHurt() //chamado no TakeHit()
    {
        spriteRenderer.color = defaultColor;
        isHurt = false;
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
