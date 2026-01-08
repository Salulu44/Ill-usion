using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DoodlePlayButtonScript : MonoBehaviour, IPointerEnterHandler
{
    [SerializeField] Sprite playButtonEndSprite;
    [SerializeField] float directionAmplifier;
    [SerializeField] GameObject minigameObject;
    public int lifes;
    Image playButtonRenderer;
    [HideInInspector] public bool lostAllLife;
    RectTransform doodlePlayRectTr;
    Vector2 canvasResolution;
    Animator doodlePlayAnim;
    Vector3[] directions = {Vector2.down, Vector2.up, Vector2.left,Vector2.right, Vector2.zero};

    void Start()
    {
        playButtonRenderer = GetComponent<Image>();
        doodlePlayRectTr = GetComponent<RectTransform>();
        canvasResolution = doodlePlayRectTr.root.GetComponent<RectTransform>().rect.size;
        Debug.Log("Resolution " + canvasResolution);
        doodlePlayAnim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        LifeCheck();
        UIExtensions.UIOrientation(doodlePlayRectTr, canvasResolution, 100);
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

        gameObject.transform.parent.gameObject.SetActive(false);
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
            Vector2 currentDirection = new Vector2(directions[Random.Range(0, directions.Length)].x * directionAmplifier, directions[Random.Range(0, directions.Length)].y * directionAmplifier);
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
