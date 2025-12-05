using UnityEngine;
[RequireComponent(typeof(Rigidbody2D))]
public class TestScript2 : MonoBehaviour
{
    Rigidbody2D testRb;
    void Start()
    {
        testRb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (testRb.linearVelocityY < 0)
        {
            Debug.Log("Kleiner");
        }
        if (Input.GetKeyDown(KeyCode.O)) 
        {

           testRb.AddForce(Vector2.up * 10,ForceMode2D.Impulse);
        }
    }
}
