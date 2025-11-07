using UnityEngine;

public class HighScoreData
{
    public int[] highscores = new int[5];
    public string miniGame;
    
    public HighScoreData(string miniGame) 
    {
        Debug.Log("i was in here");
        this.miniGame = miniGame;

    }
    public string MiniGame() => miniGame;
    public void AddNewHighscore(int highscore)
    {

        for (int i = 0; i < highscores.Length; i++)
        {
            if (highscores[i] == 0)
            {
                highscores[i] = highscore;
                SaveSystem.SaveHighScore(this,miniGame);
                break;
            }
            else if (highscores[i] < highscore)
            {
                int newHighscoreIndex = i;
                for (int j = highscores.Length - 1; j > newHighscoreIndex; j--)
                {
                    highscores[j] = highscores[j - 1];
                }
                highscores[newHighscoreIndex] = highscore;
                SaveSystem.SaveHighScore(this,miniGame);
                break;
            }
        }

    }
    public int GetBiggestScore()
    {
        int biggestNumber = 0;
        for (int i = 0; i < highscores.Length; i++)
        {
            if (biggestNumber < highscores[i])
            {
                biggestNumber = highscores[i];
            }
        }
        return biggestNumber;
    }
    private void PrintArray(int[] array)
    {
        foreach (int i in array)
        {
            Debug.Log(string.Join(", ", array));
        }
    }
}
