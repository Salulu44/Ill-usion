using TMPro;
using UnityEngine;

public class DoodleSpawnerScript : MonoBehaviour
{
    public static DoodleSpawnerScript instance;
    public GameObject platformPrefab;
    [SerializeField] private int platformAmount;
    private GameObject platformParent;
    private Vector3 highestPoint;
    private Vector3 lowestPoint;
    [HideInInspector] public GameObject player;
    [SerializeField] private Transform rocketPrefab;
    private float previousValueX;
    [SerializeField] GameObject highScoreText;
    [SerializeField] GameObject tryAgainButton;
    [SerializeField] GameObject loosePanel;
    [SerializeField] public Enemy[] enemies;
    [SerializeField,Range(0f,1f)] float spawnChangeChance;
    public bool shouldSpawn = true;
    public bool spawnVertically = true;
    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        instance = this;
    }
    private void Start()
    {
        player = GameObject.FindGameObjectWithTag(GameManagerScript.Instance.tagSO.playerTag);
        platformParent = new GameObject("PlatformParent");
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
    }
    public void SpawnHorizontally(Vector3 spawnStart) 
    {
        Debug.Log("Spawn Horizontally");
        Vector3 spawnPosition = spawnStart;
        for (int i = 0; i < platformAmount; i++)
        {
            spawnPosition.x += Random.Range(1, 5f);
            if (Mathf.Abs(spawnPosition.x - previousValueX) < 3f)
            {
                i--;
                continue;
            }
            spawnPosition.y = Random.Range(-3, 3f);
            GameObject platform = Instantiate(platformPrefab, spawnPosition, Quaternion.identity);
            platform.GetComponent<DoodlePlatformScript>().SetRandomPlatformState();
            previousValueX = spawnPosition.x;
            highestPoint = platform.transform.position;
            Debug.Log("Highest Point " + highestPoint); 
            platform.transform.SetParent(platformParent.transform);
            int random = Random.Range(0, 26);
            if (random == 25)
            {
                while (Mathf.Abs(spawnPosition.x - previousValueX) < 3f)
                {
                    spawnPosition.x += Random.Range(1, 5f);
                }
                Transform rocket = Instantiate(rocketPrefab, spawnPosition, Quaternion.identity);
                rocket.SetParent(platformParent.transform);
                rocket.GetComponent<DoodlePowerUpScript>().forceDirection = Vector2.right;
            }
            else if (random <= 5)
            {
                Debug.Log(Random.state);
               
                while (Mathf.Abs(spawnPosition.x - previousValueX) < 3f)
                {
                    spawnPosition.x += Random.Range(1, 5f);
                }
                random = Random.Range(0, enemies.Length);
                Transform enemy = Instantiate(enemies[random], spawnPosition, Quaternion.identity).transform;
                enemy.SetParent(platformParent.transform);
            }
        }
    }
    public void SpawnPlatformsVertically(Vector3 spawnStart)
    {
        Debug.Log("Spawn Vertically");
        Vector3 spawnPosition = spawnStart;
        for (int i = 0; i < platformAmount; i++)
        {
            spawnPosition.x = Random.Range(-5, 5f);
            if (Mathf.Abs(spawnPosition.x - previousValueX) < 3f)
            {
                i--;
                continue;
            }
            spawnPosition.y += Random.Range(2, 3f);
            GameObject platform = Instantiate(platformPrefab, spawnPosition, Quaternion.identity);
            platform.GetComponent<DoodlePlatformScript>().SetRandomPlatformState();
            previousValueX = spawnPosition.x;
            highestPoint = platform.transform.position;
            platform.transform.SetParent(platformParent.transform);
            int random = Random.Range(0, 26);
            if (random == 25)
            {
                while(Mathf.Abs(spawnPosition.x - previousValueX) < 3f) 
                {
                    spawnPosition.x = Random.Range(-5, 5f);
                }
                Transform rocket = Instantiate(rocketPrefab, spawnPosition, Quaternion.identity);
                rocket.SetParent(platformParent.transform);
            }
            else if(random <= 5) 
            {
                while (Mathf.Abs(spawnPosition.x - previousValueX) < 3f)
                {
                    spawnPosition.x = Random.Range(-5, 5f);
                }
                random = Random.Range(0, enemies.Length);
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
