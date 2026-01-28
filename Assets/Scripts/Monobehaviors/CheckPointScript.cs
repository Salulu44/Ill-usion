using UnityEngine;

public class CheckPointScript : MonoBehaviour
{
    static GameObject currentCheckPoint;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.transform.tag == GameManagerScript.Instance.tagSO.playerTag)
        {
            //if (currentCheckPoint == null || currentCheckPoint != gameObject)
            //{
                IRespawnable iRespawnable = collision.transform.gameObject.GetComponent<IRespawnable>();
                if(iRespawnable != null)
                {
                    iRespawnable.SetRespawnPoint(transform.position);
                }
                //checkpoint anim
                //checkPointPanel.SetActive(true);
                //checkPointAnimator.SetTrigger("CheckPoint");
                //checkPointAnimationLength = checkPointAnimationLengthTmp;

                currentCheckPoint = gameObject;
            //}
        }
    }
}
