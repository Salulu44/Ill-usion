//using System;
//using System.Collections;
//using System.Text;
//using Unity.VisualScripting;
//using UnityEngine;
//using UnityEngine.SceneManagement;
//using UnityEngine.UI;
//[System.Serializable]
//[RequireComponent(typeof(DialogueUI))]
//public class DialogueImprovedScipt : MonoBehaviour
//{
//    public enum DialogueState
//    {
//        Nothing,
//        Talking,
//        Over,
//        Typing,
//        Transition,
//        Audio,
//        TextWithoutInteraction
//    }
//    private DialogueState dialogueState;
//    [SerializeField] private bool isReplayable;
//    [SerializeField] private bool isTransitioning;
//    [SerializeField] private bool isUsingDecision;
//    private bool isTalking;
//    private bool isButtonPressed = true;
//    private bool isPlayable = true;
//    private bool isFirstLine;
//    private bool isTyping;
//    public static bool isMomTalkFinished;
//    private DialogueUI dialogueUI;
//    public event Action OnDialogueStart;
//    public event Action OnDialogueOver;
//    [SerializeField] private Dialogue dialogues;

//    private int lineIndex;
//    private int charIndex;
//    [SerializeField] private float textTypeTimer;
//    [SerializeField] private float transitionTypeTimer = .1f;
//    private float textTypeTimerTmp;
//    [SerializeField] private float pitch;
//    private bool isReadyShaking = true;

//    [SerializeField] private Vector2 moveAwayDirection;
//    private StringBuilder stringBuilder = new StringBuilder();

//    private AudioClip previousTheme;
//    private float audioTime;
//    private float audioVolumeTmp;
//    void Start()
//    {
//        dialogues.dialogueLines = new DialogueLine[20];
//        textTypeTimerTmp = textTypeTimer;
//        dialogueUI = GetComponent<DialogueUI>();
//        if (dialogues.dialogueLines.Length != 0)
//        {
//            dialogueUI.audioSource.volume = dialogues.dialogueLines[lineIndex].AudioVolume;
//            if (dialogues.dialogueLines[lineIndex].hasSpecialEffect) dialogueUI.audioSource.pitch = pitch;
//        }
//        birdie = GameObject.FindWithTag("Birdie");
//        if (isUsingDecision)
//        {
//            DecisionScript.Instance.OnDecisionNegative += AfterDecision;
//            DecisionScript.Instance.OnDecisionPositive += AfterDecision;
//        }
//    }
//    public void Deactivate()
//    {
//        gameObject.SetActive(false);
//    }
//    Update is called once per frame
//    void Update()
//    {
//        switch (dialogueState)
//        {
//            case DialogueState.Nothing:
//                break;
//            case DialogueState.Talking:
//                Talking();
//                break;
//            case DialogueState.Typing:
//                TypeCharByChar(dialogues.dialogueLines[lineIndex].textContent);
//                break;
//            case DialogueState.Over:
//                SetToDefault();
//                if (previousTheme != null)
//                {
//                    AudioManagerScript.Instance.SetMusicVolume(audioVolumeTmp);
//                }
//                OnDialogueOver?.Invoke();
//                break;
//            case DialogueState.Transition:
//                AudioManagerScript.Instance.SetMusicVolume(audioVolumeTmp);
//                OnDialogueOver?.Invoke();
//                MoveAwayFromScene();
//                break;
//            case DialogueState.TextWithoutInteraction:
//                AudioManagerScript.Instance.SetMusicVolume(audioVolumeTmp);
//                SetUIActive();
//                if (!isTyping)
//                    StartCoroutine(TypeCharByChar2(dialogues.transitionLineText[lineIndex]));
//                break;


//            case DialogueState.Audio:
//                PlayAudio();
//                break;
//        }
//    }
//    private bool IsSkippingDialogue()
//    {
//        if (Input.GetKeyDown(KeyCode.E))
//        {
//            lineIndex++;
//            charIndex = 0;
//            stringBuilder.Clear();
//            dialogueState = DialogueState.Talking;
//            isFirstLine = true;

//            return true;
//        }
//        else return false;
//    }

//    private void SetToDefault()
//    {
//        isTalking = false;
//        lineIndex = 0;
//        isButtonPressed = true;
//        dialogueUI.nameText.text = dialogues.dialogueLines[lineIndex].speaker;
//        dialogueUI.textbox.text = dialogues.dialogueLines[lineIndex].textContent;
//        dialogueUI.namePanel.SetActive(false);
//        dialogueUI.textPanel.SetActive(false);
//        if (player != null)
//        {
//            player.GetComponent<PlayerMovementScript>().enabled = true;
//            player.GetComponent<GrappleScript>().enabled = true;
//        }
//        if (dialogueUI.buttons.Length != 0)
//        {
//            dialogueUI.buttons[0].transform.parent.gameObject.SetActive(false);
//        }
//        if (birdie != null && birdie.GetComponent<BirdieScript>().hasPickedPowerup) birdie.GetComponent<BirdieScript>().SetReticle();
//        dialogueState = DialogueState.Nothing;
//    }
//    private void OnTriggerEnter2D(Collider2D collision)
//    {
//        if (collision.transform.tag == "Player")
//        {

//            player = collision.gameObject;
//            if (dialogues.dialogueLines.Length == 0 && !isTransitioning)
//            {
//                dialogueState = DialogueState.TextWithoutInteraction;
//                OnDialogueStart?.Invoke();
//                previousTheme = AudioManagerScript.Instance.GetCurrentClip();
//                audioTime = AudioManagerScript.Instance.GetMusicAudio().time;
//                audioVolumeTmp = AudioManagerScript.Instance.GetMusicAudio().volume;
//            }
//            if (isPlayable && dialogueState != DialogueState.TextWithoutInteraction)
//            {
//                isTalking = true;
//                dialogueState = DialogueState.Talking;
//                isFirstLine = true;
//                player.GetComponent<PlayerMovementScript>().enabled = false;
//                player.GetComponent<GrappleScript>().enabled = false;
//                player.GetComponent<PlayerMovementScript>().SetForce(Vector2.zero);
//                if (AudioManagerScript.Instance.GetCurrentClip() != null)
//                {
//                    previousTheme = AudioManagerScript.Instance.GetCurrentClip();
//                }
//                StartConversation();

//                StartConversation();
//                audioTime = AudioManagerScript.Instance.GetMusicAudio().time;
//            }
//            else isFirstLine = false;
//        }
//    }
//    public void SetFirstDialogue()
//    {
//        isFirstLine = true;
//        isButtonPressed = true;
//        dialogueState = DialogueState.Talking;
//        lineIndex = 0;
//        StartConversation();
//        previousTheme = AudioManagerScript.Instance.GetCurrentClip();
//        audioTime = AudioManagerScript.Instance.GetMusicAudio().time;
//    }
//    public void ActivateDialogueByEvent()
//    {
//        isFirstLine = true;
//        dialogueState = DialogueState.Talking;
//        player = GameObject.FindWithTag(GameManagerScript.Instance.TagSO.PlayerTag);
//        if (player != null)
//        {
//            player.GetComponent<PlayerMovementScript>().enabled = false;
//            player.GetComponent<GrappleScript>().enabled = false;
//        }
//        StartConversation();
//    }
//    void AfterDecision()
//    {
//        if (isTalking)
//        {
//            if (dialogueState == DialogueState.Typing) lineIndex++;
//            dialogueUI.namePanel.SetActive(true);
//            dialogueUI.textPanel.SetActive(true);
//            textTypeTimer = textTypeTimerTmp;
//            charIndex = 0;
//            stringBuilder.Clear();
//            dialogueState = DialogueState.Talking;
//            isFirstLine = true;
//            isButtonPressed = true;
//        }
//    }
//    void Talking()
//    {
//        if (isPlayable)
//        {

//            if ((Input.GetKeyDown(KeyCode.E) || isFirstLine) && isButtonPressed)
//            {
//                OnDialogueStart?.Invoke();

//                if (previousTheme != null)
//                {
//                    AudioManagerScript.Instance.SetMusicVolume(.1f);
//                }
//                isFirstLine = false;
//                if (lineIndex < dialogues.dialogueLines.Length)
//                {
//                    dialogueUI.textbox.text = string.Empty;
//                    dialogueUI.nameText.text = dialogues.dialogueLines[lineIndex].speaker;
//                    dialogueUI.dialogueSprite.sprite = dialogues.dialogueLines[lineIndex].sprite;
//                    dialogueUI.audioSource.clip = dialogues.dialogueLines[lineIndex].audioClip;
//                    if (dialogues.dialogueLines[lineIndex].hasDecision && isUsingDecision)
//                    {
//                        dialogueUI.buttons[0].transform.parent.gameObject.SetActive(true);
//                        isButtonPressed = false;
//                    }
//                    else if (!dialogues.dialogueLines[lineIndex].hasDecision && isUsingDecision)
//                    { dialogueUI.buttons[0].transform.parent.gameObject.SetActive(false); }
//                }
//                else
//                {
//                    dialogueState = DialogueState.Over;
//                    if (!isReplayable)
//                    {
//                        isPlayable = false;
//                        DisableCollider();
//                    }
//                    if (isTransitioning)
//                    {
//                        dialogueState = DialogueState.Transition;
//                        lineIndex = 0;
//                        dialogueUI.textbox.text = string.Empty;
//                    }
//                    return;
//                }
//                ChangeImageColor();
//                dialogueState = DialogueState.Typing;
//            }

//        }

//    }

//    private void DisableCollider()
//    {
//        GetComponent<BoxCollider2D>().enabled = false;
//    }
//    private void StartConversation()
//    {
//        isTalking = true;
//        dialogueUI = GetComponent<DialogueUI>();
//        dialogueUI.namePanel.SetActive(true);
//        dialogueUI.textPanel.SetActive(true);
//        dialogueUI.nameText.text = dialogues.dialogueLines[0].speaker;
//        dialogueUI.dialogueSprite.sprite = dialogues.dialogueLines[0].sprite;
//        dialogueUI.textbox.text = string.Empty;
//        dialogueUI.audioSource.clip = dialogues.dialogueLines[0].audioClip;
//        dialogueUI.audioSource.Play();
//        previousTheme = AudioManagerScript.Instance.GetCurrentClip();
//        audioTime = AudioManagerScript.Instance.GetMusicAudio().time;
//        audioVolumeTmp = AudioManagerScript.Instance.GetMusicAudio().volume;
//    }
//    private void ChangeImageColor()
//    {
//        if (dialogues.dialogueLines[lineIndex].speaker == "Primo")
//        {
//            dialogueUI.pupilObject.SetActive(true);
//            dialogueUI.pupilObject.GetComponent<Image>().color = Color.black;

//            dialogueUI.dialogueSprite.color = Color.red;
//        }
//        else
//        {
//            dialogueUI.dialogueSprite.color = Color.white;
//            dialogueUI.pupilObject.SetActive(false);
//        }
//    }
//    public void MoveAwayFromScene()
//    {
//        GetComponent<BoxCollider2D>().enabled = false;
//        if (player != null)
//            player.transform.Translate(moveAwayDirection * 6 * Time.deltaTime);
//        player.GetComponent<PlayerMovementScript>().enabled = false;
//        dialogueUI.textPanel.SetActive(true);

//        if (!isTyping)
//        {
//            StartCoroutine(TypeCharByChar2(dialogues.transitionLineText[lineIndex]));
//            isMomTalkFinished = true;
//        }

//    }
//    private void MoveAwayTransition()
//    {
//        CanvasScript.instance.LoadNextSceneWithDelay("IntroScene");
//        dialogueState = DialogueState.Nothing;
//        CanvasScript.instance.PlayFadeOutAnimation();

//    }
//    private void SetUIActive()
//    {
//        dialogueUI.textPanel.SetActive(true);
//    }
//    IEnumerator TypeCharByChar2(string text)
//    {

//        dialogueUI.textbox.text = string.Empty;
//        StringBuilder stringBuilder = new StringBuilder();
//        isTyping = true;

//        audioSource.clip = transitionClip;
//        dialogueUI.namePanel.SetActive(false);
//        foreach (char c in text.ToCharArray())
//        {
//            stringBuilder.Append(c);
//            dialogueUI.textbox.text = stringBuilder.ToString();
//            dialogueUI.audioSource.clip = dialogues.transitionLineAudioClip[lineIndex];
//            PlayAudio();
//            yield return new WaitForSeconds(transitionTypeTimer);
//        }
//        lineIndex++;

//        isTyping = false;
//        if (lineIndex >= dialogues.transitionLineText.Length)
//        {
//            isTyping = true;
//            SetUIDeactive();
//            DisableCollider();
//            if (dialogueState == DialogueState.TextWithoutInteraction) dialogueState = DialogueState.Nothing;
//        }

//    }
//    private void SetUIDeactive()
//    {
//        dialogueUI.textPanel.SetActive(false);

//    }
//    private void PlayAudio()
//    {
//        if (!dialogueUI.audioSource.isPlaying)
//        {
//            SetAudioToDefault();
//            if (dialogues.dialogueLines.Length != 0)
//            {

//                dialogueUI.audioSource.volume = dialogues.dialogueLines[lineIndex].AudioVolume;
//                if (dialogues.dialogueLines[lineIndex].hasSpecialEffect)
//                {
//                    dialogueUI.audioSource.pitch = pitch;
//                    if (!isReadyShaking)
//                        return;
//                    StartCoroutine(ShakeCamera(.5f, 1.2f));
//                }
//            }
//            if (dialogues.SFXs.Length != 0)
//                if (dialogues.SFXs[lineIndex].hasSFX) dialogueUI.audioSource.pitch = dialogues.SFXs[lineIndex].pitch;

//            if (dialogueState == DialogueState.Transition || dialogueState == DialogueState.TextWithoutInteraction) dialogueUI.audioSource.volume = dialogues.transitionAudioVolume[lineIndex];

//            dialogueUI.audioSource.Play();

//        }
//    }
//    private void TypeCharByChar(string text)
//    {
//        char[] zeichen = text.ToCharArray();
//        textTypeTimer -= Time.deltaTime;
//        if (!dialogues.dialogueLines[lineIndex].hasDecision)
//            if (IsSkippingDialogue()) return;
//        if (textTypeTimer < 0)
//        {
//            textTypeTimer = textTypeTimerTmp;
//            if (charIndex < zeichen.Length)
//            {

//                stringBuilder.Append(zeichen[charIndex]);
//                dialogueUI.textbox.text = stringBuilder.ToString();
//                charIndex++;

//                PlayAudio();
//                dialogueState = DialogueState.Audio;
//            }
//            else
//            {
//                dialogueState = DialogueState.Talking;
//                lineIndex++;
//                charIndex = 0;

//                stringBuilder.Clear();

//            }
//        }
//    }

//    IEnumerator ShakeCamera(float duration, float magnitude)
//    {
//        Vector3 position = Camera.main.transform.position;

//        float elaspedTime = 0;
//        isReadyShaking = false;
//        while (elaspedTime < duration)
//        {
//            float x = UnityEngine.Random.Range(-1, 1f) * magnitude;
//            float y = UnityEngine.Random.Range(-1, 1f) * magnitude;
//            Camera.main.transform.position += new Vector3(x, y, 0);
//            elaspedTime += Time.deltaTime;
//            yield return null;
//        }
//        Camera.main.transform.position = position;
//        isReadyShaking = true;
//    }

//    private void SetAudioToDefault()
//    {
//        dialogueUI.audioSource.pitch = 1;
//        dialogueUI.audioSource.volume = 1;
//    }
//}
