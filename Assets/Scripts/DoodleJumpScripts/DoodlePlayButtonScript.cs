using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DoodlePlayButtonScript : MonoBehaviour, IPointerEnterHandler
{
    [SerializeField] Sprite playButtonEndSprite;
    [SerializeField] float directionAmplifier;
    [SerializeField] GameObject minigameObject;
    [SerializeField] Color damageColor;
    [SerializeField] float damageColorTimer;
    [SerializeField] GameObject doodleUIBackground;
    [SerializeField] public DialogueScript dialogueScript;
    Color defaultColor;
    float damageColorTimerTmp;
    [HideInInspector] public bool lostAllLife;
    bool setDamageColor;
    public int lifes;
    Image playButtonRenderer;
    RectTransform doodlePlayRectTr;
    Vector2 canvasResolution;
    Animator doodlePlayAnim;
    Vector3[] directions = {Vector2.down, Vector2.up, Vector2.left,Vector2.right};
    Rigidbody2D playButtonRb;
    void Start()
    {
        playButtonRenderer = GetComponent<Image>();
        defaultColor = playButtonRenderer.color;
        doodlePlayRectTr = GetComponent<RectTransform>();
        canvasResolution = doodlePlayRectTr.root.GetComponent<RectTransform>().rect.size;
        doodlePlayAnim = GetComponent<Animator>();
        playButtonRb = GetComponent<Rigidbody2D>();
        damageColorTimerTmp = damageColorTimer;
    }

    // Update is called once per frame
    void Update()
    {
        LifeCheck();
        HandleOrientation();
        SetColorToDefault();
    }
    void HandleOrientation()
    {
        UIExtensions.VectorOrientation vectorOrientation;
        UIExtensions.UIOrientation(doodlePlayRectTr, canvasResolution, out vectorOrientation, 100);
        if(vectorOrientation != UIExtensions.VectorOrientation.Inside) 
        {
            playButtonRb.linearVelocity = new Vector2(playButtonRb.linearVelocity.x * .5f, playButtonRb.linearVelocityY * .5f);
        }
    }
    void LifeCheck()
    {
        if (lifes == 0 && !lostAllLife)
        {
            doodlePlayAnim.SetBool("GotHit", false);
            playButtonRenderer.sprite = playButtonEndSprite;
            lostAllLife = true;
            doodlePlayAnim.enabled = false;
            // I need to deactivate the animator to set the sprite properly
            Button button = gameObject.AddComponent<Button>();
            GetComponent<Image>().SetNativeSize();
            button.onClick.AddListener(StartDoodleGame);
            GetComponent<BoxCollider2D>().size = new Vector2(doodlePlayRectTr.rect.width, doodlePlayRectTr.rect.height);
        }
    }
    public void SetDamageColor() 
    {
        if (!setDamageColor) 
        {
            playButtonRenderer.color = damageColor;
            setDamageColor = true;
            dialogueScript.enabled = true;
            dialogueScript.StartDialogue();
            GetComponent<Collider2D>().enabled = false;
        }
    }
    void SetColorToDefault()
    {
        if (setDamageColor) 
        {
            damageColorTimer -= Time.deltaTime;
            if(damageColorTimer <= 0) 
            {
                GetComponent<Collider2D>().enabled = true;
                damageColorTimer = damageColorTimerTmp;
                playButtonRenderer.color = defaultColor;
                setDamageColor = false;
            }
        }
    }
    public void StartDoodleGame()
    {
        Debug.Log("Start Game");
        minigameObject.SetActive(true);
        if (Camera.main.gameObject.TryGetComponent(out DoodleCameraScript doodleCameraScript))
        {
            doodleCameraScript.SetPlayerReference();
            Debug.Log("Set Player reference");
        }
        else
        {
            Debug.Log("There is no " + typeof(DoodleCameraScript).ToString() + " attached to this gameobject");
        }

        doodleUIBackground.SetActive(false);
        transform.parent.gameObject.SetActive(false);
    }
    public void MoveAway()
    {
        if (lostAllLife)
        {
            //I need to maybe implement that the button always need to be inside the canvas, so that the players dont get softlocked
            Debug.Log($"Active: {gameObject.activeInHierarchy}, LostLife: {lostAllLife}, TimeScale: {Time.timeScale}, Tweening: {DOTween.IsTweening(transform)}");

            if (!lostAllLife || !gameObject.activeInHierarchy)
            {
                Debug.Log("Abgebrochen: !lostAllLife oder inaktiv");
                return;
            }

            if (DOTween.IsTweening(transform))
            {
                Debug.Log("Tween läuft schon");
                return;
            }
            Vector2 currentDirection = new Vector2(directions[UnityEngine.Random.Range(0, directions.Length)].x * directionAmplifier, directions[UnityEngine.Random.Range(0, directions.Length)].y * directionAmplifier);
            Vector2 targetPosition = transform.position + (Vector3) currentDirection;
            transform.DOMove(targetPosition, .2f);
            Debug.Log("Hiiiii");
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        MoveAway();
    }
}
