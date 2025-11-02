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
        }
    }
    void RocketPowerup()
    {
        powerupTimer -= Time.deltaTime;
        if (powerupTimer < 0)
        {
            playerRb.linearVelocityY = playerRb.linearVelocityY * .25f;
            Destroy(gameObject);
        }
        playerRb.AddForce(Vector2.up * rocketForce);
    }
}
