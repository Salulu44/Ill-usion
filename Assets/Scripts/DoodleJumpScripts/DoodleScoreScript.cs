using TMPro;
using UnityEngine;

public class DoodleScoreScript : MonoBehaviour
{
    private TextMeshProUGUI scoreText;
    private Transform player;
    //[HideInInspector] public HighscoreData highscoreData = new HighscoreData();
    [SerializeField] private TextMeshProUGUI highscoreText;
    void Start()
    {
        scoreText = GetComponent<TextMeshProUGUI>();
        player = GameObject.FindWithTag("Player").transform;

    }

    // Update is called once per frame
    void Update()
    {
        scoreText.text = "Altitude : " + (int)(player.transform.position.y * 100);
    }
    public void AddHighscoreText()
    {
        scoreText.text = "Altitude : " + (int)(player.transform.position.y * 100);
        //highscoreText.text = "Highscore : " + SaveHighScore.LoadSystem().highscores[0];
    }
    public int GetScore()
    {
        return (int)(player.transform.position.y * 100);
    }
}
