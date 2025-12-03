using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;
public class DoodleScoreScript : MonoBehaviour
{
    TextMeshProUGUI scoreText;
    Transform player;
    public HighScoreData highscoreData;
    [SerializeField] TextMeshProUGUI highscoreText;
    [SerializeField] GameObject tryAgain;
    TextMeshProUGUI tryAgainText;
    [SerializeField] VideoScript videoScript;
    [SerializeField] Transform winPosition;
    [SerializeField] SpawnEnemiesTestScript spawnEnemiesTestScript;
    [SerializeField, Range(0f, 1f)]
    float scoreChangeChance;
    [field: SerializeField] public int MaxScore { get; private set; }
    [field:SerializeField] public int CurrentScore { get; private set;}
    bool hasWon;
    string scoreWord;
    string[] possibleWordsVertically = { "Acrophobia? ", "Altitude? ", "A ", "Embarassment " };
    string[] possibleWordHorizontally = { "X-Coordinates!", "HAHA now On X", "How rizont are you?" };
    string[] possibleTryAgainWords = { "Lets try again!", "You are smashing it!", "Now or never!", "Try again" };
    RectTransform rectTransform;
    Transform canvasTr;
    Vector2 originPosition;
    Vector2 canvasSize;
    bool winningHorizontally;
    private void OnEnable()
    {
        highscoreData = SaveSystem.LoadHighScore(GameManagerScript.Instance.minigameSO.doodleJumpData);
    }
    void Start()
    {
        tryAgainText = tryAgain.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        scoreText = GetComponent<TextMeshProUGUI>();
        player = GameObject.FindWithTag("Player").transform;
        scoreWord = "Altitude : ";
        rectTransform = GetComponent<RectTransform>();
        originPosition = rectTransform.anchoredPosition;
        if (transform.parent.gameObject.GetComponent<Canvas>() != null)
        {
            canvasSize = new Vector2(transform.parent.GetComponent<RectTransform>().rect.width, transform.parent.GetComponent<RectTransform>().rect.height);
        }
        else Debug.Log("Your Parent isnt a Canvas, for your logic you need a Canvas as the parent!");
    }

    // Update is called once per frame
    void Update()
    {
        SetCurrentScore();
        ChecksWinning();
    }
    void ChecksWinning() 
    {
        if (hasWon)
        {
            VideoManagerScript.Instance.startJumpScare = true;
        }
        if (!hasWon && CurrentScore > MaxScore)
        {
            DoodleSpawnerScript.instance.shouldSpawn = false;
            videoScript.OnVideoStart += StartPhaseTwo;
            videoScript.OnVideoStop += StartEnemySpawn;
            Camera.main.GetComponent<DoodleCameraScript>().ShouldPlayerStayInViewPort(false);
            VideoManagerScript.Instance.PlayVideo(videoScript);
            DoodleSpawnerScript.instance.EndMiniGame();
            hasWon = true;
            if (winningHorizontally)
            {
                //Do Stuffy;
            }
            else
            {
                // Do Stuffy muffy as well
            }
        }
    }
    void SetCurrentScore() 
    {
        if (DoodleSpawnerScript.instance.spawnVertically)
        {
            CurrentScore = (int)(player.transform.position.y * 100);
            winningHorizontally = false;
        }
        else
        {
            winningHorizontally = true;
            CurrentScore = (int)(player.transform.position.x * 100);
        }
        scoreText.text = scoreWord + CurrentScore;
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
        if (DoodleSpawnerScript.instance.spawnVertically)
            return (int)(player.transform.position.y * 100);
        else 
            return (int)(player.transform.position.x * 100);
    }
    public void ChangeScoreWord() 
    {

        if (GameManagerScript.Instance.HasLuck(scoreChangeChance))
        {
            if (DoodleSpawnerScript.instance.spawnVertically) 
            {
                scoreWord = possibleWordsVertically[Random.Range(0, possibleWordsVertically.Length)];
            }
            else 
            {
                scoreWord = possibleWordHorizontally[Random.Range(0, possibleWordHorizontally.Length)];
            }
            rectTransform.anchoredPosition = new Vector2(Random.Range(-canvasSize.x, 0), Random.Range(-canvasSize.y, 0));
            tryAgainText.text = possibleTryAgainWords[Random.Range(0, possibleTryAgainWords.Length)];
        }
        else
        {
            scoreWord = "Altitude : ";
            rectTransform.anchoredPosition = originPosition;
        }
    }
}
