using UnityEngine;
[RequireComponent(typeof(Rigidbody2D))]
public class DoodleProjectileScript : MonoBehaviour
{
    [SerializeField] float projectileSpeed;
    [SerializeField] float projectileDamage;
    [HideInInspector] public Vector2 projectileDirection;
    private Rigidbody2D projectileRb;
    void Start()
    {
        projectileRb = GetComponent<Rigidbody2D>();
    }
    private void OnEnable()
    {
        Destroy(gameObject,2);
    }
    // Update is called once per frame
    private void OnDestroy()
    {
        DoodleWeaponScript.projectiles.Remove(this);
        DoodleWeaponScript.OnDestoy?.Invoke();
    }
    void Update()
    {
        ProjectileMovement();
    }
    public void SetDirection(Vector2 direction) 
    {
        projectileDirection = direction;
    }
    void ProjectileMovement() 
    {
        projectileRb.AddForce(projectileDirection * projectileSpeed, ForceMode2D.Impulse);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.transform.TryGetComponent(out HealthScript healthScript) && !collision.transform.CompareTag(GameManagerScript.Instance.tagSO.playerTag))
        {
            healthScript.TakeDamage(projectileDamage, gameObject);
        }
    }
}
