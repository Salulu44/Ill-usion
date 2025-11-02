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
    public event Action OnStartDialogue;
    public event Action OnWhileDialogue;
    public event Action OnEndDialogue;
    Dictionary<string, DialogueLine> dialogueDict;
    string currentDialogueID;
    StringBuilder currentText = new StringBuilder();
    bool isTyping = false;
    bool isDialogueFinished = true;
    float typeSpeed = 0.05f;
    float typeTimer = 0f;
    int NPCSeverityScore = 0;
    void Start()
    {
        currentDialogueID = dialogueAsset.dialogueLines[0].dialogueID;
        dialogueUI = GetComponent<DialogueUI>();
        BuildDialogueDictionary();
        SetButtons();
        GameManagerScript.Instance.decisionTimer.GetComponent<SliderScript>().OnTimerEnd += ClickAnyButton;
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
        dialogueDict.Add(dialogueAsset.severeLine.dialogueID, dialogueAsset.severeLine);
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
        OnStartDialogue?.Invoke();
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
        print(line.nextDialogueID);
        if (Input.GetKeyDown(KeyCode.F) && !isDialogueFinished && !isTyping && line.choices.Length == 0)
        {
            currentDialogueID = line.nextDialogueID;
            ShowDialogueLine(currentDialogueID);
        }
        if (Input.GetKeyDown(KeyCode.F) && line.nextDialogueID.ToUpper() == "END")
        {
            EndDialogue();
            OnEndDialogue?.Invoke();
        }
        if (isTyping)
            TypewriterTick();
    }
    void TypewriterTick()
    {
        if (!dialogueDict.TryGetValue(currentDialogueID, out DialogueLine line))
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
        NPCSeverityScore += choice.severity;
        if (NPCSeverityScore > severityLine)
        {
            currentDialogueID = dialogueAsset.severeLine.dialogueID;
            ShowDialogueLine(dialogueAsset.severeLine.dialogueID);
            HideChoices();
            return;
        }
        ShowDialogueLine(choice.nextDialogueID);
        currentDialogueID = choice.nextDialogueID;
    }
    void EndDialogue()
    {
        dialogueObject.SetActive(false);
        NPCSeverityScore = 0;
        isTyping = false;
        isDialogueFinished = true;
        currentDialogueID = dialogueAsset.dialogueLines[0].dialogueID;
    }
    void SetDialogueReferences(DialogueLine line)
    {
        dialogueUI.dialogueSprite.sprite = line.sprite;
        dialogueUI.nameText.text = line.speaker;
    }
}




