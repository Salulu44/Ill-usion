using UnityEngine;
public class LifebarEnemyScript : Enemy
{
    [SerializeField] RectTransform lifebarUI;  
    [SerializeField] Canvas canvas;
    [SerializeField] Transform playerTr;
    protected override void Update()
    {
        //base.Update();
        EnemyAIMovement();
    }
    protected override void Start()
    {
        base.Start();
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
    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        base.OnCollisionEnter2D(collision);
    }

    protected override void EnemyAIMovement()
    {
        Vector3 direction = new Vector3(playerTr.position.x - transform.position.x, playerTr.position.y - transform.position.y, 0);
        direction = Vector3.Normalize(direction);
        transform.position += direction * Time.deltaTime * enemySpeed;
    }

    protected override void PlayEnemySound()
    {
        //When hit
    }

    protected override void Die()
    {
        //idk if he can die
    }
}
