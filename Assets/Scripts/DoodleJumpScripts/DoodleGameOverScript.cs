using UnityEngine;

public class DoodleGameOverScript : MonoBehaviour
{
    [SerializeField] GameObject loosePanel;
    [SerializeField] DoodleScoreScript scoreTextScript;
    [SerializeField] private DoodleSpawnerScript spawnerScript;
    private GameObject player;
    void Start()
    {

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
            collision.transform.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
            loosePanel.SetActive(true);
            scoreTextScript.highscoreData.AddNewHighscore(scoreTextScript.GetScore());
            scoreTextScript.AddHighscoreText();
            player = collision.gameObject;
        }
    }
    public void Restart()
    {
        player.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
        player.transform.position = Vector3.zero;
        loosePanel.SetActive(false);
        spawnerScript.RestartGame();
    }
}
