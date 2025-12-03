using UnityEngine;
[RequireComponent(typeof(Rigidbody2D))]
public abstract class Enemy : MonoBehaviour
{
    [SerializeField] protected float enemyDamage;
    [SerializeField] protected float enemySpeed;
    protected Rigidbody2D enemyRb;
    protected virtual void Start()
    {  
        enemyRb = GetComponent<Rigidbody2D>();
    }

    protected virtual void Update()
    {
    }
    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.CompareTag(GameManagerScript.Instance.tagSO.playerTag)) 
        {
           if(collision.transform.TryGetComponent(out HealthScript healthScript)) 
            {
                healthScript.TakeDamage(enemyDamage, gameObject);
                Debug.Log("Damaged Player");
            }
        }
    }
    protected abstract void EnemyAIMovement();
    protected abstract void PlayEnemySound();
    protected abstract void Die();
}
