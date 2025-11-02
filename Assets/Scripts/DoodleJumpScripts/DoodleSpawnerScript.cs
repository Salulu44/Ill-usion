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
    [SerializeField] private GameObject rocketPrefab;
    private float previousValueX;
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
        SpawnPlatforms(Vector3.zero);
    }
    private void Update()
    {
        if (Mathf.Abs(player.transform.position.y - highestPoint.y) < 10)
        {
            SpawnPlatforms(highestPoint);
        }
    }
    public void SpawnPlatforms(Vector3 spawnStart)
    {
        Vector3 spawnPosition = spawnStart;
        for (int i = 0; i < platformAmount; i++)
        {
            spawnPosition.x = Random.Range(-5, 5f);
            if (Mathf.Abs(spawnPosition.x - previousValueX) < 3f)
            {
                i--;
                continue;
            }
            spawnPosition.y += Random.Range(1, 2f);
            GameObject platform = Instantiate(platformPrefab, spawnPosition, Quaternion.identity);
            platform.GetComponent<DoodlePlatformScript>().SetRandomPlatformState();
            previousValueX = spawnPosition.x;
            highestPoint = platform.transform.position;
            platform.transform.SetParent(platformParent.transform);
            int random = Random.Range(0, 26);
            if (random == 25)
            {
                spawnPosition.x = spawnPosition.x = Random.Range(-5, 5f);
                Transform rocket = Instantiate(rocketPrefab, spawnPosition, Quaternion.identity).transform;
                rocket.SetParent(platformParent.transform);
            }
        }
    }
    public void RestartGame()
    {
        Camera.main.transform.position = Vector3.zero;
        Destroy(platformParent);
        platformParent = new GameObject("PlatformParent");
        SpawnPlatforms(Vector3.zero);
    }
}
