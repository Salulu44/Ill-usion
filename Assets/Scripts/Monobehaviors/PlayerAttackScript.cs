using UnityEngine;

public class PlayerAttackScript : MonoBehaviour
{
    [SerializeField] float chokeDistance;
    [SerializeField] KeyCode chokeKey;
    [SerializeField] LayerMask chokeLayer;
    bool hasPressedChokeButton;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        PressedChoke();
    }
    private void FixedUpdate()
    {
        ChokeEnemies();
    }
    void PressedChoke()
    {
        if (Input.GetKeyDown(chokeKey))
        {
            hasPressedChokeButton = true;
        }
        else if (Input.GetKeyUp(chokeKey))
        {
            hasPressedChokeButton = false;
        }
    }
    void ChokeEnemies()
    {
        ContactFilter2D filter = new ContactFilter2D();
        filter.useTriggers = false;
        filter.SetLayerMask(chokeLayer);
        filter.useLayerMask = true;
        RaycastHit2D[] results = new RaycastHit2D[1];
        int hitCount = Physics2D.Raycast(new Vector3(transform.position.x +.5f ,transform.position.y,transform.position.z), transform.right, filter, results, chokeDistance);
        if (hitCount > 0)
        {
            Debug.Log(results[0].transform.gameObject.name);
            if (results[0].transform.gameObject.tag == GameManagerScript.Instance.tagSO.enemyTag) 
            {
               
                if (hasPressedChokeButton && Vector3.Dot(results[0].transform.right,transform.right) > 0)
                {
                    results[0].transform.gameObject.SetActive(false);
                }
                else
                {
                    Debug.Log("Du hast den ChokeKey nicht gedrückt " + results[0].transform.gameObject.name);
                }
                Debug.Log("Dot Product " + Vector3.Dot(results[0].transform.right, transform.right));
            }
         
        }
    }
}
