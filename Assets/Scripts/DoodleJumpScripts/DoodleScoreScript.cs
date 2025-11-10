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
    public readonly int MAXSCORE = 10000;
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

        if(!hasWon && CurrentScore > MAXSCORE) 
        {
            DoodleSpawnerScript.instance.shouldSpawn = false;
            videoScript.OnVideoStart += SetWinPosition;
            Camera.main.GetComponent<DoodleCameraScript>().ShouldPlayerStayInViewPort(false);
            VideoManagerScript.Instance.PlayVideo(videoScript);
            DoodleSpawnerScript.instance.EndMiniGame();
            hasWon = true;
        }
    }
    public void SetWinPosition() 
    {
        player.position = winPosition.position;
        videoScript.OnVideoStart -= SetWinPosition;
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
