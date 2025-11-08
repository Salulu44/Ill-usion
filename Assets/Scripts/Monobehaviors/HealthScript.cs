using UnityEngine;
using UnityEngine.Events;

public class HealthScript : MonoBehaviour
{
    public float maxHealth = 10f;

    [field:SerializeField] public float currentHealth { get; private set; }

    public bool invincible = false;

    public float damageResistance = 0f;
  
    public bool isDead { get; private set; }

    public UnityAction OnDamaged;
    public UnityAction OnHealed;
    public UnityAction OnDeath;

    private void Start()
    {
        currentHealth = maxHealth;
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

    public void TakeDamage(float damage, GameObject damageSource)
    {
        if (invincible)
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
        }

        HandleDeath();
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
}
