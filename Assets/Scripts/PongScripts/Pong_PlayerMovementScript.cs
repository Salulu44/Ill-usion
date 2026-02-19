using UnityEngine;

public class Pong_PlayerMovementScript : MonoBehaviour
{
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void Bounce(Collision2D collider)
    {
        float ballX = collider.transform.position.x;
        float paddleX = transform.position.x;
        float halfWidth = collider.collider.bounds.size.x * 0.5f;

        // -1 (linke Ecke) bis +1 (rechte Ecke)
        float t = Mathf.Clamp((ballX - paddleX) / halfWidth, -1f, 1f);

        // Basiswinkel in Grad relativ zur Senkrechten
        float maxAngle = 60f;
        float angleDeg = t * maxAngle;

        // Richtung aus Winkel bauen (immer nach oben)
        float angleRad = angleDeg * Mathf.Deg2Rad;
        Vector2 dir = new Vector2(Mathf.Sin(angleRad), Mathf.Cos(angleRad)).normalized;

        collider.transform.gameObject.GetComponent<Rigidbody2D>().AddForce(dir * 5, ForceMode2D.Impulse);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        //Ball Bounce Ca´ll
    }
}
