using System.Net;
using DG.Tweening;
using UnityEngine;

public class DoodleSpecialWallScript : MonoBehaviour
{
    Transform doodlePlayerTr;
    [SerializeField] float distance;
    [SerializeField] Vector2 endPointOffset;
    [SerializeField] float travelDuration;
    bool isMoving;
    Vector2 startPoint;
    Vector3 endPoint;
    void Start()
    {
        startPoint = transform.position;
        endPoint += transform.position + (Vector3)endPointOffset;
    }

    // Update is called once per frame
    void Update()
    {
        CheckPlayerInRange();
        CheckPlayerDistance();
    }
    void CheckPlayerInRange()
    {
       Collider2D[] colliders =  Physics2D.OverlapCircleAll(startPoint, 4f);
        foreach (Collider2D collider in colliders) 
        {
            if(collider.transform.tag == GameManagerScript.Instance.tagSO.playerTag) 
            {
                doodlePlayerTr = collider.transform;
                return;
            }
        }
        doodlePlayerTr = null;
    }
    void CheckPlayerDistance()
    {   if (doodlePlayerTr == null) return;
        if (Vector3.Distance(doodlePlayerTr.position, transform.position) < distance)
        {

            if (!isMoving)
            {
                isMoving = true;
                Sequence sequence = DOTween.Sequence();
                sequence.Append(transform.DOMove(endPoint, travelDuration));

                sequence.Append(transform.DOMove(startPoint, travelDuration * 0.5f));
                sequence.OnComplete(() => isMoving = false);
            }
        }
    }
}
