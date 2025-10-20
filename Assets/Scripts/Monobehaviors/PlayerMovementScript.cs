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
    private float currentSpeed;
    public bool IsRunning { get; private set; }
    [HideInInspector] public bool canRun = true;

    Rigidbody2D playerRb;
    [HideInInspector] public bool isGrappling;
    private Vector2 playerInput;
    void Awake()
    {
        playerRb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        playerRb.AddForce(playerInput * currentSpeed);
    }
    private void Update()
    {
        playerInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
        {
            GetComponent<SpriteRenderer>().flipX = false;
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
        {
            GetComponent<SpriteRenderer>().flipX = true;
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
        {
            // Set forward sprite
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
        {
            //Set backwards sprite
        }
        currentSpeed = Input.GetKeyDown(KeyCode.LeftShift) ? runSpeed : walkSpeed;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
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
