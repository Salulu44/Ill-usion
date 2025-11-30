using Unity.Cinemachine;
using UnityEngine;

public class DoodleGameOverScript : MonoBehaviour
{
    [SerializeField] GameObject loosePanel;
    [SerializeField] DoodleScoreScript scoreTextScript;
    [SerializeField] private DoodleSpawnerScript spawnerScript;
    private GameObject player;
    private SpriteRenderer playerSpriteRenderer;
    [SerializeField,Range(0f,1f)] float jumpScareChance;
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
        transform.position = new Vector3(Camera.main.transform.position.x, deadZoneScreenPosition.y - 1.1f, 0);
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
        if(GameManagerScript.Instance.HasLuck(jumpScareChance))
        VideoManagerScript.Instance.startJumpScare = true;
        player.transform.position = Vector3.zero;
    }
}
