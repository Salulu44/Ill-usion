using UnityEngine;
public class LifebarEnemyScript : MonoBehaviour
{
    [SerializeField] RectTransform lifebarUI;  
    [SerializeField] Canvas canvas;
    [SerializeField] Transform playerTr;
    void Update()
    {
        Vector3 direction = new Vector3(playerTr.position.x - transform.position.x, playerTr.position.y - transform.position.y,0);
        direction = Vector3.Normalize(direction);
        transform.position += direction * Time.deltaTime;
    }
    private void Start()
    {

    }
    [ContextMenu(nameof(SetPosition))]
    public void SetPosition() 
    {
        if (canvas == null || lifebarUI == null) return;

        Vector3 worldPos = Camera.main.ScreenToWorldPoint(lifebarUI.position);
        worldPos.z = 0;
        transform.position = worldPos;
    }
    private void OnEnable()
    {
        SetPosition();
    }

}
