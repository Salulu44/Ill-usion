using UnityEngine;
using UnityEngine.PlayerLoop;

public class DoodlePlayerScript : MonoBehaviour
{
    [SerializeField] float movementSpeed;
    [SerializeField] float jumpForce;
    [SerializeField] float doubleJumpTimer;
    [SerializeField] float doubleJumpForce;
    [SerializeField] float playerDamage;
    HealthScript healthScript;
    Rigidbody2D playerRb;
    float horizontalInputX;
    float verticalInputY;
    float doubleJumpTimerTmp;
    public Color playerColor { get; private set; }
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

        if (healthScript.invincible) 
        {
            StatusCheck();
        }
    }
    void StatusCheck() 
    {
        healthScript.invisibleTimer -= Time.deltaTime;
        if(healthScript.invisibleTimer < 0) 
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
        velocity += new Vector2(horizontalInputX, -Mathf.Abs(verticalInputY)) * movementSpeed * Time.unscaledDeltaTime;
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
}
