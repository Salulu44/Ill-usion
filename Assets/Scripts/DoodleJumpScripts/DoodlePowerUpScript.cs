using UnityEngine;

public class DoodlePowerUpScript : MonoBehaviour
{
    public enum PowerupState
    {
        Rocket
    }
    [SerializeField] private float rocketForce;
    [SerializeField] private float powerupTimer;
    private float powerupTimerTmp;
    private PowerupState powerupState;
    private Rigidbody2D playerRb;
    private bool wasTouched;
    private Collider2D playerCollider;
    [HideInInspector]public Vector2 forceDirection = Vector2.up;
    void Start()
    {
        powerupTimerTmp = powerupTimer;
    }

    // Update is called once per frame
    void Update()
    {
        if (wasTouched)
            switch (powerupState)
            {
                case PowerupState.Rocket:
                    RocketPowerup();
                    break;
            }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.tag == "Player")
        {
            playerRb = collision.GetComponent<Rigidbody2D>();
            wasTouched = true;
            GetComponent<Collider2D>().enabled = false;
            GetComponent<SpriteRenderer>().enabled = false;
            playerCollider = collision;
            playerCollider.enabled = false;
        }
    }
    void RocketPowerup()
    {
        powerupTimer -= Time.deltaTime;
        if (powerupTimer < 0)
        {
            playerRb.linearVelocityY = playerRb.linearVelocityY * .25f;
            playerCollider.enabled = true;
            Destroy(gameObject);
        }
        playerRb.AddForce(Vector2.up * rocketForce);
    }
}
