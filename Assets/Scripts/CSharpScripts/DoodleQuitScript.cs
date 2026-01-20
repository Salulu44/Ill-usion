using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DoodleQuitScript : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] Sprite[] doodlequitSprites;
    [SerializeField] DialogueScript doodlequitMenuScript;
    [SerializeField] DialogueScript doodlehitQuitButtonScript;
    RectTransform quitButtonTr;
    Image doodlequitRenderer;
    int spriteIndex;
    [SerializeField] GameObject lifeBar;
    [SerializeField] float invincibleTimer;
    float invincibleTimerTmp;
    Vector2 canvasResolution;
    bool startInvincibility;
    Vector3[] directions = { Vector2.down, Vector2.up, Vector2.left, Vector2.right };
    [SerializeField] float directionAmplifier;
    void Start()
    {
        canvasResolution = transform.root.GetComponent<RectTransform>().rect.size;
        quitButtonTr = GetComponent<RectTransform>();
        invincibleTimerTmp = invincibleTimer;
        invincibleTimer = 0;
        doodlequitRenderer = GetComponent<Image>();
     //   quitMenuScript.OnWhileDialogue += CancelCurrentDialogueEvent;
        doodlehitQuitButtonScript.OnEndDialogue += HitQuitButtonEvent;
        Debug.Log("HIIIIIIIIIIIIIIIIII");
    }

    private void HitQuitButtonEvent()
    {
        Debug.Log("End of dialogue");
      //  hitQuitButtonScript.enabled = true;
        doodlequitMenuScript.enabled = true;
        doodlequitMenuScript.StartDialogue();
       // hitQuitButtonScript.StartDialogue();
    }

    // Update is called once per frame
    void Update()
    {
        UIOrientation();
        QuitButtonLogic();
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        if(spriteIndex < doodlequitSprites.Length) 
        {
            doodlequitRenderer.sprite = doodlequitSprites[spriteIndex++];
            doodlehitQuitButtonScript.enabled = true;
            doodlehitQuitButtonScript.StartDialogue();
            startInvincibility = true;
            doodlequitMenuScript.enabled = false;
            Debug.Log("Hi " +eventData);
        }

    }
    void UIOrientation()
    {
        //if (startInvincibility) 
        //{
        //    UIExtensions.SetUIOrientation(quitButtonTr, canvasResolution);
        //}

    }
    void QuitButtonLogic()
    {
        if (startInvincibility) 
        {
            invincibleTimer -= Time.deltaTime;
            float ratio = invincibleTimer / invincibleTimerTmp;
            Color col = doodlequitRenderer.color;
            col.a = ratio;
            doodlequitRenderer.color = col;
            if (invincibleTimer < 0) 
            {
                Debug.Log("YEAH");
                invincibleTimer = invincibleTimerTmp;
                Vector2 currentDirection = new Vector2(directions[UnityEngine.Random.Range(0, directions.Length)].x * directionAmplifier, directions[UnityEngine.Random.Range(0, directions.Length)].y * directionAmplifier);
                Vector2 targetPosition = transform.position + (Vector3)currentDirection;
                transform.position = targetPosition;
                Color colorFull = doodlequitRenderer.color;
                colorFull.a = 1f;
                doodlequitRenderer.color = colorFull;
            }
        }
    }
    private void CancelCurrentDialogueEvent()
    {
        if (!doodlehitQuitButtonScript.IsDialogueFinished) 
        {
            doodlequitMenuScript.enabled = false;
        }

    }
    private void OnDestroy()
    {
        Debug.Log("Ich wurde destroyed");
    }
}
