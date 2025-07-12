using UnityEngine;
using FMODUnity;

[RequireComponent(typeof(Rigidbody2D), typeof(Animator))]
public abstract class EnemyBase : MonoBehaviour, IEnemy
{
    [Header("Dados")]
    public EnemyObject enemyObject;
    protected string enemyName;
    protected float dropRate;
    protected GameObject dropPrefab;
    protected Color damageColor;
    [SerializeField] protected int currentHealth;
    [SerializeField] GameObject deathEffect;
    protected Rigidbody2D rb;
    protected Animator animator;
    Color defaultColor;
    protected bool isHurt = false;
    protected bool isDead = false;

    [Header("FMOD Events")]
    [SerializeField] EventReference hit;
    [SerializeField] EventReference death;

    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        if (enemyObject != null)
        {
            currentHealth = enemyObject.maxHealth;
            enemyName = enemyObject.name;
            dropRate = enemyObject.dropRate;
            dropPrefab = enemyObject.dropPrefab;
            damageColor = enemyObject.damageColor;
            defaultColor = GetComponent<SpriteRenderer>().color;
        }
        else
        {
            Debug.LogWarning($"{gameObject.name} está sem EnemyObject atribuído!");
            currentHealth = 3; // fallback
        }
    }

    protected virtual void Update()
    {
        // Corrige "flutuação" se estiver com velocidade vertical e sem gravidade
        if (Mathf.Abs(rb.velocity.y) > 0.1f)
        {
            rb.velocity = new Vector2(rb.velocity.x, 0f);
        }
    }

    public virtual void TakeHit(int power, Vector2 hitDirection, float knockbackForce = 5f)
    {
        if (isHurt || isDead) return;

        isHurt = true;
        rb.velocity = new Vector2(hitDirection.normalized.x * knockbackForce, 0f);

        TakeDamage(power);
    }

    public virtual void TakeDamage(int amount)
    {
        currentHealth -= amount;
        GetComponent<SpriteRenderer>().color = damageColor;
        CinemachineShake.instance.ShakeCamera(3f, 0.15f); //tremida na camera

        if (currentHealth <= 0)
        {
            Die();
        }
        else
        {
            RuntimeManager.PlayOneShot(hit);
            animator.SetTrigger("Hurt");
        }
    }

    protected virtual void Die()
    {
        if (isDead) return;

        isDead = true;
        rb.velocity = Vector2.zero;
        animator.SetTrigger("Dead");
        RuntimeManager.PlayOneShot(death);

        TryDropItem();

        Instantiate(deathEffect, transform.position, Quaternion.identity);

        PlayerHealth playerHealth = FindFirstObjectByType<PlayerHealth>();
        playerHealth.FillBottle(3f);

        PlayerLevel playerLevel = FindFirstObjectByType<PlayerLevel>();
        playerLevel.GainXP(5);
    }

    private void TryDropItem()
    {
        for (int i = 0; i < dropRate; i++)
        {
            if (dropPrefab != null)
            {
                Instantiate(dropPrefab, transform.position, Quaternion.identity);
            }
        }
    }

    protected virtual void ResetHurt()
    {
        isHurt = false;
        GetComponent<SpriteRenderer>().color = defaultColor;
    }
}
