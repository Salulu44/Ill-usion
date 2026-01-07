using Unity.VisualScripting;
using UnityEngine;

public class DoodlePaddleScript : MonoBehaviour
{
    RectTransform barRectTr;
    Rigidbody2D paddleRb;
    [SerializeField] float paddleSpeed;
    [SerializeField] float deadZone;
    void Start()
    {
        barRectTr = GetComponent<RectTransform>();
        paddleRb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Movement();
    }
    void Movement()
    {
        if (Vector2.Distance(transform.position,Input.mousePosition) < deadZone)
        {
            paddleRb.linearVelocity = Vector2.zero;
            return;
        }
        Vector2 direction = (Input.mousePosition - transform.position).normalized;
        paddleRb.linearVelocity = direction * paddleSpeed;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log(collision.gameObject.name);
    }
}
