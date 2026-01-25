using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(BoxCollider2D))]
public class DoodleZoomTriggerScript : MonoBehaviour
{
    [SerializeField] float zoomSize;
    [SerializeField] float zoomInDuration = 1f;
    [SerializeField] float zoomOutDuration = 1f;
    float originalSize;

    void Start()
    {
        originalSize = Camera.main.orthographicSize;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(GameManagerScript.Instance.tagSO.playerTag))
        {
            Camera.main.DOOrthoSize(zoomSize, zoomInDuration)
                .SetEase(Ease.InOutQuad); // smooth ein/aus
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag(GameManagerScript.Instance.tagSO.playerTag))
        {
            Camera.main.DOOrthoSize(originalSize, zoomOutDuration)
                .SetEase(Ease.InOutQuad);
        }
    }
}
