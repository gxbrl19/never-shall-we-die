using System;
using System.Collections;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [Header("Vida e Mana")]
    [HideInInspector] public float maxHealth;
    public float currentHealth;
    [HideInInspector] public float maxHealing;
    public float currentHealing;

    [Header("Partículas & Visual")]
    public GameObject _particleHit;
    public Color _damageColor;

    SpriteRenderer spriteRenderer;
    Color startColor;

    Player player;
    PlayerAudio playerAudio;
    PlayerAnimations playerAnimations;
    PlayerInputs playerInput;

    private void Awake()
    {
        player = GetComponent<Player>();
        playerAudio = GetComponent<PlayerAudio>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        playerAnimations = GetComponent<PlayerAnimations>();
        playerInput = GetComponent<PlayerInputs>();
    }

    void Start()
    {
        maxHealth = GameManager.instance._hpMax;
        maxHealing = GameManager.instance._hlMax;

        startColor = spriteRenderer.color;
        currentHealth = GetHealth();
        currentHealing = GetHealing();
    }

    void FixedUpdate()
    {

    }

    public void TakeDamage(int damage)
    {
        if (player.isDead || player.onHit) return;

        currentHealth -= damage;
        SetHealth(currentHealth);
        playerAudio.PlayHit();
        CinemachineShake.instance.ShakeCamera(6f, 0.15f);

        if (currentHealth < 1)
            Die();
        else
            OnHit();

        Instantiate(_particleHit, player.transform.position, Quaternion.identity);
    }

    void OnHit()
    {
        player.rb.velocity = Vector2.zero;
        playerAnimations.OnHit();
        player.gameObject.layer = LayerMask.NameToLayer("Invencible");
        player.onHit = true;
        player.canMove = false;
        playerInput.CancelInputs();
        player.CancelMovesOnHit();
        spriteRenderer.color = _damageColor;
    }

    public void FinishHit() //chamado da animação de Hit
    {
        player.onHit = false;
        player.canMove = true;
        StartCoroutine(FinishInvincible());
    }

    void Die()
    {
        player.isDead = true;
        playerAudio.PlayDeath();
        player.rb.velocity = Vector2.zero;
        player.OnDead();
        BackgroundMusic.instance.MusicControl(9);
        LostGold(0.3f);
    }

    IEnumerator FinishInvincible()
    {
        float hitDuration = 0.8f;
        float elapsed = 0f;
        bool visible = true;

        while (elapsed < hitDuration)
        {
            spriteRenderer.color = visible ? new Color(1, 1, 1, 0.5f) : _damageColor;
            visible = !visible;
            yield return new WaitForSeconds(0.1f);
            elapsed += 0.1f;
        }

        spriteRenderer.color = startColor;
        player.gameObject.layer = LayerMask.NameToLayer("Player");
    }

    void LostGold(float percentual)
    {
        int gold = GameManager.instance._gold;

        if (gold > 0)
        {
            int lostGold = Mathf.RoundToInt(gold * percentual);
            GameManager.instance._gold -= lostGold;
        }
    }

    public void FillHealing(float healing)
    {
        currentHealing += healing;

        if (currentHealing >= maxHealing)
            currentHealing = maxHealing;

        SetHealing(currentHealing);
    }

    public void Healing()
    {
        currentHealth += 2;
        currentHealth = currentHealth > maxHealth ? maxHealth : currentHealth;

        currentHealing -= 1;

        SetHealth(currentHealth);
        SetHealing(currentHealing);
    }

    public void ResetHealth()
    {
        currentHealth = maxHealth;
        currentHealing = 0;
    }

    #region GameManager (Persistência entre cenas)

    public void SetHealth(float health)
    {
        GameManager.instance._currentHP = health;
    }

    public float GetHealth()
    {
        return GameManager.instance._currentHP != 0 ? GameManager.instance._currentHP : maxHealth;
    }

    public void SetHealing(float mana)
    {
        GameManager.instance._currentHL = mana;
    }

    public float GetHealing()
    {
        return GameManager.instance._currentHL != 0 ? GameManager.instance._currentHL : 0;
    }

    #endregion
}
