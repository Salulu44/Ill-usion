using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text;
using System;
using TMPro;
[RequireComponent(typeof(DialogueUI))]
public class DialogueScript : MonoBehaviour
{
    [SerializeField] Dialogue[] dialogueAsset;
    [SerializeField] GameObject dialogueObject;
    [SerializeField] int severityLine;
    [SerializeField] bool onlyText;
    [SerializeField] bool shouldSkipWithoutPressing;
    [SerializeField] bool changeUIPosition;
    [SerializeField] bool playOnlyOneLine;
    [SerializeField] Vector3 textPosition;
    [SerializeField] TextAlignmentOptions textAlignment;
    [SerializeField] float typeSpeed = 0.05f;
    int dialogueAssetIndex;
    bool skipDialogue;
    DialogueUI dialogueUI;
    Button[] choiceButtons = new Button[3];
    public event Action OnStartDialogue;
    public event Action OnWhileDialogue;
    public event Action OnEndDialogue;
    Dictionary<string, DialogueLine> dialogueDict;
    string currentDialogueID;
    StringBuilder currentText = new StringBuilder();
    bool isTyping = false;
    private bool isDialogueFinished = true;
    public bool IsDialogueFinished { get => isDialogueFinished; private set => isDialogueFinished = value; }
    float typeTimer = 0f;
    int NPCSeverityScore = 0;
    void Start()
    {
        currentDialogueID = dialogueAsset[dialogueAssetIndex].dialogueLines[0].dialogueID;
        dialogueUI = GetComponent<DialogueUI>();
        if (dialogueUI.dialogueSprite == null)
        {
            SearchUIReferences();
        }
        BuildDialogueDictionary();
        SetButtons();
        if(dialogueObject == null) 
        {
            dialogueObject = GameObject.FindWithTag(GameManagerScript.Instance.tagSO.dialogueTag).transform.GetChild(0).gameObject;

        }
        GameManagerScript.Instance.decisionTimer.GetComponent<SliderScript>().OnTimerEnd += ClickAnyButton;
    }
    void SearchUIReferences() 
    {
        DialogueUI dialogueUIRefernces = GameObject.FindWithTag(GameManagerScript.Instance.tagSO.dialogueUITag).GetComponent<DialogueUI>();
        dialogueUI.SetReferences(dialogueUIRefernces);
    }
    void ClickAnyButton() 
    {
        if(dialogueDict.TryGetValue(currentDialogueID, out DialogueLine line)) 
        {

            if (!isDialogueFinished) 
            {
                Debug.Log("This is the current DialogueId " + currentDialogueID + "gameObject " + gameObject.name);
                int index = UnityEngine.Random.Range(0, 3);
                try
                {
                    choiceButtons[index].onClick.Invoke();
                    choiceButtons[index].onClick.RemoveAllListeners();
                }
                catch (IndexOutOfRangeException range)
                {
                    Debug.Log(range.Message + " index " + index);
                    Debug.Log("thats the length of choices " + line.choices.Length);
                }
            }
        }
    }
    void BuildDialogueDictionary()
    {
        dialogueDict = new Dictionary<string, DialogueLine>();
        foreach (Dialogue dialogue in dialogueAsset) 
        {
            foreach (DialogueLine line in dialogue.dialogueLines)
            {
                if (dialogueDict.ContainsKey(line.dialogueID))
                    Debug.LogWarning($"Duplicate dialogueID: {line.dialogueID}");
                else
                    dialogueDict.Add(line.dialogueID, line);
            }
        }
        dialogueDict.Add(dialogueAsset[dialogueAssetIndex].severeLine.dialogueID, dialogueAsset[dialogueAssetIndex].severeLine);
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
    public void CloseCanvas() 
    {
       dialogueObject.SetActive(false);
       typeTimer = typeSpeed;
    }
    public void StartDialogue() 
    {
        ShowDialogueLine(currentDialogueID);
        dialogueObject.SetActive(true);
        Transform textTr = null;
        if (onlyText)
        {
            foreach (Transform child in dialogueObject.transform)
            {
                if (child.gameObject != dialogueUI.textbox.gameObject)
                {
                    child.gameObject.SetActive(false);
                }
                textTr = child;
            }
        }
        if (changeUIPosition) 
        {
            textTr.localPosition = textPosition;
            textTr.gameObject.GetComponent<TextMeshProUGUI>().alignment = textAlignment;
        }
        OnStartDialogue?.Invoke();
        isDialogueFinished = false;
    }
    public void SetNextLine()
    {
        if(dialogueDict.TryGetValue(currentDialogueID,out DialogueLine dialogueLine))
        {
            Debug.Log("NEXT LINE");
            currentDialogueID = dialogueLine.nextDialogueID;
            currentText.Clear();
            if (currentDialogueID.ToUpper() == "END") 
            {
                EndDialogue();
            }
        }
    }
    void ShowDialogueLine(string dialogueID)
    {
        if (!dialogueDict.TryGetValue(dialogueID, out DialogueLine line))
        {
            Debug.LogWarning($"no Dialogueline with ID '{dialogueID}' found.");
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
    public void PlayOneDialogueLine() 
    {

    }
    void Update()
    {
        DialogueCheck();
    }
    void DialogueCheck() 
    {
        dialogueDict.TryGetValue(currentDialogueID, out DialogueLine line);
        if (((Input.GetKeyDown(KeyCode.F) || Input.GetKeyDown(KeyCode.Mouse0) )&& !isDialogueFinished && !isTyping && line.choices.Length == 0) || skipDialogue)
        {
            Debug.Log("SkipDialogue");
            currentDialogueID = line.nextDialogueID;
            ShowDialogueLine(currentDialogueID);

        }
        if ((Input.GetKeyDown(KeyCode.F) || Input.GetKeyDown(KeyCode.Mouse0)) && line.nextDialogueID.ToUpper() == "END" || line.nextDialogueID.ToUpper() == "END" && skipDialogue)
        {
            Debug.Log("End Dialogue " + line.dialogueID);

            EndDialogue();
        }
        if (isTyping)
            TypewriterTick();
    }
    void TypewriterTick()
    {
        if (!dialogueDict.TryGetValue(currentDialogueID, out DialogueLine line))
            return;
        OnWhileDialogue?.Invoke();
        skipDialogue = false;
        string fullText = line.textContent;
        typeTimer -= Time.unscaledDeltaTime;
        if (typeTimer <= 0)
        {
            typeTimer = typeSpeed;
            if (currentText.Length < fullText.Length)
            {
                currentText.Append(fullText[currentText.Length]);
                dialogueUI.textbox.text = currentText.ToString();
                if (line.audioClip != null)
                    AudioManagerScript.Instance.PlayDialogue(line.audioClip, line.AudioVolume, 1);
            }
            else
            {
                isTyping = false;
                if (shouldSkipWithoutPressing) 
                {
                    skipDialogue = true;
                }
                if (playOnlyOneLine) 
                {
                    if(line.nextDialogueID == "END") 
                    {
                        Debug.Log("ENDDDD");
                        EndDialogue();
                        return;
                    }
                    isDialogueFinished = true;
                    dialogueObject.SetActive(false);
                    enabled = false;
                }
            }

        }
    }
    void ShowChoices(DialogueChoice[] choices)
    {

        GameManagerScript.Instance.decisionButtons.SetActive(true);
        GameManagerScript.Instance.decisionTimer.SetActive(true);
        GameManagerScript.Instance.decisionTimer.GetComponent<SliderScript>().ResetSlider();
        for (int i = 0; i < choices.Length; i++)
        {
            int index = i;
            choiceButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = choices[i].choiceText;
            choiceButtons[i].onClick.RemoveAllListeners();
            Debug.Log("Remove");
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
            Debug.Log("Severe");
            currentDialogueID = dialogueAsset[dialogueAssetIndex].severeLine.dialogueID;
            ShowDialogueLine(dialogueAsset[dialogueAssetIndex].severeLine.dialogueID);
            return;
        }
        HideChoices();
        ShowDialogueLine(choice.nextDialogueID);
        currentDialogueID = choice.nextDialogueID;
        Debug.Log("I have chosen");
    }
    void EndDialogue()
    {
        if (onlyText) 
        {
            foreach(Transform child in dialogueObject.transform) 
            {
                child.gameObject.SetActive(true);
            }
        }
        dialogueAssetIndex++;
        if(dialogueAssetIndex >= dialogueAsset.Length) 
        {
            dialogueAssetIndex = 0;
        }
        NPCSeverityScore = 0;
        isTyping = false;
        isDialogueFinished = true;
        currentDialogueID = dialogueAsset[dialogueAssetIndex].dialogueLines[0].dialogueID;
        skipDialogue = false;
        dialogueObject.SetActive(false);
        OnEndDialogue?.Invoke();
    }
    void SetDialogueReferences(DialogueLine line)
    {
        if(dialogueUI.dialogueSprite.sprite == null || dialogueUI.nameText == null) 
        {
           // Debug.Log("I have not assigned the references, maybe it is a OnlyText-dialogue, check it");
            return;
        }
        dialogueUI.dialogueSprite.sprite = line.sprite;
        dialogueUI.nameText.text = line.speaker;
    }
}




