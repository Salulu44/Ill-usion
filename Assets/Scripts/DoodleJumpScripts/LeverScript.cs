using UnityEngine;
public class LeverScript : MonoBehaviour
{
    [SerializeField] GameObject[] gameObjects;
    [SerializeField] Vector3[] directions;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.tag == GameManagerScript.Instance.tagSO.doodlePlayerBulletTag)
        {
            GetComponent<SpriteRenderer>().color = Color.red;
            //Implement Trigger
            if(gameObjects.Length == 0) 
            {
                Debug.Log("You have not assigned objects into the inspector Daddy!");
                return;
            }
            foreach (GameObject platform in gameObjects)
            {
               //Move the Platform with Dottween
            }
        }
    }
}
