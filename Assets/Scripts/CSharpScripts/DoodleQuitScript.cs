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
     //   quitMenuScript.OnWhileDialogue += CancelCurrentDialogueEvent;
        hitQuitButtonScript.OnEndDialogue += HitQuitButtonEvent;
    }

    private void HitQuitButtonEvent()
    {
        Debug.Log("End of dialogue");
        quitMenuScript.enabled = true;
        quitMenuScript.StartDialogue();
       // hitQuitButtonScript.StartDialogue();
    }

    // Update is called once per frame
    void Update()
    {
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        if(spriteIndex < quitSprites.Length) 
        {
            quitRenderer.sprite = quitSprites[spriteIndex++];
            hitQuitButtonScript.enabled = true;
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
