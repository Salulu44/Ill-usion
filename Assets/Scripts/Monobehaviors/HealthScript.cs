using UnityEngine;
using UnityEngine.Events;
using UsefulClasses;

public class HealthScript : MonoBehaviour
{
    public float maxHealth = 10f;

    [field:SerializeField] public float currentHealth { get; private set; }

    public bool isInvincible = false;

    public float damageResistance = 0f;
  
    public bool isDead { get; private set; }

    public UnityAction OnDamaged;
    public UnityAction OnHealed;
    public UnityAction OnDeath;
    public UnityTimer invincibleTimer;
    private void Start()
    {
        currentHealth = maxHealth;
        invincibleTimer.PrepareStart();
        isDead = false;
    }
    public void Heal(float healAmount, GameObject healSource)
    {
        float healthBefore = currentHealth;
        currentHealth += healAmount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        float trueHealAmount = currentHealth - healthBefore;

        if (trueHealAmount > 0)
        {
            OnHealed?.Invoke();
        }
    }
    void CheckInvisibility()
    {
        if (isInvincible)
        {
            invincibleTimer.Tick();
            if (invincibleTimer.IsFinished())
            {
                isInvincible = false;
                invincibleTimer.PrepareStart();
            }
        }
    }
    public void TakeDamage(float damage, GameObject damageSource)
    {
        if (isInvincible)
        {
            return;
        }

        float healthBefore = currentHealth;
        currentHealth -= damage * (1f - damageResistance);
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        float trueDamageAmount = healthBefore - currentHealth;
        if (trueDamageAmount > 0)
        {
            OnDamaged?.Invoke();
            isInvincible = true;
        }

        HandleDeath();
    }
    public void SetInvisibility(bool invicible) 
    {
        this.isInvincible = invicible;  
    }
    public void Kill()
    {
        currentHealth = 0;
        OnDamaged?.Invoke();

        HandleDeath();
    }

    public void HandleDeath()
    {
        if (isDead)
            return;

        if (currentHealth <= 0)
        {
            isDead = true;
            OnDeath?.Invoke();
        }
    }

    public void Resurrect()
    {
        currentHealth = maxHealth;
        isDead = false;
    }
    private void Update()
    {
       CheckInvisibility();
    }
}
