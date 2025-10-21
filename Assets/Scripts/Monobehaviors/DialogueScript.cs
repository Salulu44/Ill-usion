using System.Collections;
using System.Text;
using UnityEngine;
[RequireComponent(typeof(DialogueUI),typeof(AudioSource))]
public class DialogueScript : MonoBehaviour
{
    [SerializeField] Dialogue dialogue;
    DialogueUI dialogueUI;
    private int lineIndex;
    [SerializeField] GameObject dialogueObject;
    private Coroutine coroutine;
    private bool skippingDialogue;
    void Start()
    {
        dialogueUI = GetComponent<DialogueUI>();
        print(dialogue.dialogueLines.Length);
    }

    // Update is called once per frame
    void Update()
    {
        if (lineIndex == dialogue.dialogueLines.Length -1 && Input.GetKeyDown(KeyCode.F))
        {
            StopAllCoroutines();
            lineIndex = 0;
            print("ist over");
            dialogueUI.textbox.text = string.Empty;
            dialogueObject.SetActive(false);
        }
        //if (Input.GetKeyDown(KeyCode.F))
        //{
        //  if(coroutine != null) 
        //    {
        //        skippingDialogue = true;
        //    }
        //}
    }
    public IEnumerator RunDialogue()
    {
        foreach (DialogueLine line in dialogue.dialogueLines)
        {
            print(line.textContent);
          coroutine =  StartCoroutine(TypeText(line.textContent, .05f));
            yield return coroutine;
           yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.F));
            lineIndex++;
        } 
    }
    IEnumerator TypeText(string lineContent,float typeSpeed) 
    {
        StringBuilder stringBuilder = new StringBuilder();
        SetDialogueReferences();
        foreach (char letter in lineContent) 
        {
            if (skippingDialogue) 
            {
                skippingDialogue = false;
                coroutine = null;
                break;
            }
            AudioManagerScript.Instance.PlayDialogue(dialogue.dialogueLines[lineIndex].audioClip, dialogue.dialogueLines[lineIndex].AudioVolume, 1);
            stringBuilder.Append(letter);
            dialogueUI.textbox.text = stringBuilder.ToString();
            yield return new WaitForSeconds(typeSpeed);
        }

    }
    void SetDialogueReferences() 
    {
        dialogueUI.dialogueSprite.sprite = dialogue.dialogueLines[lineIndex].sprite;
        dialogueUI.nameText.text = dialogue.dialogueLines[lineIndex].speaker;
    }
}
