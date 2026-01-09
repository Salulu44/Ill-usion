using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DoodleQuitScript : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] Sprite[] quitSprites;
    Image quitRenderer;
    int spriteIndex;
    [SerializeField] GameObject lifeBar;
    void Start()
    {
        quitRenderer = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.O)) 
        {
            Destroy(lifeBar);
        }
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        if(spriteIndex < quitSprites.Length) 
        {
            quitRenderer.sprite = quitSprites[spriteIndex++];
        }

    }
}
