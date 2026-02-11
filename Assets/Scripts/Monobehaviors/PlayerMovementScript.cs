using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovementScript : MonoBehaviour
{
    [Header("Running")]
    [SerializeField] float walkSpeed = 5;
    [SerializeField] float runSpeed = 9;
    [SerializeField] float maxSpeed;
    [Space]
    [SerializeField] float knockbackTimer;
    float knockbackTimerTmp;
    private float currentSpeed;
    public bool IsRunning { get; private set; }
    bool hasKnockback;
    [HideInInspector] public bool canRun = true;

    Rigidbody2D playerRb;
    [HideInInspector] public bool isGrappling;
    private Vector2 playerInput;
    private void Start()
    {
        playerRb = GetComponent<Rigidbody2D>();
        knockbackTimerTmp = knockbackTimer;
        GetComponent<HealthScript>().OnDeath += Dead;
    }
    void Dead()
    {
        Debug.Log("I am dead");
    }
    void FixedUpdate()
    {
        // playerRb.AddForce(playerInput * currentSpeed);
        currentSpeed = Mathf.Clamp(currentSpeed, -maxSpeed, maxSpeed);
        if (!hasKnockback)
        {
            playerRb.linearVelocity = new Vector2(playerInput.normalized.x * currentSpeed, playerInput.normalized.y * currentSpeed);
        }
    }
    private void Update()
    {
        PlayerMovement();
        CheckKnockback();
    }
    void PlayerMovement()
    {
        playerInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
        {
            GetComponent<SpriteRenderer>().flipX = false;
            currentSpeed += Input.GetKeyDown(KeyCode.LeftShift) ? runSpeed * .1f : walkSpeed * .1f;
        }
        else if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
        {
            GetComponent<SpriteRenderer>().flipX = true;
            currentSpeed += Input.GetKeyDown(KeyCode.LeftShift) ? runSpeed * .1f : walkSpeed * .1f;
        }
        else if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W))
        {
            // Set forward sprite
            currentSpeed += Input.GetKeyDown(KeyCode.LeftShift) ? runSpeed * .1f : walkSpeed * .1f;
        }
        else if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S))
        {
            //Set backwards sprite
            currentSpeed += Input.GetKeyDown(KeyCode.LeftShift) ? runSpeed * .1f : walkSpeed * .1f;
        }
        if (Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.D) || Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.S))
        {
            currentSpeed *= .5f;
        }
    }
    void CheckKnockback()
    {
        if (hasKnockback)
        {
            knockbackTimer -= Time.deltaTime;
            if (knockbackTimer <= 0)
            {
                knockbackTimer = knockbackTimerTmp;
                hasKnockback = false;
            }
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
    }
    public void ApplyKnockback(Vector2 direction, float strength)
    {
        playerRb.AddForce(direction.normalized * strength, ForceMode2D.Impulse);
        Debug.Log("Knockback");
        hasKnockback = true;
    }
    void Dot () 
    {
        //// transform the forward vector from local to world space
        //Vector3 forward = transform.TransformDirection(Vector3.forward);
        //// calculate a unit vector from the other object to this object
        //Vector3 toOther = Vector3.Normalize(other.position - transform.position);
        //// use the dot product sign to determine whether other is in front or behind
        //if (Vector3.Dot(forward, toOther) < 0)
        //{
        //    //print("The other transform is behind me!");

        //}

    }
}
