using Pathfinding;
using UnityEngine;

public class PathFindingScript : MonoBehaviour
{
    [Header("EnemyAI")]
    [SerializeField] private Transform target;
    [SerializeField] private float speed;
    [SerializeField] private float nextWaypointDistance;

    private Animator enemyAnimator;
    private Path path;
    private int currentWaypoint;
    private bool reachedEndOfPath;
    private Seeker seeker;
    private Rigidbody2D enemyRb;
    [Space]
    private bool hasKnockback;
    [SerializeField] private float knockbackAgainstEnemyForce;
    [SerializeField] private float enemyDamage;
    [SerializeField] private float knockbackTimer;
    private float knockbackTimerTmp;
    private float animationTimer;
    private bool isPlayingAttackAnimation;
    [SerializeField] private GridGraph graph;
    private float timer = 1f;
    private float timerTmp = 0f;
    private void OnEnable()
    {
        target = GameObject.FindGameObjectWithTag(GameManagerScript.Instance.tagSO.playerTag).transform;
        seeker = GetComponent<Seeker>();
        enemyRb = GetComponent<Rigidbody2D>();
        GetComponent<HealthScript>().OnDeath += DestroyObject;
        InvokeRepeating(nameof(UpdatePath), 0, .5f);
        seeker.StartPath(enemyRb.position, target.position, OnPathComplete);
        knockbackTimerTmp = knockbackTimer;

    }
    void Start()
    {
        //target = GameObject.FindGameObjectWithTag(GameManagerScript.Instance.tagSO.playerTag).transform;
        graph = AstarPath.active.data.gridGraph;
    }

    public void UpdatePath()
    {
        if (seeker.IsDone())
        {
            seeker.StartPath(enemyRb.position, target.position, OnPathComplete);
        }
    }
    void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
            print("Path");
            currentWaypoint = 0;
        }
    }
    // Update is called once per frame
    void Update()
    {
        timerTmp += Time.deltaTime;
        if(timerTmp >= timer) 
        {
            //timerTmp = 0f;
            //graph.active.Scan();
        }

        //aiPath.desiredVelocity klappt nicht
        //if (enemyRb.linearVelocityX >= 0)
        //{

        //    GetComponent<SpriteRenderer>().flipX = false;
        //}
        //else
        //{
        //    GetComponent<SpriteRenderer>().flipX = true;
        //}
        //SearchPath();
        //if (Vector2.Distance(transform.position, target.position) < 2f)
        //{
        //    //if (!enemyAnimator.IsPlaying("CrabselAttack"))
        //    //{
        //    //    CancelInvoke(nameof(UpdatePath));
        //    //    enemyAnimator.SetTrigger("Attack");
        //    //    AnimatorClipInfo[] info = enemyAnimator.GetCurrentAnimatorClipInfo(0);
        //    //    AnimationClip animationClip = info[0].clip;
        //    //    animationTimer = animationClip.length;
        //    //    isPlayingAttackAnimation = true;
        //    //}
        //}
        //SearchPath();
        //KnockbackCheck();

    }
    public void SearchPath()
    {
        if (isPlayingAttackAnimation)
        {
            animationTimer -= Time.deltaTime;
            if (animationTimer <= 0)
            {
                InvokeRepeating(nameof(UpdatePath), 0, .5f);
                isPlayingAttackAnimation = false;
            }
        }

    }
    private void FixedUpdate()
    {
        AILogic();
    }
    private void AILogic()
    {

        if (path == null)
            return;
        if (currentWaypoint >= path.vectorPath.Count)
        {
            return;
        }

        Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - enemyRb.position).normalized;
        Vector2 force = direction * speed * Time.deltaTime;
        enemyRb.AddForce(force);
        float distance = Vector2.Distance(path.vectorPath[currentWaypoint], enemyRb.position);
        if (distance < nextWaypointDistance)
        {
            currentWaypoint++;
        }
    }
    private void DestroyObject()
    {
        Destroy(gameObject);
        GetComponent<HealthScript>().OnDeath -= DestroyObject;
    }
    private void KnockbackCheck()
    {
        if (hasKnockback)
        {
            knockbackTimer -= Time.deltaTime;
            if (knockbackTimer <= 0)
            {
                enemyRb.linearVelocity = Vector2.zero;
                knockbackTimer = knockbackTimerTmp;
                hasKnockback = false;
                GetComponent<SpriteRenderer>().color = Color.white;
            }
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.tag == "Player")
        {
            collision.transform.GetComponent<HealthScript>().TakeDamage(enemyDamage,gameObject);
            Vector2 direction = (collision.transform.position - transform.position).normalized;
            //collision.transform.GetComponent<PlayerMovementScript>().Knockback(direction, knockbackAgainstEnemyForce);
        }
    }
    public void Knockback(Vector2 direction, float knockbackForce)
    {
        enemyRb.AddForce(direction * knockbackForce, ForceMode2D.Impulse);
        hasKnockback = true;
    }


}
public static class AnimatorExtension
{
    public static bool IsPlaying(this Animator animator, string clipName, int layer = 0)
    {

        AnimatorClipInfo[] info = animator.GetCurrentAnimatorClipInfo(layer);
        if (info.Length != 0)
        {
            return info[0].clip.name == clipName;
        }
        return false;
    }
}
