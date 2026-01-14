using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DoodleSpawnerScript : MonoBehaviour
{
    public static DoodleSpawnerScript instance;
    public GameObject platformPrefab;
    [SerializeField] private int platformAmount;
    private GameObject platformParent;
    private Vector3 highestPoint;
    [HideInInspector] public GameObject player;
    [SerializeField] private Transform rocketPrefab;
    [SerializeField] GameObject exitGateKeeper;
    private float previousValueX;
    [SerializeField] GameObject highScoreText;
    [SerializeField] GameObject tryAgainButton;
    [SerializeField] GameObject loosePanel;
    [SerializeField] public Enemy[] enemies;
    [SerializeField,Range(0f,1f)] float spawnChangeChance;
    [SerializeField, Range(0f, 1f)] float exitGateKeeperChance;
    [SerializeField] GameObject doodleQuitButton;
    [SerializeField] DialogueScript quitDialogueScript;
    bool isQutting;
    public bool shouldSpawn = true;
    public bool spawnVertically = true;
    GameObject leftBorder;
    GameObject rightBorder;
    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        instance = this;
    }
    void OpenQuitPanel() 
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Time.timeScale = isQutting ? 1f: 0f;
            isQutting = !isQutting;
            if (isQutting)
            {
                quitDialogueScript.enabled = true;
                quitDialogueScript.StartDialogue();
            }
            else 
            {
                quitDialogueScript.CloseCanvas();
                quitDialogueScript.enabled = false;
            }
            doodleQuitButton.SetActive(isQutting);
            doodleQuitButton.transform.parent.gameObject.SetActive(isQutting);
        }
    }
    private void Start()
    {
       if(Camera.main.TryGetComponent(out DoodleCameraScript doodleCameraScript))
        {
            doodleCameraScript.OnScreenExit += SpawnEnemiesEvent;
        }
        else 
        {
            Debug.Log("DoodleScript not on tze camera");
        }
        player = GameObject.FindGameObjectWithTag(GameManagerScript.Instance.tagSO.playerTag);

        platformParent = new GameObject("PlatformParent");
        SceneManager.MoveGameObjectToScene(platformParent, SceneManager.GetSceneByName("DoodleJump"));
        SpawnPlatformsVertically(Vector3.zero);
    }
    private void Update()
    {
        if (shouldSpawn && Mathf.Abs(player.transform.position.y - highestPoint.y) < 10 && spawnVertically)
        {
            SpawnPlatformsVertically(highestPoint);
        }
        else if (shouldSpawn && Mathf.Abs(player.transform.position.x - highestPoint.x) < 10 && !spawnVertically)
        {
            SpawnHorizontally(highestPoint);       
        }
        if(leftBorder && rightBorder != null) 
        {
            leftBorder.transform.position = new Vector3(leftBorder.transform.position.x, player.transform.position.y, 0);
            rightBorder.transform.position = new Vector3(rightBorder.transform.position.x, player.transform.position.y, 0);
        }
        OpenQuitPanel();
    }
    public void SpawnEnemiesEvent() 
    {
        if (GameManagerScript.Instance.HasLuck(DoodleScoreScript.winDestinationRatio)) 
        {
            Vector3 leftBorderPosition = Camera.main.ScreenToWorldPoint(new Vector3(0, Screen.height / 2, 0));
            Vector3 rightBorderPosition = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height / 2, 0));
            leftBorder = Instantiate(exitGateKeeper, new Vector3(leftBorderPosition.x, leftBorderPosition.y, 0), Quaternion.identity);
            rightBorder = Instantiate(exitGateKeeper, new Vector3(rightBorderPosition.x, rightBorderPosition.y, 0), Quaternion.identity);
            leftBorder.transform.SetParent(platformParent.transform);
            rightBorder.transform.SetParent(platformParent.transform);
            Destroy(leftBorder, 3);
            Destroy(rightBorder, 3);
        }
      Vector3 topScreenPosition = Camera.main.ScreenToWorldPoint(new Vector3(0, Screen.height + 200, 0));
      Enemy enemy =  Instantiate(enemies[UnityEngine.Random.Range(0, enemies.Length)],new Vector3(topScreenPosition.x,topScreenPosition.y,0),Quaternion.identity);
      enemy.transform.SetParent(platformParent.transform);
    }
    public void SpawnHorizontally(Vector3 spawnStart) 
    {
        Vector3 spawnPosition = spawnStart;
        Vector2[] moveDirections = { Vector2.right, Vector2.left, Vector2.down,Vector2.up};
        for (int i = 0; i < platformAmount; i++)
        {
            spawnPosition.x += UnityEngine.Random.Range(1, 5f);
            if (Mathf.Abs(spawnPosition.x - previousValueX) < 3f)
            {
                i--;
                continue;
            }
            spawnPosition.y = UnityEngine.Random.Range(-3, 3f);
            GameObject platform = Instantiate(platformPrefab, spawnPosition, Quaternion.identity);
            platform.GetComponent<DoodlePlatformScript>().SetRandomPlatformState();
            platform.GetComponent<DoodlePlatformScript>().SetMoveDirection(moveDirections[UnityEngine.Random.Range(0, moveDirections.Length)]);
            previousValueX = spawnPosition.x;
            highestPoint = platform.transform.position;
            platform.transform.SetParent(platformParent.transform);
            int random = UnityEngine.Random.Range(0, 26);
            if (random == 25)
            {
                while (Mathf.Abs(spawnPosition.x - previousValueX) < 3f)
                {
                    spawnPosition.x += UnityEngine.Random.Range(1, 5f);
                }
                Transform rocket = Instantiate(rocketPrefab, spawnPosition, Quaternion.identity);
                rocket.SetParent(platformParent.transform);
                rocket.GetComponent<DoodlePowerUpScript>().forceDirection = Vector2.right;
            }
            else if (random <= 10)
            {  
                while (Mathf.Abs(spawnPosition.x - previousValueX) < 3f)
                {
                    spawnPosition.x +=  UnityEngine.Random.Range(1, 5f);
                }
                random = UnityEngine.Random.Range(0, enemies.Length);
                Transform enemy = Instantiate(enemies[random], spawnPosition, Quaternion.identity).transform;
                enemy.SetParent(platformParent.transform);
            }
        }
    }
    public void SpawnPlatformsVertically(Vector3 spawnStart)
    {
        Vector3 spawnPosition = spawnStart;
        Vector2[] moveDirections = { Vector2.left, Vector2.right,Vector2.down,Vector2.up};
        for (int i = 0; i < platformAmount; i++)
        {
            spawnPosition.x = UnityEngine.Random.Range(-5, 5f);
            if (Mathf.Abs(spawnPosition.x - previousValueX) < 3f)
            {
                i--;
                continue;
            }
            spawnPosition.y += UnityEngine.Random.Range(2, 3f);
            GameObject platform = Instantiate(platformPrefab, spawnPosition, Quaternion.identity);
            platform.GetComponent<DoodlePlatformScript>().SetRandomPlatformState();
            platform.GetComponent<DoodlePlatformScript>().SetMoveDirection(moveDirections[UnityEngine.Random.Range(0, moveDirections.Length)]);
            previousValueX = spawnPosition.x;
            highestPoint = platform.transform.position;
            platform.transform.SetParent(platformParent.transform);
            int random = UnityEngine.Random.Range(0, 26);
            if (random == 25)
            {
                while(Mathf.Abs(spawnPosition.x - previousValueX) < 3f) 
                {
                    spawnPosition.x = UnityEngine.Random.Range(-5, 5f);
                }
                Transform rocket = Instantiate(rocketPrefab, spawnPosition, Quaternion.identity);
                rocket.SetParent(platformParent.transform);
            }
            else if(random <= 5) 
            {
                while (Mathf.Abs(spawnPosition.x - previousValueX) < 3f)
                {
                    spawnPosition.x = UnityEngine.Random.Range(-5, 5f);
                }
                random = UnityEngine.Random.Range(0, enemies.Length);
                Transform enemy = Instantiate(enemies[random], spawnPosition, Quaternion.identity).transform;
                enemy.SetParent(platformParent.transform);
            }
        }
    }
    public void RestartGame()
    {
        Camera.main.transform.position = Vector3.zero;
        Destroy(platformParent);
        platformParent = new GameObject("PlatformParent");

        highScoreText.SetActive(false);
        tryAgainButton.SetActive(false);
        loosePanel.SetActive(false);
        if (GameManagerScript.Instance.HasLuck(spawnChangeChance))
        {
            spawnVertically = false;
            SpawnHorizontally(Vector3.zero); 
        }
        else
        {
            spawnVertically = true;
            SpawnPlatformsVertically(Vector3.zero);
        }
    }
    public void EndMiniGame() 
    {
        shouldSpawn = false;
        Destroy(platformParent);
    }
    private void Restart() 
    {

    }
}
