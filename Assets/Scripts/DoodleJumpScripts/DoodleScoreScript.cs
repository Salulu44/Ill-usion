using System.Text;
using TMPro;
using UnityEngine;

public class DoodleScoreScript : MonoBehaviour
{
    TextMeshProUGUI scoreText;
    Transform player;
    public HighScoreData highscoreData;
    [SerializeField] TextMeshProUGUI highscoreText;
    [SerializeField] GameObject tryAgain;
    [SerializeField] VideoScript videoScript;
    [SerializeField] Transform winPosition;
    [SerializeField] SpawnEnemiesTestScript spawnEnemiesTestScript;
    [field: SerializeField] public int MaxScore { get; private set; }
   [field:SerializeField] public int CurrentScore { get; private set;}
    bool hasWon;
    private void OnEnable()
    {
        highscoreData = SaveSystem.LoadHighScore(GameManagerScript.Instance.minigameSO.doodleJumpData);
        print(highscoreData.MiniGame());
    }
    void Start()
    {
        scoreText = GetComponent<TextMeshProUGUI>();
        player = GameObject.FindWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        CurrentScore = (int)(player.transform.position.y * 100);
        scoreText.text = "Altitude : " + CurrentScore;

        if(!hasWon && CurrentScore > MaxScore) 
        {
            DoodleSpawnerScript.instance.shouldSpawn = false;
            videoScript.OnVideoStart += StartPhaseTwo;
            videoScript.OnVideoStop += StartEnemySpawn;
            Camera.main.GetComponent<DoodleCameraScript>().ShouldPlayerStayInViewPort(false);
            VideoManagerScript.Instance.PlayVideo(videoScript);
            DoodleSpawnerScript.instance.EndMiniGame();
            hasWon = true;
        }
    }
    public void StartEnemySpawn() 
    {
        spawnEnemiesTestScript.enabled = true;
        player.GetComponent<Collider2D>().enabled = true;
        player.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
        videoScript.OnVideoStart -= StartEnemySpawn;
    }
    public void StartPhaseTwo() 
    {
        player.position = winPosition.position;
        player.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
        player.GetComponent<Collider2D>().enabled = false;
        videoScript.OnVideoStart -= StartPhaseTwo;
    }
    public void AddHighscoreText()
    {
        highscoreText.gameObject.SetActive(true);
        tryAgain.SetActive(true);
        StringBuilder leaderboard = new StringBuilder();
        for (int i = 0; i < highscoreData.highscores.Length; i++)
        {
            leaderboard.Append($"{i + 1}. {highscoreData.highscores[i]}\n");
        }
        highscoreText.text = leaderboard.ToString();
    }
    public int GetScore()
    {
        return (int)(player.transform.position.y * 100);
    }
}
