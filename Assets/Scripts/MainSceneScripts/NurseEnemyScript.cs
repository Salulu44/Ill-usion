using DG.Tweening;
using UnityEngine;

public class NurseEnemyScript : Enemy
{
    [SerializeField] float rotationTimer;
    [SerializeField] float castRadius;
    [SerializeField] float degrees;
    [SerializeField] LayerMask targetLayers;
    [SerializeField] float playerInRadius;
    [SerializeField] float attackTimer;
    float attackRandomTimer;
    [SerializeField] float followPlayerTimer;
    float followPlayerTimerTmp;
    Transform playerTr;
    bool canRotate = true;
    protected override void Start()
    {
        base.Start();
        followPlayerTimerTmp = followPlayerTimer;
        attackRandomTimer = UnityEngine.Random.Range(0,attackTimer);
    }

    protected override void Die()
    {
       
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.CompareTag(GameManagerScript.Instance.tagSO.playerTag))
        {
            if (collision.transform.TryGetComponent(out HealthScript healthScript))
            {
                Debug.Log("Hit Enemy");
                healthScript.TakeDamage(enemyDamage, gameObject);
                followPlayerTimer = followPlayerTimerTmp;
            }
        }
    }
    protected override void EnemyAIMovement()
    {
        Rotate();
    }
    void Attack()
    {
        if(playerTr != null)
        {
            attackRandomTimer -= Time.fixedDeltaTime;
            if (attackRandomTimer <= 0)
            {
                Debug.Log("Attack");
                enemyRb.AddForce((playerTr.transform.position - transform.position).normalized * attackSpeed, ForceMode2D.Impulse);
                attackRandomTimer = UnityEngine.Random.Range(0, attackTimer);
            }
        }
    }
    void MoveToPlayer()
    {
        if(playerTr != null)
        {
            enemyRb.linearVelocity = (playerTr.position - transform.position).normalized * enemySpeed;
            followPlayerTimer -= Time.deltaTime;
            if (followPlayerTimer <= 0) 
            {
                followPlayerTimer = followPlayerTimerTmp;
                playerTr = null;
            }
        }
    }
    void CheckPlayer()
    {
        //ContactFilter2D filter = new ContactFilter2D();
        //filter.useTriggers = false;
        //filter.SetLayerMask(targetLayers);
        //filter.useLayerMask = true;
        //RaycastHit2D[] results = new RaycastHit2D[1];
        //int hitCount = Physics2D.CircleCast(
        //    transform.position,
        //    castRadius,
        //    transform.right,
        //    filter,
        //    results
        //);
        //if (hitCount > 0)
        //{
        //    RaycastHit2D hit = results[0];
        //    if (hit.transform.tag == GameManagerScript.Instance.tagSO.playerTag)
        //    {
        //        Debug.Log("Hit Player");
        //        playerTr = hit.transform;
        //    }
        //    else
        //    {
        //        Debug.Log($"Hit {hit.transform.gameObject}");
        //    }
        //}
        ContactFilter2D filter = new ContactFilter2D();
        filter.useTriggers = false;
        filter.SetLayerMask(targetLayers);
        filter.useLayerMask = true;
        RaycastHit2D[] results = new RaycastHit2D[1];
        int hitCount = Physics2D.Raycast(transform.position, transform.up,filter,results);
        if (hitCount > 0) 
        {
            if (results[0].transform.tag == GameManagerScript.Instance.tagSO.playerTag)
            {
                playerTr = results[0].transform;
            }
            Debug.Log("Hit " + results[0].transform.gameObject.name);
        }
    }
    void Rotate()
    {
        if (canRotate && playerTr == null)
        {
            canRotate = false;
            transform.DORotate(new Vector3(0, 0, degrees), rotationTimer, RotateMode.LocalAxisAdd).OnComplete(() => canRotate = true);
        }
    }
    protected override void PlayEnemySound()
    {
        
    }
    private void FixedUpdate()
    {
        MoveToPlayer();
        CheckPlayer();
        Attack();
    }

    // Update is called once per frame
    protected override void Update()
    {
        EnemyAIMovement();
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position, transform.position + transform.up);
    }
}

