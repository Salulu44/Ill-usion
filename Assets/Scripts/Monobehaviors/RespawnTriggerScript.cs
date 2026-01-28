using UnityEngine;
[RequireComponent(typeof(BoxCollider2D))]
public class RespawnTriggerScript : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.transform.tag == GameManagerScript.Instance.tagSO.playerTag)
        {
            IRespawnable iRespawnable = collision.gameObject.GetComponent<IRespawnable>();
            if(iRespawnable != null)
            {
                iRespawnable.Respawn();
            }
        }
    }
}
