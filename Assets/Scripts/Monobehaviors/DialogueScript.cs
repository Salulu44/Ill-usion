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
    [SerializeField] int severityLine;
    DialogueUI dialogueUI;
    Button[] choiceButtons = new Button[3];
    public event Action onStartDialogue;
    public event Action onWhileDialogue;
    public event Action onEndDialogue;
    Dictionary<string, DialogueLine> dialogueDict;
    string nextDialogueID;
    string currentDialogueID;
    StringBuilder currentText = new StringBuilder();
    bool isTyping = false;
    bool isDialogueFinished = true;
    float typeSpeed = 0.05f;
    float typeTimer = 0f;
    int playerSeverityScore = 0;
    void Start()
    {
        nextDialogueID = dialogueAsset.dialogueLines[0].dialogueID;
        currentDialogueID = nextDialogueID;
        dialogueUI = GetComponent<DialogueUI>();
        BuildDialogueDictionary();
        SetButtons();
        GameManagerScript.Instance.decisionTimer.GetComponent<SliderScript>().onTimerEnd += ClickAnyButton;
    }
    void ClickAnyButton() 
    {
        if(dialogueDict.TryGetValue(currentDialogueID, out DialogueLine line)) 
        {
            int index = UnityEngine.Random.Range(0, 3);
            choiceButtons[index].onClick.AddListener(() => OnChoiceSelected(line.choices[index]));
            choiceButtons[index].onClick.Invoke();
            choiceButtons[index].onClick.RemoveAllListeners();
        }
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
        ShowDialogueLine(currentDialogueID);
        onStartDialogue?.Invoke();
        isDialogueFinished = false;
    }
    void ShowDialogueLine(string dialogueID)
    {
        if (!dialogueDict.TryGetValue(dialogueID, out DialogueLine line))
        {
            Debug.LogWarning($"Keine Dialogzeile mit ID '{dialogueID}' gefunden.");
            //EndDialogue();
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
        DialogueCheck();
    }
    void DialogueCheck() 
    {
        dialogueDict.TryGetValue(currentDialogueID, out DialogueLine line);
        if (Input.GetKeyDown(KeyCode.F) && !isDialogueFinished && !isTyping && line.choices.Length == 0)
        {
            currentDialogueID = nextDialogueID;
            ShowDialogueLine(nextDialogueID);
        }
        if (Input.GetKeyDown(KeyCode.F) && nextDialogueID.ToUpper() == "END")
        {
            EndDialogue();
            onEndDialogue?.Invoke();
        }
        if (isTyping)
            TypewriterTick();
    }
    void TypewriterTick()
    {
        if (!dialogueDict.TryGetValue(nextDialogueID, out DialogueLine line))
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
                nextDialogueID = line.nextDialogueID;
                isTyping = false;
            }
        }
    }
    void ShowChoices(DialogueChoice[] choices)
    {
        GameManagerScript.Instance.decisionButtons.SetActive(true);
        GameManagerScript.Instance.decisionTimer.SetActive(true);
        for (int i = 0; i < choiceButtons.Length; i++)
        {
            int index = i;
            choiceButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = choices[i].choiceText;
            choiceButtons[i].onClick.RemoveAllListeners();
            choiceButtons[i].onClick.AddListener(() => OnChoiceSelected(choices[index]));   
        }
    }
    void HideChoices()
    {
        foreach (var btn in choiceButtons)
        {
            btn.onClick.RemoveAllListeners();
        }
        GameManagerScript.Instance.decisionButtons.SetActive(false);
        GameManagerScript.Instance.decisionTimer.SetActive(false);
    }
    void OnChoiceSelected(DialogueChoice choice)
    {
        playerSeverityScore += choice.severity;
        //if(playerSeverityScore > severityLine) 
        //{
        //    ShowDialogueLine(dialogueAsset.severityID);
        //    return;
        //}
        ShowDialogueLine(choice.nextDialogueID);
        nextDialogueID = choice.nextDialogueID;
        currentDialogueID = nextDialogueID;
    }
    void EndDialogue()
    {
        dialogueObject.SetActive(false);
        playerSeverityScore = 0;
        isTyping = false;
        isDialogueFinished = true;
        nextDialogueID = dialogueAsset.dialogueLines[0].dialogueID;
        currentDialogueID = nextDialogueID;
    }
    void SetDialogueReferences(DialogueLine line)
    {
        dialogueUI.dialogueSprite.sprite = line.sprite;
        dialogueUI.nameText.text = line.speaker;
    }
}




