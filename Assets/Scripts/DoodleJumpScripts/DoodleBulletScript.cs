using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.Universal;
[RequireComponent(typeof(Rigidbody2D),typeof(HealthScript))]
public class DoodleBulletScript : Enemy
{
    [SerializeField] float enemyDetectionRadius;
    private DoodleCameraScript cameraScript;
    private HealthScript enemyHealthScript;
    private Transform target;
    protected override void EnemyAIMovement()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, enemyDetectionRadius);
        foreach (Collider2D collider in colliders) 
        {
            if (collider.transform.tag == GameManagerScript.Instance.tagSO.playerTag)
            {
                target = collider.transform;
                break;
            }
            else target = null;
        }
        if(target == null) 
        {
            float forceX = Mathf.Sin(Time.time) * enemySpeed;
            enemyRb.AddForce(new Vector2(forceX, 0));
            cameraScript.StayInViewPort(transform);
            return;
        }
        Vector2 direction = (Vector2) (target.position - transform.position).normalized;
        enemyRb.AddForce(direction * enemySpeed);
    }
    public void SetDetectionRadius(float radius) 
    {
        enemyDetectionRadius = radius;
    }
     protected override void Start()
    {
        base.Start();
        cameraScript = Camera.main.transform.GetComponent<DoodleCameraScript>();
        enemyHealthScript = GetComponent<HealthScript>();
        enemyHealthScript.OnDamaged += PlayEnemySound;
        enemyHealthScript.OnDeath += Die;
    }
    protected override void Update()
    {
        EnemyAIMovement();
    }
    protected override void PlayEnemySound()
    {   
        //AudioManager
    }

    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        base.OnCollisionEnter2D(collision);
    }

    protected override void Die()
    {
        print("I am Dead");
        GetComponent<SpriteRenderer>().enabled = false;
        GetComponent<Collider2D>().enabled = false;
        Destroy(gameObject, 2);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, enemyDetectionRadius);
    }
}
