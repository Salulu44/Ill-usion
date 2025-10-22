//using System;
//using System.Collections;
//using System.Collections.Generic;
//using System.Text;
//using TMPro;
//using UnityEngine;
//using UnityEngine.UI;
//[RequireComponent(typeof(DialogueUI),typeof(AudioSource))]
//public class DialogueScript : MonoBehaviour
//{
//    [SerializeField] GameObject dialogueObject;
//    [SerializeField] Dialogue dialogue;
//    DialogueUI dialogueUI;
//    private Button[] decisionButtons = new Button[3];
//    public event Action onStartDialogue;
//    public event Action onWhileDialogue;
//    public event Action onEndDialogue;
//    private int lineIndex;
//    private float typeTimer;
//    private StringBuilder currentText = new StringBuilder();

//    private bool isTyping;
//    private bool dialogueFinished = true;
//    private float typeSpeed = 0.05f;
//    void Start()
//    {
//        dialogueUI = GetComponent<DialogueUI>();
//        SetAllButtons();
//    }
//    // Update is called once per frame
//    void Update()
//    {
//        DialogueCheck();
//    }
//    void DialogueCheck() 
//    {
//        if (Input.GetKeyDown(KeyCode.F) && !dialogueFinished)
//        {
//            if (!dialogue.dialogueLines[lineIndex].hasDecision) 
//            {
//                if (!isTyping)
//                {
//                    onWhileDialogue?.Invoke();
//                    StartNextLine();
//                }
//                else
//                {
//                    onWhileDialogue?.Invoke();
//                    lineIndex++;
//                    StartNextLine();
//                }
//            }
//            else 
//            {
//                if (!GameManagerScript.Instance.decisionButtons.activeSelf) 
//                {
//                    GameManagerScript.Instance.decisionButtons.SetActive(true);
//                    SetButtonText(dialogue.dialogueLines[lineIndex].choices);
//                }
//            }
//        }
//        if (isTyping)
//        {
//            TickTypewriter();
//        }
//        //if (dialogue.dialogueLines[lineIndex].hasDecision && GameManagerScript.Instance.decisionButtons.activeSelf) 
//        //{

//        //}
//    }
//    void SetButtonText(DialogueChoice[] choices) 
//    {
//        for (int i = 0; decisionButtons.Length > i; i++)
//        {
//            decisionButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = choices[i].choiceText;
//            decisionButtons[i].onClick.AddListener( () => OnChoiceSelected(choices[i]));
//        }
//    }
//    void OnChoiceSelected(DialogueChoice choice)
//    {
//        //playerSeverityScore += choice.severity;
//        //lineIndex = choice.nextDialogueIndex;
//    }
//    private void SetAllButtons() 
//    {

//        for(int i = 0; i < decisionButtons.Length; i++) 
//        {
//            if(GameManagerScript.Instance.decisionButtons.transform.GetChild(i).TryGetComponent(out Button button)) 
//            {
//                decisionButtons[i] = button;
//            }

//        }
//    }
//    public void StartDialogue()
//    {
//        dialogueFinished = false;
//        onStartDialogue?.Invoke();
//    }
//    void EndDialogue() 
//    {
//        onEndDialogue.Invoke();
//        dialogueObject.SetActive(false);
//        lineIndex = 0;
//        currentText.Clear();
//        dialogueUI.textbox.text = "";
//        dialogueFinished = true;
//    }
//    void StartNextLine()
//    {
//        currentText.Clear();
//        dialogueUI.textbox.text = "";
//        if (lineIndex >= dialogue.dialogueLines.Length)
//        {
//            dialogueFinished = true;
//            isTyping = false;
//            EndDialogue();
//            return;
//        }
//        DialogueLine line = dialogue.dialogueLines[lineIndex];
//        SetDialogueReferences(line);
//        typeTimer = 0f;
//        isTyping = true;
//    }
//    void TickTypewriter()
//    {
//        DialogueLine line = dialogue.dialogueLines[lineIndex];
//        string text = line.textContent;
//        typeTimer -= Time.deltaTime;
//        if (typeTimer <= 0)
//        {
//            int nextChar = currentText.Length;
//            if (nextChar < text.Length)
//            {
//                currentText.Append(text[nextChar]);
//                dialogueUI.textbox.text = currentText.ToString();
//                AudioManagerScript.Instance.PlayDialogue(line.audioClip, line.AudioVolume, 1);
//                typeTimer = typeSpeed;
//            }
//            else
//            {
//                isTyping = false;
//                lineIndex++;
//            }
//        }
//    }
//    void SetDialogueReferences(DialogueLine line)
//    {
//        dialogueUI.dialogueSprite.sprite = line.sprite;
//        dialogueUI.nameText.text = line.speaker;
//    }
//}

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text;
using System;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    [SerializeField] Dialogue dialogueAsset;
    [SerializeField] GameObject dialogueObject;
    private int dialogueCounter;
    DialogueUI dialogueUI;
    Button[] choiceButtons = new Button[3];
    public event Action onStartDialogue;
    public event Action onWhileDialogue;
    public event Action onEndDialogue;
    Dictionary<string, DialogueLine> dialogueDict;
    string currentDialogueID;
    StringBuilder currentText = new StringBuilder();
    bool isTyping = false;
    bool isDialogueFinished = true;
    float typeSpeed = 0.05f;
    float typeTimer = 0f;
    int playerSeverityScore = 0;
    void Start()
    {
        BuildDialogueDictionary();
        dialogueUI = GetComponent<DialogueUI>();
        SetButtons();
    }
    void BuildDialogueDictionary()
    {
        dialogueDict = new Dictionary<string, DialogueLine>();
        foreach (DialogueLine line in dialogueAsset.dialogueLines)
        {
            if (dialogueDict.ContainsKey(line.dialogueID))
                Debug.LogWarning($"Duplicate dialogueID: {line.dialogueID}");
            else
                dialogueDict.Add(line.dialogueID, line);
        }
    }
    private void SetButtons() 
    {
        for (int i = 0; i < choiceButtons.Length; i++)
        {
            if (GameManagerScript.Instance.decisionButtons.transform.GetChild(i).TryGetComponent(out Button button))
            {
                choiceButtons[i] = button;
            }
        }
    }
    public void StartDialogue() 
    {
        currentDialogueID = "Start";
        ShowDialogueLine("Start");
        onStartDialogue?.Invoke();
        isDialogueFinished = false;
    }
    void ShowDialogueLine(string dialogueID)
    {
        if (!dialogueDict.TryGetValue(dialogueID, out DialogueLine line))
        {
            Debug.LogWarning($"Keine Dialogzeile mit ID '{dialogueID}' gefunden.");
            EndDialogue();
            return;
        }

        dialogueUI.textbox.text = "";
        currentText.Clear();
        SetDialogueReferences(line);
        isTyping = true;
        typeTimer = 0f;
        if (line.choices != null && line.choices.Length > 0)
            ShowChoices(line.choices);
        else
            HideChoices();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F) && !isDialogueFinished && !isTyping)
        {
            ShowDialogueLine(currentDialogueID);
        }
        if(Input.GetKeyDown(KeyCode.F) && currentDialogueID == "End") 
        {
            EndDialogue(); ;
            dialogueCounter = 0;
            onEndDialogue?.Invoke();
        }
        if (isTyping)
            TypewriterTick();
    }

    void TypewriterTick()
    {
        if (!dialogueDict.TryGetValue(currentDialogueID, out var line))
            return;

        string fullText = line.textContent;
        typeTimer -= Time.deltaTime;

        if (typeTimer <= 0)
        {
            if (currentText.Length < fullText.Length)
            {
                currentText.Append(fullText[currentText.Length]);
                dialogueUI.textbox.text = currentText.ToString();
                if (line.audioClip != null)
                    AudioManagerScript.Instance.PlayDialogue(line.audioClip, line.AudioVolume, 1);
                typeTimer = typeSpeed;
            }
            else
            {
                currentDialogueID = line.nextDialogueID;
                isTyping = false;
                dialogueCounter++;
            }
        }
    }

    void ShowChoices(DialogueChoice[] choices)
    {
        GameManagerScript.Instance.decisionButtons.SetActive(true);
        for (int i = 0; i < GameManagerScript.Instance.decisionButtons.transform.childCount; i++)
        {
            if(GameManagerScript.Instance.decisionButtons.transform.TryGetComponent(out Button button)) 
            {
                int index = i;
                choiceButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = choices[i].choiceText;
                choiceButtons[i].onClick.RemoveAllListeners();
                print("Index " + i);
                print("CHoice length " + choices.Length);

                choiceButtons[i].onClick.AddListener(() => OnChoiceSelected(choices[index]));
            }
        }
    }
    void HideChoices()
    {
        foreach (var btn in choiceButtons)
        {
            btn.gameObject.SetActive(false);
            btn.onClick.RemoveAllListeners();
        }
    }

    void OnChoiceSelected(DialogueChoice choice)
    {
        playerSeverityScore += choice.severity;
        ShowDialogueLine(choice.nextDialogueID);
    }

    void EndDialogue()
    {
        dialogueObject.SetActive(false);
        Debug.Log($"Dialog beendet. Severity Score: {playerSeverityScore}");
        playerSeverityScore = 0;
        isTyping = false;
    }

    void SetDialogueReferences(DialogueLine line)
    {
        dialogueUI.dialogueSprite.sprite = line.sprite;
        dialogueUI.nameText.text = line.speaker;
    }
}




