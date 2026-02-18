using UnityEngine;
using UnityEngine.PlayerLoop;

public class DoodlePlayerScript : MonoBehaviour, IRespawnable
{
    [SerializeField] float movementSpeed;
    [SerializeField] float jumpForce;
    [SerializeField] float doubleJumpTimer;
    [SerializeField] float doubleJumpForce;
    [SerializeField] float playerDamage;
    public Vector3 respawnPoint;
    HealthScript healthScript;
    Rigidbody2D playerRb;
    float horizontalInputX;
    float verticalInputY;
    float doubleJumpTimerTmp;
    public Color playerColor { get; private set; }
    public Vector3 RespawnPoint { get; set; }
    [SerializeField] float squishTime = 1f;
    [SerializeField] LayerMask crushLayers = -1;
    float crushTimer;
    bool topCrushed, bottomCrushed;
    void Start()
    {
        playerRb = GetComponent<Rigidbody2D>();
        doubleJumpTimerTmp = doubleJumpTimer;
        healthScript = GetComponent<HealthScript>();
        healthScript.OnDeath += Dead;
        healthScript.OnDamaged += Hurt;
        playerColor = GetComponent<SpriteRenderer>().color;
    }
    public void Hurt() 
    {
        //Play Hurt Animation
        GetComponent<SpriteRenderer>().color = Color.red;
        healthScript.SetInvisibility(true);
    }
    public void Dead() 
    {
        //Play Die Animation
        healthScript.OnDeath -= Dead;
    }
    
    // Update is called once per frame
    void Update()
    {
        if (Time.timeScale == 0f) return;
        DoubleJump();
        Movement();
        if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
        {
            GetComponent<SpriteRenderer>().flipX = true;
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
        {
            GetComponent<SpriteRenderer>().flipX = false;
        }

        if (healthScript.isInvincible) 
        {
            StatusCheck();
        }
        CheckCrush();
    }
    void CheckCrush()
    {
        topCrushed = Physics2D.OverlapPoint(transform.position + Vector3.up * 0.9f * transform.localScale.y / 2, crushLayers);
        bottomCrushed = Physics2D.OverlapPoint(transform.position - Vector3.up * 0.9f * transform.localScale.y / 2, crushLayers);

        if (topCrushed && bottomCrushed)
        {
            crushTimer += Time.deltaTime;
            if (crushTimer >= squishTime)
            {
                DieSquished(); // Respawn, Zerstören, etc.
            }
        }
        else
        {
            crushTimer = 0;
        }
    }
    void DieSquished()
    {

    }
    void StatusCheck() 
    {
        
        if(healthScript.invincibleTimer.IsFinished()) 
        {
            healthScript.SetInvisibility(false);
            GetComponent<SpriteRenderer>().color = playerColor;
        }
    }
    void DoubleJump()
    {
        doubleJumpTimer -= Time.deltaTime;
        if (doubleJumpTimer <= 0)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                playerRb.AddForce(Vector2.up * doubleJumpForce, ForceMode2D.Impulse);
                doubleJumpTimer = doubleJumpTimerTmp;
            }
        }
    }
    private void FixedUpdate()
    {

    }
    void Movement()
    {
        horizontalInputX = Input.GetAxis("Horizontal");
        verticalInputY = Input.GetAxis("Vertical");
        Vector2 velocity = playerRb.linearVelocity;
        velocity += new Vector2(horizontalInputX, -Mathf.Abs(verticalInputY)) * movementSpeed * Time.deltaTime;
        if (playerRb.bodyType == RigidbodyType2D.Dynamic)
        {
            velocity.x = Mathf.Clamp(velocity.x, -10, 10);
            velocity.y = Mathf.Clamp(velocity.y, -10, 10);
            playerRb.linearVelocity = velocity;
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.CompareTag(GameManagerScript.Instance.tagSO.groundTag) && playerRb.linearVelocityY <= 5f) 
        {
            if(playerRb.linearVelocityY < 0) 
            {
                playerRb.linearVelocityY = 0;
            }
            playerRb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }
    }

    public void SetRespawnPoint(Vector3 respawnPoint)
    {
        RespawnPoint = respawnPoint;
    }

    public void Respawn()
    {
        transform.position = RespawnPoint;
    }
}
