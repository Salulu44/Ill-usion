using DG.Tweening;
using UnityEngine;

public class NurseEnemyScript : Enemy
{
    [SerializeField] float rotationTimer;
    [SerializeField] float castRadius;
    [SerializeField] float degrees;
    [SerializeField] LayerMask targetLayers;
    [SerializeField] float playerinRadius;
    Transform playerTr;
    bool canRotate = true;
    protected override void Die()
    {
       
    }

    protected override void EnemyAIMovement()
    {
        Rotate();
        CheckPlayer();
    }
    void MoveToPlayer()
    {
        if(playerTr != null)
        {
            enemyRb.linearVelocity = (playerTr.position - transform.position).normalized * enemySpeed;

        }
    }
    void CheckPlayer()
    {
        ContactFilter2D filter = new ContactFilter2D();
        filter.useTriggers = false; // <-- Ignoriert Trigger!
        filter.SetLayerMask(targetLayers);
        filter.useLayerMask = true;
        RaycastHit2D[] results = new RaycastHit2D[1];
        int hitCount = Physics2D.CircleCast(
            transform.position,
            castRadius,
            transform.right,
            filter,
            results
        );
        if (hitCount > 0)
        {
            RaycastHit2D hit = results[0];
            if (hit.transform.tag == GameManagerScript.Instance.tagSO.playerTag)
            {
                Debug.Log("Hit Player");
                playerTr = hit.transform;
            }
            else
            {
                Debug.Log($"Hit {hit.transform.gameObject}");
            }
        }
    }
    void Rotate()
    {
        Vector3 targetRotation = Vector3.zero;
        if (canRotate && playerTr == null)
        {
            //Debug.Log("Hiiii");
            //targetRotation = new Vector3(0, 0, (transform.eulerAngles.z + degrees) % 360);
            //Debug.Log("TargetRotation " + targetRotation);
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
    }
    protected override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    protected override void Update()
    {
        EnemyAIMovement();
        Debug.Log(transform.up);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position, transform.position + transform.up);
    }
}

