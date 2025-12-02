using UnityEngine;

public class LifebarEnemyScript : MonoBehaviour
{
    [SerializeField] Transform lifebarTr;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 ScreenPosition = Camera.main.WorldToScreenPoint(new Vector3(lifebarTr.position.x, lifebarTr.position.y, 0)); 
        Vector3 ScreenPositionToWorld =Camera.main.ScreenToWorldPoint(new Vector3(ScreenPosition.x, Screen.height, 0));
        transform.position = new Vector3(ScreenPositionToWorld.x,ScreenPositionToWorld.y,0);
    }
}
