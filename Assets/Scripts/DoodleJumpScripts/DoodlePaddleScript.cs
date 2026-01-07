using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class DoodlePaddleScript : MonoBehaviour, IPointerEnterHandler
{
    RectTransform barRectTr;
    Rigidbody2D paddleRb;
    [SerializeField] float paddleSpeed;
    [SerializeField] float deadZone;
    bool entered;
    void Start()
    {
        barRectTr = GetComponent<RectTransform>();
        paddleRb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (entered)
        {
            Movement();
        }
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
    public void SetPaddle(bool on = true)
    {
        gameObject.SetActive(on);
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        entered = true;
        paddleRb.constraints = RigidbodyConstraints2D.FreezeRotation;
    }
}
