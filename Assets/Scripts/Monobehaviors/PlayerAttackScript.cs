using UnityEngine;

public class PlayerAttackScript : MonoBehaviour
{
    [SerializeField] float chokeDistance;
    [SerializeField] KeyCode chokeKey;
    [SerializeField] LayerMask chokeLayer;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        ChokeEnemies();
    }
    void ChokeEnemies()
    {
        ContactFilter2D filter = new ContactFilter2D();
        filter.useTriggers = false;
        filter.SetLayerMask(chokeLayer);
        filter.useLayerMask = true;
        RaycastHit2D[] results = new RaycastHit2D[1];
        int hitCount = Physics2D.Raycast(transform.position, transform.right, filter, results, chokeDistance);
        if (hitCount > 0)
        {
            //if (results[0].transform.gameObject is Enemy enemy)
            //{

            //}
            Debug.Log("Hit " + results[0].transform.gameObject.name);
        }
    }
}
