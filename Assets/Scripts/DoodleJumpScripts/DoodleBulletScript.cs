using Unity.VisualScripting;
using UnityEngine;
[RequireComponent(typeof(Rigidbody2D),typeof(HealthScript))]
public class DoodleBulletScript : Enemy
{
    private DoodleCameraScript cameraScript;
    private HealthScript enemyHealthScript;

    protected override void EnemyAIMovement()
    {
        float directionX = Mathf.Sin(Time.time) * enemySpeed;
        enemyRb.AddForce(new Vector2(directionX, 0));
        cameraScript.StayInViewPort(transform);
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
}
