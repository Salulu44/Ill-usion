using DG.Tweening;
using UnityEngine;

public class NurseEnemyScript : Enemy
{
    [SerializeField] float rotationTimer;
    [SerializeField] float sightDistance;
    [SerializeField] float knockbackStrength;
    //[SerializeField] float castRadius;
    [SerializeField] float colorChangeTimer;
    float colorChangeTimerTmp;
    bool hasChangedColor;
    [SerializeField] float degrees;
    [SerializeField] LayerMask targetLayers;
    //[SerializeField] float playerInRadius;
    [SerializeField] float attackTimer;
    float attackRandomTimer;
    [SerializeField] float followPlayerTimer;
    float followPlayerTimerTmp;
    Transform playerTr;
    bool canRotate = true;
    bool isFollowing;
    SpriteRenderer nurseRenderer;
    Color defaultColor;
    protected override void Start()
    {
        base.Start();
        followPlayerTimerTmp = followPlayerTimer;
        attackRandomTimer = UnityEngine.Random.Range(0,attackTimer);
        nurseRenderer = GetComponent<SpriteRenderer>();
        defaultColor = nurseRenderer.color;
        colorChangeTimerTmp = colorChangeTimer;
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
                collision.transform.gameObject.GetComponent<PlayerMovementScript>().ApplyKnockback(enemyRb.linearVelocity, knockbackStrength);
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
               // nurseRenderer.color = Color.red;
                hasChangedColor = true;
                enemyRb.AddForce((playerTr.transform.position - transform.position).normalized * attackSpeed, ForceMode2D.Impulse);
                
                attackRandomTimer = UnityEngine.Random.Range(0, attackTimer);
            }
        }
    }
    void ChangeColorToDefault()
    {
        if (hasChangedColor)
        {
            colorChangeTimer -= Time.deltaTime;
            if(colorChangeTimer <= 0)
            {
                colorChangeTimer = colorChangeTimerTmp;
                nurseRenderer.color = defaultColor;
                hasChangedColor = false;
            }
        }
    }
    void MoveToPlayer()
    {
        if(playerTr != null)
        {
            isFollowing = true;
            enemyRb.linearVelocity = (playerTr.position - transform.position).normalized * enemySpeed;
            followPlayerTimer -= Time.deltaTime;
            enemyRb.constraints = RigidbodyConstraints2D.FreezeRotation;
            if (followPlayerTimer <= 0) 
            {
                followPlayerTimer = followPlayerTimerTmp;
                playerTr = null;
                isFollowing = false;
                enemyRb.constraints = RigidbodyConstraints2D.None;
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
        if (!isFollowing)
        {
            ContactFilter2D filter = new ContactFilter2D();
            filter.useTriggers = false;
            filter.SetLayerMask(targetLayers);
            filter.useLayerMask = true;
            RaycastHit2D[] results = new RaycastHit2D[1];
            int hitCount = Physics2D.Raycast(transform.position, transform.right, filter, results, sightDistance);
            if (hitCount > 0)
            {
                if (results[0].transform.tag == GameManagerScript.Instance.tagSO.playerTag)
                {
                    playerTr = results[0].transform;
                }
                Debug.Log("Hit " + results[0].transform.gameObject.name);
            }
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
      //  ChangeColorToDefault();
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position, transform.position + transform.right);
    }
}

