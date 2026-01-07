using DG.Tweening.Core.Easing;
using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.Universal;
[RequireComponent(typeof(Rigidbody2D),typeof(HealthScript))]
public class DoodleBulletScript : Enemy
{
    [System.Flags]
    public enum DoodleBulletVariants
    {
        None = 0,
        Chase = 1 << 0,
        Bounce = 1 << 1,
        UI = 1 << 2
    }
    [SerializeField] float enemyDetectionRadius;
    private DoodleCameraScript cameraScript;
    private HealthScript enemyHealthScript;
    private Transform target;
    [SerializeField] DoodleBulletVariants doodleVariant = DoodleBulletVariants.Chase;
    [SerializeField] float bounceForce;
    RectTransform doodleUITr;
    Vector2 canvasResolution;
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
        DetermineVariant();
        //variantFlags |=(byte) DoodleBulletVariants.Bounce | (byte) DoodleBulletVariants.UI;
        //Debug.Log(( (byte)variantFlags & (byte)DoodleBulletVariants.Bounce) != 0);
        //Debug.Log($"doodleVariant: {(int)doodleVariant}");
        //Debug.Log("Number" + (byte)(doodleVariant & DoodleBulletVariants.Bounce) + " " + gameObject.name);
        //if (( (byte)doodleVariant & (byte) DoodleBulletVariants.Bounce) != 0)
        //{
        //    Debug.Log("Is no bounce " + gameObject.name);
        //}
        //else 
        //{
        //    Debug.Log("Yeah Bounce");
        //}
    }
     void DetermineVariant() 
    {
        if ((doodleVariant & DoodleBulletVariants.Chase) != 0)
        {
          //If something is needed in Start
        }
        if ((doodleVariant & DoodleBulletVariants.Bounce) != 0)
        {
            doodleUITr = GetComponent<RectTransform>();
            Debug.Log("RectTransform " + gameObject.name);
            canvasResolution = doodleUITr.parent.GetComponent<RectTransform>().rect.size;
            
        }
    }
    protected override void Update()
    {
        if((doodleVariant & DoodleBulletVariants.Chase) != 0) 
        {
            EnemyAIMovement();
        }
        else 
        {
            Debug.Log("Ist null" + (doodleVariant & DoodleBulletVariants.Chase) + " " + gameObject.name);
        }
        if ((doodleVariant & DoodleBulletVariants.Bounce) != 0)
        {
            UIOrientation();
        }

    }
    void Bounce(Collision2D collider)
    {
        float paddleX = collider.transform.position.x;
        float ballX = transform.position.x;
        float halfWidth = collider.collider.bounds.size.x * 0.5f;

        // -1 (linke Ecke) bis +1 (rechte Ecke)
        float t = Mathf.Clamp((ballX - paddleX) / halfWidth, -1f, 1f);

        // Basiswinkel in Grad relativ zur Senkrechten
        float maxAngle = 60f;
        float angleDeg = t * maxAngle;

        // Richtung aus Winkel bauen (immer nach oben)
        float angleRad = angleDeg * Mathf.Deg2Rad;
        Vector2 dir = new Vector2(Mathf.Sin(angleRad), Mathf.Cos(angleRad)).normalized;

        enemyRb.linearVelocity = dir * bounceForce;
    }
    void UIOrientation() 
    {
        UIExtensions.VectorOrientation orientation = doodleUITr.CheckOrientation();
        Vector2 pos = doodleUITr.anchoredPosition;
        switch (orientation)
        {
            case UIExtensions.VectorOrientation.Below:
                pos.y = canvasResolution.y / 2f -30;
                break;

            case UIExtensions.VectorOrientation.Above:
                pos.y = -canvasResolution.y / 2f +30;
                break;

            case UIExtensions.VectorOrientation.Left:
                pos.x = canvasResolution.x / 2f -30;
                break;
            case UIExtensions.VectorOrientation.Right:
                pos.x = -canvasResolution.x / 2f +30;
                break;
        }
        doodleUITr.anchoredPosition = pos;
    }
    protected override void PlayEnemySound()
    {   
        //AudioManager
    }

    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        base.OnCollisionEnter2D(collision);
        if((doodleVariant & DoodleBulletVariants.Bounce ) != 0) 
        {
           if(collision.transform.tag == GameManagerScript.Instance.tagSO.doodlePaddleTag)
            {
             Bounce(collision);
            }
           if(collision.transform.tag == GameManagerScript.Instance.tagSO.doodlePlayTag) 
            {
                collision.gameObject.GetComponent<Animator>().SetBool("IsHovering", true);
            }
            Debug.Log(collision.gameObject.name);

        }
    }

    protected override void Die()
    {
        print("I am Dead");
        SpawnEnemiesTestScript.enemyCounter--;
        GetComponent<SpriteRenderer>().enabled = false;
        GetComponent<Collider2D>().enabled = false;
        Destroy(gameObject, 2);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, enemyDetectionRadius);
    }
}
public static class UIExtensions
{
    public enum VectorOrientation
    {
        Inside,
        Left,
        Right,
        Above, 
        Below
    }
    public static bool IsRectCompletelyOffScreen(this RectTransform rectTransform)
    {
        Vector3[] corners = new Vector3[4];
        rectTransform.GetWorldCorners(corners);
        foreach (var corner in corners)
        {
            Vector3 screenPoint = RectTransformUtility.WorldToScreenPoint(null, corner);
            if (screenPoint.x >= 0 && screenPoint.x <= Screen.width &&
                screenPoint.y >= 0 && screenPoint.y <= Screen.height)
            {
                return false; // if one corner is visible
            }
        }

        return true; // all are not visible
    }
    public static bool IsRectHorizontallyOffScreen(this RectTransform rectTransform) 
    {
       
        Vector3[] corners = new Vector3[4];
        rectTransform.GetWorldCorners(corners);

        foreach (Vector3 corner in corners) 
        {
            Vector3 screenPoint = RectTransformUtility.WorldToScreenPoint(null, corner);
            if (screenPoint.x >= 0 && screenPoint.x <= Screen.width)
            {
                return false;
            }
        }
        return true;
    }
    public static VectorOrientation CheckOrientation(this RectTransform rectTransform)
    {
        Vector3[] corners = new Vector3[4];
        rectTransform.GetWorldCorners(corners);
        bool anyAbove = false;
        bool anyBelow = false;
        bool anyRight = false;
        bool anyLeft = false;
        bool anyInside = false;
        foreach (var corner in corners)
        {
            Vector3 screenPoint = RectTransformUtility.WorldToScreenPoint(null, corner);
            if (screenPoint.y >= Screen.height)
            {
                anyAbove = true;
            }
            else if(screenPoint.y <= 0)
            {
                anyBelow = true;
            }
            else if (screenPoint.x >= Screen.width) 
            {
                anyRight = true;
            }
            else if(screenPoint.x <= 0)
            {
                anyLeft = true;
            }
            else 
            {
                anyInside = true;
            }
        }
        if (anyInside)
            return VectorOrientation.Inside;

        if (anyAbove && !anyBelow)
            return VectorOrientation.Above;

        if (anyBelow && !anyAbove)
            return VectorOrientation.Below;
        if(anyLeft && !anyRight)
        {
            return VectorOrientation.Left;
        }
        if(!anyLeft && anyRight) 
        {
            return VectorOrientation.Right;
        }
        //if the UI Element is too big but still inside
        return VectorOrientation.Inside;
    }
}

