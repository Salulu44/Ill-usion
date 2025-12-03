using UnityEngine;

public class SpawnEnemiesTestScript : MonoBehaviour
{
    
    [SerializeField] float enemyspawnTimer;
    float enemySpawnTimerTmp;
    float[] spawnPositions = { -Screen.width, Screen.width, 0, Screen.height };
    public static int enemyCounter;
    void Start()
    {
        enemySpawnTimerTmp = enemyspawnTimer;
    }

    // Update is called once per frame
    void Update()
    {
        SpawnEnemies();

    }
    void SpawnEnemies() 
    {
        enemyspawnTimer -= Time.deltaTime;
        if (enemyspawnTimer <= 0 && enemyCounter <= 10)
        {
            enemyspawnTimer = enemySpawnTimerTmp;
            Vector3 position = new Vector3(0, 0, 0);
            position.x = spawnPositions[Random.Range(0, 2)];
            position.y = spawnPositions[Random.Range(2, 4)];
            Vector3 spawnPosition = Camera.main.ScreenToWorldPoint(position);
            GameObject enemy =  Instantiate(DoodleSpawnerScript.instance.enemies[Random.Range(0, DoodleSpawnerScript.instance.enemies.Length)], new Vector3(spawnPosition.x, spawnPosition.y, spawnPosition.z), Quaternion.identity).gameObject;
            Destroy(enemy, 5);
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            Vector3 position = new Vector3(0, 0, 0);
            position.x = spawnPositions[Random.Range(0, 2)];
            position.y = spawnPositions[Random.Range(2, 4)];
            Vector3 spawnPosition = Camera.main.ScreenToWorldPoint(position);
            Instantiate(DoodleSpawnerScript.instance.enemies[Random.Range(0, DoodleSpawnerScript.instance.enemies.Length)], new Vector3(spawnPosition.x, spawnPosition.y, spawnPosition.z), Quaternion.identity);

        }
    }
}
