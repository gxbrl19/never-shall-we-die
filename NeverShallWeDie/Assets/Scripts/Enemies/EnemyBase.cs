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
    [SerializeField] protected int currentHealth;
    [SerializeField] GameObject deathEffect;
    protected Rigidbody2D rb;
    protected Animator animator;
    protected Color defaultColor;
    protected Color damageColor = Color.red;
    protected SpriteRenderer sprite;
    protected bool isHurt = false;
    protected bool isDead = false;

    [Header("FMOD Events")]
    [SerializeField] EventReference hit;
    [SerializeField] EventReference death;

    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();

        if (enemyObject != null)
        {
            currentHealth = enemyObject.maxHealth;
            enemyName = enemyObject.name;
            dropRate = enemyObject.dropRate;
            dropPrefab = enemyObject.dropPrefab;
            defaultColor = sprite.color;
        }
        else
        {
            Debug.LogWarning($"{gameObject.name} está sem EnemyObject atribuído!");
            currentHealth = 3; //fallback
        }
    }

    public virtual void TakeHit(int power, Vector2 hitDirection, float knockbackForce = 5f)
    {
        if (isHurt || isDead || enemyName == "Beetboom" || enemyName == "Barrel Monkey") return;

        isHurt = true;
        Invoke("ResetHurt", .2f);
        TakeDamage(power);

        //knockback
        if (isDead || enemyName == "Evil Vine") return;
        rb.velocity = Vector2.zero;
        rb.velocity = new Vector2(hitDirection.normalized.x * knockbackForce, 0f);
    }

    public virtual void TakeDamage(int amount)
    {
        currentHealth -= amount;
        sprite.color = damageColor;
        CinemachineShake.instance.ShakeCamera(3f, 0.15f); //tremida na camera

        if (currentHealth <= 0)
            Die();
        else
            OnHurt();
    }

    protected virtual void OnHurt()
    {
        //animator.SetTrigger("Hurt");
        RuntimeManager.PlayOneShot(hit);
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

    protected virtual void ResetHurt() //chamado no TakeHit()
    {
        rb.velocity = Vector2.zero;
        sprite.color = defaultColor;
        isHurt = false;
    }
}
