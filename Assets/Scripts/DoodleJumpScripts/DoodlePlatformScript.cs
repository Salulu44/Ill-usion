using System;
using UnityEngine;

public class DoodlePlatformScript : MonoBehaviour
{
    public enum PlatformState
    {
        Static = 0,
        Moving = 1,
        Spinning = 2,
        CrackOnce = 3,
    }
    [SerializeField] private float jumpForce;
    [SerializeField] private PlatformState platformState;
    private Vector2 moveDirection;
    [SerializeField] private float moveAmplitude;
    [SerializeField] private float rotationAmplifier;
    [SerializeField] private Sprite[] sprites;
    private bool hasTouchedOnce;
    void Start()
    {

    }
    private void OnEnable()
    {

    }
    public void SetMoveDirection(Vector2 direction) 
    {
        moveDirection = direction;
    }
    // Update is called once per frame
    void Update()
    {
        switch (platformState)
        {
            case PlatformState.Moving:
                Move();
                break;
            case PlatformState.Spinning:
                Spin();
                break;
            case PlatformState.CrackOnce:
                if (CheckTouchedOnce()) Destroy(gameObject);
                break;

        }
    }
    bool CheckTouchedOnce() => hasTouchedOnce;
    private void Spin()
    {
        float zRotation = Mathf.Sin(Time.time) * rotationAmplifier;
        transform.eulerAngles = new Vector3(0, 0, zRotation);
    }

    public void SetRandomPlatformState()
    {
        int length = Enum.GetNames(typeof(PlatformState)).Length;
        int randomNumber = UnityEngine.Random.Range(0, length);
        platformState = (PlatformState)randomNumber;
        //if (platformState == PlatformState.Moving)
        //{
        //    GetComponent<SpriteRenderer>().sprite = sprites[0];
        //}
        //else if (platformState == PlatformState.Spinning)
        //{
        //    GetComponent<SpriteRenderer>().sprite = sprites[1];
        //}
        //else if (platformState == PlatformState.CrackOnce)
        //{
        //    GetComponent<SpriteRenderer>().sprite = sprites[2];
        //}
    }
    private void Move()
    {
        float sinValue = Mathf.Sin(Time.time) * moveAmplitude;
        transform.position += ((Vector3)moveDirection * sinValue) * Time.deltaTime;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.tag == "Player" && collision.relativeVelocity.y <= 0)
        {
            hasTouchedOnce = true;
            Rigidbody2D playerRb = collision.transform.GetComponent<Rigidbody2D>();
            if (playerRb != null)
            {
                playerRb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            }
        }
    }
}
