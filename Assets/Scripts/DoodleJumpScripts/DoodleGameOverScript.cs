using UnityEngine;

public class DoodleGameOverScript : MonoBehaviour
{
    [SerializeField] GameObject loosePanel;
    [SerializeField] DoodleScoreScript scoreTextScript;
    [SerializeField] private DoodleSpawnerScript spawnerScript;
    private GameObject player;
    private SpriteRenderer playerSpriteRenderer;
    void Start()
    {
        player = GameObject.FindWithTag(GameManagerScript.Instance.tagSO.playerTag);
        player.GetComponent<HealthScript>().OnDeath += Lost;
        playerSpriteRenderer = player.GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 deadZoneScreenPosition = Camera.main.ScreenToWorldPoint(Vector3.zero);
        transform.position = new Vector3(0, deadZoneScreenPosition.y - 1.25f, 0);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.tag == "Player")
        {
            Lost();
        }
    }
    public void Lost() 
    {
        player.SetActive(false);
        loosePanel.SetActive(true);
        scoreTextScript.highscoreData.AddNewHighscore(scoreTextScript.GetScore());
        scoreTextScript.AddHighscoreText();
    }
    public void Restart()
    {
        playerSpriteRenderer.color = player.GetComponent<DoodlePlayerScript>().playerColor;
        player.GetComponent<HealthScript>().Resurrect();
        loosePanel.SetActive(false);
        player.SetActive(true);
        spawnerScript.RestartGame();
        player.transform.position = Vector3.zero;
    }
}
