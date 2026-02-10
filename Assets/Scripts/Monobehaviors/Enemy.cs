using UnityEngine;
[RequireComponent(typeof(Rigidbody2D))]
public abstract class Enemy : MonoBehaviour
{
    [SerializeField] protected float enemyDamage;
    [SerializeField] protected float enemySpeed;
    [SerializeField] protected float attackSpeed;
    protected Rigidbody2D enemyRb;
    protected virtual void Start()
    {  
        enemyRb = GetComponent<Rigidbody2D>();
    }

    protected virtual void Update()
    {
    }
    protected abstract void EnemyAIMovement();
    protected abstract void PlayEnemySound();
    protected abstract void Die();
}
