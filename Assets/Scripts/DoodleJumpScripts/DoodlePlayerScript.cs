using UnityEngine;

public class DoodlePlayerScript : MonoBehaviour
{
    [SerializeField] float movementSpeed;
    [SerializeField] float jumpForce;
    [SerializeField] float doubleJumpTimer;
    [SerializeField] float doubleJumpForce;
    Rigidbody2D playerRb;
    float horizontalInputX;
    bool doubleJumpReady;
    float doubleJumpTimerTmp;
    void Start()
    {
        playerRb = GetComponent<Rigidbody2D>();
        doubleJumpTimerTmp = doubleJumpTimer;
    }

    // Update is called once per frame
    void Update()
    {
        DoubleJump();
        horizontalInputX = Input.GetAxis("Horizontal");
        if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
        {
            GetComponent<SpriteRenderer>().flipX = true;
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
        {
            GetComponent<SpriteRenderer>().flipX = false;
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
        Movement();
    }
    void Movement()
    {
        Vector2 velocity = playerRb.linearVelocity;
        velocity += new Vector2(horizontalInputX, 0) * movementSpeed * Time.deltaTime;
        if (playerRb.bodyType == RigidbodyType2D.Dynamic)
        {
            velocity.x = Mathf.Clamp(velocity.x, -10, 10);
            playerRb.linearVelocity = velocity;
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.CompareTag(GameManagerScript.Instance.tagSO.groundTag) && playerRb.linearVelocityY <= 5f) 
        {
            playerRb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            print("Hi");
        }
    }
}
