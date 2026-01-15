using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DoodleQuitScript : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] Sprite[] quitSprites;
    [SerializeField] DialogueScript quitMenuScript;
    [SerializeField] DialogueScript hitQuitButtonScript;
    Image quitRenderer;
    int spriteIndex;
    [SerializeField] GameObject lifeBar;
    void Start()
    {
        Debug.Log("Yeah");
        quitRenderer = GetComponent<Image>();
        quitMenuScript.OnWhileDialogue += CancelCurrentDialogueEvent;
        hitQuitButtonScript.OnEndDialogue += HitQuitButtonEvent;
    }

    private void HitQuitButtonEvent()
    {
        quitMenuScript.enabled = true;
       // hitQuitButtonScript.StartDialogue();
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
            hitQuitButtonScript.StartDialogue();
        }

    }

    private void CancelCurrentDialogueEvent()
    {
        if (!hitQuitButtonScript.IsDialogueFinished) 
        {
            quitMenuScript.enabled = false;
        }

    }
}
